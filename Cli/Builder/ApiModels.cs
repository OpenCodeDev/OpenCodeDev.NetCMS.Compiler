using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Builder
{
    public static class ApiModels
    {

        public static void CreateModelCSFiles(List<ClassBuilder> onClassBuilderRequest, string currentProjectDir, string side)
        {
            string serverJson = File.ReadAllText($"{currentProjectDir}\\.netcms_config\\server.json");
            JObject serverSettings = JObject.Parse(serverJson);

            foreach (var modelClass in onClassBuilderRequest)
            {
                FileInfo file = new FileInfo($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());

            }
        }

        public static void CreateServiceCSFiles(List<ClassBuilder> onBuilderRequest, string currentProjectDir, string side)
        {
            string serverJson = File.ReadAllText($"{currentProjectDir}\\.netcms_config\\server.json");
            JObject serverSettings = JObject.Parse(serverJson);

            foreach (var modelClass in onBuilderRequest)
            {
                FileInfo file = new FileInfo($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());

            }
        }

        public static void CreateCSFiles(List<ClassBuilder> onBuilderRequest, string currentProjectDir, string side) {
            foreach (var modelClass in onBuilderRequest)
            {
                FileInfo file = new FileInfo($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());

            }
        }
        
        public static void CreateControllerCSFiles(List<InterfaceBuilder> onBuilderRequest, string currentProjectDir, string side)
        {
            string serverJson = File.ReadAllText($"{currentProjectDir}\\.netcms_config\\server.json");
            JObject serverSettings = JObject.Parse(serverJson);

            foreach (var model in onBuilderRequest)
            {
                FileInfo file = new FileInfo($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{model._Namespace}.{model._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{currentProjectDir}\\.netcms_config\\generated\\{side}\\{model._Namespace}.{model._Name}.cs", model.ToString());

            }
        }

        public static void BuildPublicModel(List<ClassBuilder> models, string CurrentProjectDir)
        {
                foreach (var modelClass in models)
                {
                    FileInfo file = new FileInfo($"{CurrentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.{modelClass._Name}.cs");
                    file.Directory.Create(); // If the directory already exists, this method does nothing.
                    File.WriteAllText($"{CurrentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());
                }
    

        }

        public static void BuildPrivateModel(string CurrentProjectDir)
        {
 
            string serverJson = File.ReadAllText($"{CurrentProjectDir}\\.netcms_config\\server.json");
            JObject serverSettings = JObject.Parse(serverJson);
            var models = PrivateModelController.Build(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir);
            foreach (var modelClass in models)
            {
                    FileInfo file = new FileInfo($"{CurrentProjectDir}\\.netcms_config\\generated\\server\\{modelClass._Namespace}.{modelClass._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{CurrentProjectDir}\\.netcms_config\\generated\\server\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());
                    
                }
            

         
        }
    }
}
