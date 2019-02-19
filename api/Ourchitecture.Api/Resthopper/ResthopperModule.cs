using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace Resthopper
{
    public class ResthopperModule : NancyModule
    {
        public ResthopperModule()
        {
            Post["/grasshopper"] = _ => Resthopper.Grasshopper(Context);
            Post["/io"] = _ => Resthopper.GetIoNames(Context);
        }
    }
}