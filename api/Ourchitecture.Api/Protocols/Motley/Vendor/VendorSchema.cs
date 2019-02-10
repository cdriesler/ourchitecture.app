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

            res.PathSamplePointFrames = new List<Plane>();
            sampleDistances.ForEach(x =>
            {
                res.Path.LengthParameter(x, out var t);
                res.Path.FrameAt(t, out var plane);
                res.PathSamplePointFrames.Add(plane);
            });
        }
    }
}