﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rhino.Geometry;

namespace Ourchitecture.Api.Protocols.Motley.Vendor
{
    public static class Motifs
    {
        public static Curve GothicProfile(Plane plane, double w, double h, double p)
        {
            var ptA = plane.PointAt(w / -2, 0);
            var ptB = plane.PointAt(w / 2, 0);

            var ptC = new Point3d(ptB);
            ptC.Transform(Transform.Translation(new Vector3d(0, 0, p)));
            var ptD = new Point3d(ptA);
            ptD.Transform(Transform.Translation(new Vector3d(0, 0, p)));

            var rightArc = new Arc(plane, ptD, w, Math.PI).ToNurbsCurve();
            var leftArc = new Arc(plane, ptC, w, Math.PI).ToNurbsCurve();

            var ccx = Rhino.Geometry.Intersect.Intersection.CurveCurve(rightArc, leftArc, 0.1, 0.1).Where(x => x.IsPoint).First();

            var rightTop = rightArc.Trim(new Interval(0, ccx.ParameterA));
            leftArc.LengthParameter(leftArc.GetLength(), out var t);
            var leftTop = leftArc.Trim(new Interval(ccx.ParameterB, t));

            var bottomHalfSegments = new List<Curve>()
            {
                new LineCurve(ptA, ptB).ToNurbsCurve(),
                new LineCurve(ptA, ptD).ToNurbsCurve(),
                new LineCurve(ptB, ptC).ToNurbsCurve(),
            };

            var topHalfSegments = new List<Curve>
            {
                rightTop,
                leftTop
            };

            var bottom = Curve.JoinCurves(bottomHalfSegments)[0];
            var top = Curve.JoinCurves(topHalfSegments)[0];

            var topBox = top.GetBoundingBox(Plane.WorldXY);
            var topHeight = topBox.Max.Z - topBox.Min.Z;

            top.Transform(Transform.Scale(new Plane(top.PointAtStart, Vector3d.XAxis, Vector3d.YAxis), 1, 1, (h - p) / topHeight));

            var allSegments = new List<Curve>();
            allSegments.AddRange(bottomHalfSegments);
            allSegments.Add(top);

            return Curve.JoinCurves(allSegments)[0];
        }
    }
}