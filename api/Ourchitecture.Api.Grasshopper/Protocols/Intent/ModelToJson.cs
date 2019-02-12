using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Protocols.Intent
{
    public class ModelToJson : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModelToJson class.
        /// </summary>
        public ModelToJson()
          : base("ModelToJson", 
                "Json",
              "Given a 3dm object, output its correlated text-based json.",
              "Protocol", "Intent")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Geometry to convert.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Json", "J", "Output json.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GeometryBase geo = null;
            if (!DA.GetData(0, ref geo)) return;

            var data = JsonConvert.SerializeObject(geo);

            System.IO.File.WriteAllText(@"C:\Users\cdrie\Google Drive\academic\prattsoa\2019SP\ARCH 503\_protocols\motley\_intent\memory\model_json.txt", data);

            DA.SetData(0, data);
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
            get { return new Guid("121fb4fe-9408-4591-bce3-80935c05e938"); }
        }
    }
}