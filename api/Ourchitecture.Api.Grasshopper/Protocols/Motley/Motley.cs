using System;
using System.Collections.Generic;
using Ourchitecture.Api.Protocols.Motley;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Protocols.Motley
{
    public class Motley : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Motley class.
        /// </summary>
        public Motley()
          : base("Motley", "Motley",
              "Format universal input for Motley protocol.",
              Properties.Resources.Category_Name, "Motley")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Boundary", "B", "Generally square boundary to operate within.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Path", "P", "Generally linear path to operate along.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Cell", "C", "Generally rectangular cell to aggregate with.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Motley Request", "[M]", "Universal input for each praxis in the motley protocol.", GH_ParamAccess.item);
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

            DA.SetData(0, new MotleyRequest(bounds, path, cell));
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
            get { return new Guid("d7f9e186-7ff7-4f9e-b8c0-62f820427d45"); }
        }
    }
}