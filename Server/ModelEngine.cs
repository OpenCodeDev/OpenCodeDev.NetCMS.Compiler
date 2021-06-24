using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenCodeDev.NetCMS.Core.Compiler
{
    public static class ModelEngine
    {
        public static string BuildModel(){
            string jsonstr = File.ReadAllText(@"C:\\Users\\Admin\\source\\repos\\OpenCodeDev.NetCMS\\Configurations\\Api\\Recipes\\Models\\recipes.model.json");
            JObject config = JObject.Parse(jsonstr);



            string privFields = "";
            string pubFields = "";
            ModelFieldEngine.BuildModelFields(config, out privFields, out pubFields);
            string idFields = ModelFieldEngine.BuildIdentifierField(); // User cannot change this field.
            if (config.SelectToken("Collection.Name") == null)
            {
                throw new Exception("Collection Name is missing, misconfiguration of file. Please avoid editing config files use the admin dashboard.");
            }

            string tColName = config.SelectToken("Collection.Name").ToString();

            Debug.WriteLine($"Creating Model for {tColName}");

            ClassBuilder PublicModel =
            new ClassBuilder($"{tColName}PublicModel", "OpenCodeDev.NetCms.Plugin.Api.{tColName}.Models", "public partial");
            PublicModel.UsingAdd(new List<string>() { "using System;" });
            PublicModel.PropertyAdd($"{idFields} {pubFields}");
            return "";
        }
    }
}
