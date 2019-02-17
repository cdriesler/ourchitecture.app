using System;
using System.Linq;
using System.Collections.Generic;
using Ourchitecture.Api.Protocols.Motley;
using Ourchitecture.Api.Protocols.Motley.Impact;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Protocols.Motley
{
    public class Impact : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Impact class.
        /// </summary>
        public Impact()
          : base("Impact", "Impact",
              "",
              Properties.Resources.Category_Name, "Motley")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Motley Request", "[M]", "Input packaged for motley protocol.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Ruin Regions", "R", "Regions to interact with.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Market Cell Quota", "Q", "Number of market cells to request.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Impact Manifest", "[M][I]", "Unformatted results from impact praxis.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            MotleyRequest req = null;
            if (!DA.GetData(0, ref req)) return;

            var regions = new List<Curve>();
            if (!DA.GetDataList(1, regions)) return;
            if (regions.Any(x => x == null)) return;

            int quota = 0;
            if (!DA.GetData(2, ref quota)) return;

            var request = new ImpactRequest(req, regions, quota);
            var result = ImpactSchema.Solve(request);

            //RhinoApp.WriteLine(result.PrecastArchWidth.ToString());

            DA.SetData(0, result);
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
            get { return new Guid("fb8ce6ea-5e7e-434a-bba1-d7abf6911dd2"); }
        }
    }
}