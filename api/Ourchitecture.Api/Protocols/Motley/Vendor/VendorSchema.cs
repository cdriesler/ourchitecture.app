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

            //Generate cell and entrance masses.


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
            var r = new Random(9);

            //TODO: Fix code duplication here.

            for (int i = res.LeftPathFlanks.Count - 1; i > 0; i--)
            {
                var activeFlank = res.LeftPathFlanks[i];
                var nextFlank = res.LeftPathFlanks[i - 1];

                for (int j = 0; j < activeFlank.FlankPoints.Count - 1; j++)
                {
                    var cell = new VendorCell();

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

                    var elevation = new Vector3d(0, 0, new Interval(9, 13).NoiseBasedValue(r, res.NoiseFromCellProfileSegments));

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

                    res.MarketCells.Add(cell);
                }
            }

            for (int i = res.RightPathFlanks.Count - 1; i > 0; i--)
            {
                var activeFlank = res.RightPathFlanks[i];
                var nextFlank = res.RightPathFlanks[i - 1];

                for (int j = 0; j < activeFlank.FlankPoints.Count - 1; j++)
                {
                    var cell = new VendorCell();

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

                    var elevation = new Vector3d(0, 0, new Interval(9, 13).NoiseBasedValue(r, res.NoiseFromCellProfileSegments));

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

                    res.MarketCells.Add(cell);
                }
            }
        }
    }
}