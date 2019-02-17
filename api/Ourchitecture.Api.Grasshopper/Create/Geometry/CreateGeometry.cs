using System;
using System.Collections.Generic;
using Ourchitecture.Api.Protocols.Motley.Impact;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Create.Geometry
{
    public enum GeometryRequestType { Test, All }

    public class CreateGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateGeometry class.
        /// </summary>
        public CreateGeometry()
          : base("Create Geometry", "Geometry",
              "Parse praxis result for a subset of useful geometry.",
              Properties.Resources.Category_Name, "Create")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Manifest", "[?][?]", "Manifest to parse for geometry.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "T", "Type of geometry to request. 0 = Test, 1 = All", GH_ParamAccess.item, 0);

            Param_Integer param = pManager[1] as Param_Integer;
            param.AddNamedValue("Test", 0);
            param.AddNamedValue("All", 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Output geometry.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ImpactManifest manifest = null;
            if (!DA.GetData(0, ref manifest)) return;

            int type = 0;
            if (!DA.GetData(1, ref type)) return;

            var geo = new List<GeometryBase>();
     
            if (manifest != null)
            {
                geo = ImpactGeometry.GetGeometry(manifest, (GeometryRequestType)type);
                DA.SetDataList(0, geo);
                return;
            }

            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No geometry parsing logic found for supplied manifest type.");
            return;
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
            get { return new Guid("e511fcbf-accb-4a5f-9a8e-4c0302f0c82d"); }
        }
    }
}