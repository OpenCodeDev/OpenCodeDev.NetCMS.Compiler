using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api.Models
{
    public static class PublicModelController
    {
        public static List<ClassBuilder> Build(string rootDir, string sharedSettingsDir){

            // Load Shared Settings
            string buildSharedSettingFile = $"{sharedSettingsDir}\\.netcms_config\\shared.json";
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            List<ClassBuilder> ListOfModel = new List<ClassBuilder>();

            // List All File located to "Configuration";
            string[] files = Directory.GetFiles($"{rootDir}\\_Config_NetCMS\\", "*.model.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                // Load Full Model
                string json = File.ReadAllText(file);
                // Assign Base of Namespace.
                string baseNamespace = sharedSettings.Namespace;
                // Create the mandatory ID Property
                PropertyBuilder IdProp = Quickies.CreateIdProp();
                Console.WriteLine(json);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                // Assign Model Name
                string name = collection.Collection.Name;
                // Make sure namespace has basic validity
                if (baseNamespace == null || !baseNamespace.EndsWith(".Shared"))
                {
                    throw new Exception("Namespace base cannot be null or must endwith .Shared, most likely due to misconfiguration of the project.json or project file not available in output.");
                }
                // Build full class.
                ClassBuilder cBuild = new ClassBuilder($"{name}PublicModel", $"{baseNamespace}.Api.{name}.Models", "public partial");
                cBuild.Using(new UsingsExtractor(json).ToList());
                cBuild.Attribute(new AttributesExtractor(json).ToList());
                cBuild.Property(new PropertiesExtractor(json).ToList());
                cBuild.Property(IdProp);
                ListOfModel.Add(cBuild);
            }
            return ListOfModel;
        }
    }
}
