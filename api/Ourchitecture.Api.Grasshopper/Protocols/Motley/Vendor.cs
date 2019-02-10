using System;
using System.Linq;
using System.Collections.Generic;
using Ourchitecture.Api.Protocols.Motley;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Protocols.Motley
{
    public class Vendor : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Vendor class.
        /// </summary>
        public Vendor() : base(
            "Vendor", 
            "Vendor",
            "Description",
            "Protocol", 
            "Motley")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Boundary", "B", "Square boundary of operation.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Path", "P", "Path to aggregate along.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Cell", "C", "Basic cell geometry.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Result Geometry", "G", "Resultant construct in rhino geometry.", GH_ParamAccess.list);
            pManager.AddTextParameter("Result SVG", "SVG", "Resultant construct as svg drawing.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve bounds = null;
            if (!DA.GetData(0, ref bounds)) return;

            Curve path = null;
            if (!DA.GetData(1, ref path)) return;

            Curve cell = null;
            if (!DA.GetData(2, ref cell)) return;

            var res = VendorSchema.Solve(new VendorRequest(bounds, path, cell));

            var geo = new List<Curve>();

            geo.AddRange(res.LeftPathFlanks.Select(x => x.FlankCurve).ToList());
            geo.AddRange(res.RightPathFlanks.Select(x => x.FlankCurve).ToList());

            //DA.SetData(0, res.GetAllGeometry());
            DA.SetDataList(0, res.MarketCells.Select(x => x.CellPlane).ToList() );
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("54d74b40-bf08-4a54-a281-35769738d2e2"); }
        }
    }
}