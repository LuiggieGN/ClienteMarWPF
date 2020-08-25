using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MAR.AppLogic.MARHelpers
{
    public static class JSONHelper
    {
        public static T CreateNewFromJSON<T>(String pJSON)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(pJSON);
        }

        public static T CreateNewFromJSONNullValueIgnore<T>(String pJSON)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(pJSON, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeToJSON(object pObject)
        {
            return JsonConvert.SerializeObject(pObject, Formatting.Indented,
                      new JsonSerializerSettings
                      {
                          PreserveReferencesHandling = PreserveReferencesHandling.Objects
                      });
        }


    }
}
