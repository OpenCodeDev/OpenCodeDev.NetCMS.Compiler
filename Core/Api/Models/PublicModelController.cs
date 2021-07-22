using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api.Models
{
    public static class PublicModelController
    {
        public static List<ClassBuilder> Build(string rootDir, string sharedSettingsDir, ProgressBar progress = null){

            
            // Load Shared Settings
            string buildSharedSettingFile = $"{sharedSettingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));
            List<ClassBuilder> ListOfModel = new List<ClassBuilder>();
            string modelDir = $"{rootDir}\\_netcms_\\models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");
            // List All File located to "Configuration";

            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            Console.WriteLine($"Parsing {files.Length} Public Models");
            int prog = 0;
            foreach (var file in files)
            {
                prog++;
                // Load Full Model
                string json = File.ReadAllText(file);
                // Assign Base of Namespace.
                string baseNamespace = sharedSettings.Namespace;
                // Create the mandatory ID Property
                PropertyBuilder IdProp = Quickies.CreateIdEFProp();
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                var props = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;
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
                // Props that are Index and Unique like license plate, social security or 
                cBuild.Attribute(props.Where(p=> !p.Private && !p.ServerSideOnly && p.Unique).Select(p=> new AttributeBuilder("Index", $"nameof({p.Name}), IsUnique=true")).ToList());
                cBuild.Property(new PropertiesExtractor(props.Where(p => !p.ServerSideOnly).ToList()).ToList());
                cBuild.Property(IdProp);
                ListOfModel.Add(cBuild);
                progress?.Report(prog/ (files.Length * 2));
            }
            return ListOfModel;
        }
    }
}
