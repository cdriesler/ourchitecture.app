using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;
using Ourchitecture.Api.Protocols.Motley.Vendor;

namespace Ourchitecture.Api.Protocols.Motley
{
    public static class VendorSchema
    {
        public static VendorManifest Solve(VendorRequest request)
        {
            var result = new VendorManifest();

            //Parse inputs from request for dimensions and system constraints.
            ParseBoundaryInput(result, request.Boundary);
            ParseCellProfileInput(result, request.Cell);
            ParsePathInput(result, request.Path);

            //Generate initial massing.
            GeneratePathSamplePoints(result);
            GeneratePathFlanks(result);
            GenerateMarketCells(result);
            GenerateRoofMass(result);

            //Sculpt out spaces from overall massing.
            GenerateMarketSolid(result);
            SculptRoofArches(result);
            SculptRoofWindows(result);


            return result;
        }

        private static void ParseBoundaryInput(VendorManifest res, Curve bounds)
        {
            res.PlanarBounds = bounds;

            var box = bounds.GetBoundingBox(Plane.WorldXY);

            res.VolumeBounds = new BoundingBox(box.Min, new Point3d(box.Max.X, box.Max.Y, 100)).ToBrep();
        }

        private static void ParseCellProfileInput(VendorManifest res, Curve cell)
        {
            res.CellProfile = cell;

            var box = cell.GetBoundingBox(Plane.WorldXY);
            res.CellProfileCenter = box.Center;
            res.CellProfileDepth = box.Max.Y - box.Min.Y;
            res.CellProfileWidth = box.Max.X - box.Min.X;

            res.CellProfileSegmentVolatility = Measure.CurveSegmentVolatility(cell);
            res.NoiseFromCellProfileSegments = new Interval(0, res.CellProfileSegmentVolatility.Remap(new Interval(0, 10), new Interval(0, 1)));

            res.CellProfileCornerAngleVolatility = Measure.CurveCornerAngleVolatility(cell);
            res.NoiseFromCellProfileCorners = new Interval(0, res.CellProfileCornerAngleVolatility.Remap(new Interval(0, 10), new Interval(0, 1)));
        }

        private static void ParsePathInput(VendorManifest res, Curve path)
        {
            //Set path reference
            res.Path = path;

            //Measure path volatility
            var baseline = new LineCurve(path.PointAtStart, path.PointAtEnd);
            var driftPts = path.DivideByCount(10, false, out var pts);

            pts.ToList().RemoveAt(8);
 
            res.PathDriftVolatility = pts.Select(x =>
            {
                baseline.ClosestPoint(x, out var t);
                var dist = x.DistanceTo(baseline.PointAt(t));
                return dist;
            }).Average();

            res.NoiseFromPathDrift = new Interval(0, res.PathDriftVolatility.Remap(new Interval(0, 40), new Interval(0, 1)));
        }

        private static void GeneratePathSamplePoints(VendorManifest res)
        {
            var bayCount = Math.Round(res.Path.GetLength() / res.CellProfileWidth);
            var sampleDistances = new List<double>();

            var random = new Random(9);

            for (int i = 0; i < bayCount + 1; i++)
            {
                var stepVal = 
                    (res.CellProfileWidth * i)
                    +
                    ((random.Next(Convert.ToInt32(res.NoiseFromCellProfileSegments.Min * 100), Convert.ToInt32(res.NoiseFromCellProfileSegments.Max * 100)) / 100) * (res.CellProfileSegmentVolatility * 2));

                if (stepVal > res.Path.GetLength()) break;

                sampleDistances.Add(stepVal);
            }

            sampleDistances.Remap(new Interval(0, res.Path.GetLength()));

            res.PathSamplePoints = sampleDistances.Select(x => res.Path.PointAtLength(x)).ToList();

            res.PathSamplePointDistances = sampleDistances;
            res.PathSamplePointNormalizedDistances = sampleDistances.Remap(new Interval(0, 1));

            res.PathSamplePointFrames = new List<Plane>();
            sampleDistances.ForEach(x =>
            {
                res.Path.LengthParameter(x, out var t);
                res.Path.FrameAt(t, out var plane);
                res.PathSamplePointFrames.Add(plane);
            });
        }

        private static void GeneratePathFlanks(VendorManifest res)
        {
            var numFlanks = Convert.ToInt32(4 + ((Math.Round(res.NoiseFromPathDrift.Max / 0.3)) * 2));
            var random = new Random(9);

            //Generate placement vectors for flanks
            var outerBounds = res.PlanarBounds.DuplicateCurve();
            outerBounds.Transform(Transform.Scale(outerBounds.GetBoundingBox(Plane.WorldXY).Center, 1.25));
            var divider = res.Path.DuplicateCurve().Extend(CurveEnd.Both, 50, CurveExtensionStyle.Line);

            var ccx = Rhino.Geometry.Intersect.Intersection.CurveCurve(outerBounds, divider, 0.1, 0.1);

            outerBounds.ClosestPoint(divider.PointAtStart, out var tA);
            outerBounds.ClosestPoint(divider.PointAtEnd, out var tB);

            var splitPts = new List<double>() { tA, tB };

            var outerCrvs = outerBounds.Split(ccx.Where(x => x.IsPoint).Select(x => x.ParameterA)).OrderBy(x => x.PointAtNormalizedLength(0.5).Y);
            var dividerCrv = divider.Split(ccx.Where(x => x.IsPoint).Select(x => x.ParameterB)).OrderBy(x => x.GetLength()).Last();

            res.RightFlankRegion = Curve.JoinCurves(new List<Curve>()
            {
                outerCrvs.First(),
                dividerCrv.DuplicateCurve()
            })[0];

            res.LeftFlankRegion = Curve.JoinCurves(new List<Curve>()
            {
                outerCrvs.Last(),
                dividerCrv.DuplicateCurve()
            })[0];

            var frames = res.PathSamplePointFrames;

            frames.ForEach(x =>
            {
                var dirA = new Vector3d(x.YAxis);
                var dirB = new Vector3d(x.YAxis);
                dirB.Reverse();

                var testPt = x.Origin + dirA;

                if (res.LeftFlankRegion.Contains(testPt, Plane.WorldXY, 0.1) == PointContainment.Inside)
                {
                    res.LeftFlankVectors.Add(dirA);
                    res.RightFlankVectors.Add(dirB);
                }
                else
                {
                    res.LeftFlankVectors.Add(dirB);
                    res.RightFlankVectors.Add(dirA);
                }
            });

            //Generate left-hand flanks.
            var segmentNoise = res.NoiseFromCellProfileSegments;
            var angleNoise = res.NoiseFromCellProfileCorners;
            var pathNoise = res.NoiseFromPathDrift;

            res.LeftPathFlanks = new List<VendorPathFlank>();

            for (int i = 0; i < numFlanks / 2; i++)
            {
                var flank = new VendorPathFlank();
                var flankPts = new List<Point3d>();

                var steps = i > 1 
                    ? Convert.ToInt32(Math.Round(frames.Count * (1 - ((i - 1) * .4))))
                    : frames.Count;

                for (int j = 0; j < steps; j++) {
                    var dir = res.LeftFlankVectors[j];
                    dir.Unitize();

                    var randomVal = random.NextDouble() * segmentNoise.Max;
                    var noise = 4 * randomVal;

                    var offset = dir * (6.5 + noise) * (i + 1);

                    flankPts.Add(new Point3d(frames[j].Origin) + offset);
                }

                flank.FlankCurve = new Polyline(flankPts).ToNurbsCurve();
                flank.FlankPoints = flankPts;

                res.LeftPathFlanks.Add(flank);
            }

            //Generate right flanks
            res.RightPathFlanks = new List<VendorPathFlank>();

            for (int i = 0; i < numFlanks / 2; i++)
            {
                var flank = new VendorPathFlank();
                var flankPts = new List<Point3d>();

                var steps = i > 1
                    ? Convert.ToInt32(Math.Round(frames.Count * (1 - ((i - 1) * .4))))
                    : frames.Count;

                for (int j = 0; j < steps; j++)
                {
                    var dirs = new List<Vector3d>(res.RightFlankVectors);
                    dirs.Reverse();
                    var dir = dirs[j];
                    dir.Unitize();

                    var randomVal = random.NextDouble() * segmentNoise.Max;
                    var noise = 4 * randomVal;

                    var offset = dir * (6.5 + noise) * (i + 1);

                    flankPts.Add(new Point3d(frames[j].Origin) + offset);
                }

                flank.FlankCurve = new Polyline(flankPts).ToNurbsCurve();
                flank.FlankPoints = flankPts;

                res.RightPathFlanks.Add(flank);
            }

        }

        private static void GenerateMarketCells(VendorManifest res)
        {
            var rand = new Random(9);

            res.MarketCells.AddRange(ParseFlanksForCells(res.LeftPathFlanks, res.NoiseFromCellProfileSegments, rand));
            res.MarketCells.AddRange(ParseFlanksForCells(res.RightPathFlanks, res.NoiseFromCellProfileSegments, rand));

            List<VendorCell> ParseFlanksForCells(List<VendorPathFlank> flanks, Interval noise, Random r)
            {
                var cells = new List<VendorCell>();

                for (int i = flanks.Count - 1; i > 0; i--)
                {
                    var activeFlank = flanks[i];
                    var nextFlank = flanks[i - 1];

                    for (int j = 0; j < activeFlank.FlankPoints.Count - 1; j++)
                    {
                        var cell = new VendorCell();

                        //Draw cell profile
                        var ptA = nextFlank.FlankPoints[j + 1];
                        var ptB = activeFlank.FlankPoints[j + 1];
                        var ptC = activeFlank.FlankPoints[j];
                        var ptD = nextFlank.FlankPoints[j];

                        cell.CellProfile = new Polyline(new List<Point3d>()
                    {
                        ptA,
                        ptB,
                        ptC,
                        ptD,
                        new Point3d(ptA)
                    }).ToNurbsCurve();

                        //Determine cell orientation and edges
                        cell.BackEdge = new LineCurve(ptC, ptB);
                        cell.FrontEdge = new LineCurve(ptD, ptA);
                        cell.RightEdge = new LineCurve(ptC, ptD);
                        cell.LeftEdge = new LineCurve(ptB, ptA);

                        var ctr = cell.CellProfile.GetBoundingBox(Plane.WorldXY).Center;
                        res.Path.ClosestPoint(ctr, out var t);
                        var toPath = new Vector3d(res.Path.PointAt(t) - ctr);

                        var testRotA = new Vector3d(toPath);
                        testRotA.Rotate(Math.PI / 2, Vector3d.ZAxis);
                        var testRotB = new Vector3d(toPath);
                        testRotB.Rotate(Math.PI / -2, Vector3d.ZAxis);

                        var testPtA = new Point3d(ctr);
                        testPtA.Transform(Transform.Translation(testRotA));
                        var testPtB = new Point3d(ctr);
                        testPtB.Transform(Transform.Translation(testRotB));

                        var toNext = testPtA.DistanceTo(ptB) < testPtB.DistanceTo(ptB) ? testRotA : testRotB;

                        cell.CellPlane = new Plane(ctr, toNext, toPath);

                        //Create cell volume
                        var elevation = new Vector3d(0, 0, new Interval(9, 13).NoiseBasedValue(r, noise));

                        var floor = Brep.CreatePlanarBreps(cell.CellProfile, 0.1);
                        var extrusion = Extrusion.CreateExtrusion(cell.CellProfile, elevation).ToBrep();

                        var roofCrv = cell.CellProfile.DuplicateCurve();
                        roofCrv.Translate(elevation);

                        var roof = Brep.CreatePlanarBreps(roofCrv, 0.1);

                        var faces = new List<Brep>();
                        faces.AddRange(floor);
                        faces.Add(extrusion);
                        faces.AddRange(roof);

                        cell.CellVolume = Brep.JoinBreps(faces, 0.1)[0];

                        //Dispath cell to list
                        cells.Add(cell);
                    }
                }

                return cells;
            }
        }

        private static void GenerateRoofMass(VendorManifest res)
        {
            //Find closed regions of flanks closest to path.
            var leftPts = new List<Point3d>(res.LeftPathFlanks[0].FlankPoints);
            var leftCrv = new Polyline(res.LeftPathFlanks[0].FlankPoints).ToNurbsCurve();
            var rightPts = new List<Point3d>(res.RightPathFlanks[0].FlankPoints);
            var rightCrv = new Polyline(rightPts).ToNurbsCurve();

            Curve leftCorrection = null;
            Curve rightCorrection = null;

            res.PlanarBounds.ClosestPoint(leftCrv.PointAtEnd, out var tL);
            if (leftCrv.PointAtEnd.DistanceTo(res.PlanarBounds.PointAt(tL)) > 0) leftCorrection = new LineCurve(leftCrv.PointAtEnd, res.PlanarBounds.PointAt(tL));

            res.PlanarBounds.ClosestPoint(rightCrv.PointAtEnd, out var tR);
            if (rightCrv.PointAtEnd.DistanceTo(res.PlanarBounds.PointAt(tR)) > 0) rightCorrection = new LineCurve(rightCrv.PointAtEnd, res.PlanarBounds.PointAt(tR));

            var roofCrvs = new List<Curve>()
            {
                leftCrv,
                rightCrv,
                new LineCurve(leftCrv.PointAtStart, rightCrv.PointAtStart)
            };

            var leftEndPt = leftCorrection == null ? leftCrv.PointAtEnd : leftCorrection.PointAtEnd;
            var rightEndPt = rightCorrection == null ? rightCrv.PointAtEnd : rightCorrection.PointAtEnd;

            if (leftCorrection != null) roofCrvs.Add(leftCorrection);
            if (rightCorrection != null) roofCrvs.Add(rightCorrection);

            roofCrvs.Add(new LineCurve(leftEndPt, rightEndPt));

            var roofProfiles = Curve.JoinCurves(roofCrvs);

            var roofMasses = roofProfiles.Select(x =>
            {
                var scale = Transform.Scale(x.GetBoundingBox(Plane.WorldXY).Center, 1.1);
                x.Transform(scale);
                var groundFace = Brep.CreatePlanarBreps(x, 0.1)[0];
                var extrusion = Extrusion.CreateExtrusion(x, Vector3d.ZAxis * 7).ToBrep();
                var topFace = Brep.CreatePlanarBreps(x, 0.1)[0];
                topFace.Translate(new Vector3d(0, 0, 7));

                return Brep.JoinBreps(new List<Brep> { groundFace, extrusion, topFace }, 0.1)[0];             
            });

            var roofMass = Brep.CreateBooleanUnion(roofMasses, 0.1)[0];
            roofMass.Translate(new Vector3d(0, 0, 9));

            res.RoofMass = roofMass;

            //Generate short and long axis lines
            var longAxisPts = new List<Point3d>();

            for (int i = 0; i < rightPts.Count; i++)
            {               
                if (i != rightPts.Count - 1)
                {
                    var rightAnchor = (rightPts[i] + rightPts[i + 1]) / 2;
                    var leftAnchor = (leftPts[i] + leftPts[i + 1]) / 2;

                    res.RoofShortAxis.Add(new LineCurve(rightAnchor, leftAnchor));

                    longAxisPts.Add((rightAnchor + leftAnchor) / 2);

                    longAxisPts[i].Transform(Transform.Translation(new Vector3d(0, 0, 9)));
                }
            }

            var longAxis = new Polyline(longAxisPts).ToNurbsCurve().Extend(CurveEnd.Both, 10, CurveExtensionStyle.Line);
            var adjustedLongAxisPts = longAxis.DivideByCount(8, true, out var pts);

            res.RoofLongAxis = new Polyline(pts).ToNurbsCurve();

            res.RoofLongAxis.Translate(new Vector3d(0, 0, 9));
        }     

        private static void GenerateMarketSolid(VendorManifest res)
        {
            var masses = new List<Brep> { res.RoofMass };

            masses.AddRange(res.MarketCells.Select(x => x.CellVolume));

            res.AllMasses = masses;
            //res.MarketMass = Brep.CreateBooleanUnion(masses, 0.1)[0];
        }

        private static void SculptRoofArches(VendorManifest res)
        {
            res.RoofLongAxis.PerpendicularFrameAt(0, out var plane);

            Rhino.RhinoDoc.ActiveDoc.Objects.Add(Motifs.GothicProfile(plane, 5, 10, 5));
        }

        private static void SculptRoofWindows(VendorManifest res)
        {

        }
    }
}