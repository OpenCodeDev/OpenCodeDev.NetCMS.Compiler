using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api.Models
{
    public static class PrivateModelController
    {
        public static List<ClassBuilder> Build(string rootDir, string sharedSettingsDir, ProgressBar progress = null)
        {

            // Load Shared Settings
            string buildSharedSettingFile = $"{sharedSettingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{sharedSettingsDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            
            string modelDir = $"{rootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");
            List<ClassBuilder> ListOfModel = new List<ClassBuilder>();

            // List All File located to "Configuration";
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            Console.WriteLine($"Parsing {files.Length} Public Models");
            Console.WriteLine($"Extracting Private Information from Public Models...");
            int prog = 0;
            foreach (var file in files)
            {
                prog++;
                // Load Full Model
                string json = File.ReadAllText(file);
                // Create the mandatory ID Property
                PropertyBuilder IdProp = Quickies.CreateIdProp();
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                // Assign Model Name
                string name = collection.Collection.Name;
                // Make sure namespace has basic validity
                if (serverSettings.Namespace == null || !serverSettings.Namespace.EndsWith(".Server"))
                {
                    throw new Exception("Namespace base cannot be null or must endwith .Server, most likely due to misconfiguration of the project.json or project file not available in output.");
                }
                // Extract Private Fields on 
                List<PropertyBuilder> props = new PropertiesExtractor(JsonSerializer.Deserialize<PropertiesModel>(json).Properties.Where(p=>p.ServerSideOnly).ToList()).ToList();
                // Build full class.
                ClassBuilder cBuild = new ClassBuilder($"{name}Model", $"{serverSettings.Namespace}.Api.{name}.Models", "public partial")
                { _Inheritance = $"{name}PublicModel" };
                cBuild.Using(new UsingsExtractor(json).ToList());
                cBuild.Using($"{sharedSettings.Namespace}.Api.{name}.Models"); // Add Parent's Namespace
                cBuild.Property(props);
                ListOfModel.Add(cBuild);
                progress?.Report(prog / (files.Length * 2));
            }
            return ListOfModel;
        }
    }
}
