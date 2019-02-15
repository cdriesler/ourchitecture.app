using System;
using System.Collections.Generic;
using Ourchitecture.Api.Protocols;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ourchitecture.Api.Grasshopper.Protocols.Motley
{
    public class Swerve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Swerve class.
        /// </summary>
        public Swerve(): base(
            "Swerve", 
            "Swerve",
            "Description",
            "Protocol", 
            "Motley"
            )
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
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
            get { return new Guid("83460d82-1092-4643-b025-5be4bf872ea6"); }
        }
    }
}