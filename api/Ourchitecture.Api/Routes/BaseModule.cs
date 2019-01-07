using Nancy;
using Rhino.Geometry;

namespace Ourchitecture.Api.Routes
{
    public class BaseModule : NancyModule
    {
        public BaseModule()
        {
            Get["/"] = _ => "Hello World!";
            Get["/health"] = _ => "Healthy!";
            Get["/test"] = _ =>
            {
                var testLineA = new LineCurve(new Point3d(0, -1, 0), new Point3d(0, 1, 0));
                var testLineB = new LineCurve(new Point3d(-1, 0, 0), new Point3d(1, 0, 0));

                var ccx = Rhino.Geometry.Intersect.Intersection.CurveCurve(testLineA, testLineB, 0.1, 0.1);

                return ccx.Count.ToString();
            };
        }
    }
}