using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Resthopper.Models
{
    public class ResthopperObject
    {
        [JsonProperty(PropertyName = "type")]
        //public Type Type { get; set; }
        public string Type { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        [JsonConstructor]
        public ResthopperObject()
        {

        }

        public ResthopperObject(object obj)
        {
            this.Data = JsonConvert.SerializeObject(obj);
            this.Type = obj.GetType().FullName;
        }
    }
}