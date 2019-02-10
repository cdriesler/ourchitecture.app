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
            ParseCellProfileInput(result, request.Cell);
            ParsePathInput(result, request.Path);

            //Generate initial massing.
            GeneratePathSamplePoints(result);
            GeneratePathFlanks(result);

            //Generate cell and entrance masses.

            return result;
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
            res.Path = path;

            var baseline = new LineCurve(path.PointAtStart, path.PointAtEnd);
            var driftPts = path.DivideByCount(10, false, out var pts);

            pts.ToList().RemoveAt(8);

            res.PathDriftVolatility = pts.Select(x =>
            {
                path.ClosestPoint(x, out var t);
                return x.DistanceTo(path.PointAt(t));
            }).Average();

            res.NoiseFromPathDrift = new Interval(0, res.PathDriftVolatility.Remap(new Interval(0, 100), new Interval(0, 1)));
        }

        private static void GeneratePathSamplePoints(VendorManifest res)
        {
            var bayCount = Math.Round(res.Path.GetLength() / res.CellProfileWidth);
            var sampleDistances = new List<double>();

            var random = new Random();

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
            var numFlanks = Convert.ToInt32(4 + ((Math.Round(res.NoiseFromPathDrift.Max / 0.4)) * 2));
            var random = new Random();

            //Generate left-hand flanks.

            var frames = res.PathSamplePointFrames;
            var segmentNoise = res.NoiseFromCellProfileSegments;
            var angleNoise = res.NoiseFromCellProfileCorners;

            res.LeftPathFlanks = new List<VendorPathFlank>();

            for (int i = 0; i < numFlanks / 2; i++)
            {
                var flank = new VendorPathFlank();
                var flankPts = new List<Point3d>();

                var steps = i > 1 
                    ? Convert.ToInt32(Math.Round(frames.Count * (1 - ((i - 1) * .33))))
                    : frames.Count;

                for (int j = 0; j < steps; j++) {
                    var dir = new Vector3d(frames[j].YAxis);
                    dir.Unitize();

                    var offset = dir * (8 + (4 * (random.Next(Convert.ToInt32(segmentNoise.Min * 100), Convert.ToInt32(segmentNoise.Max * 100)) / 100)));

                    flankPts.Add(new Point3d(frames[j].Origin) + offset);
                }

                flank.FlankCurve = new Polyline(flankPts).ToNurbsCurve();
                flank.FlankPoints = flankPts;

                res.LeftPathFlanks.Add(flank);
            }

        }
    }
}