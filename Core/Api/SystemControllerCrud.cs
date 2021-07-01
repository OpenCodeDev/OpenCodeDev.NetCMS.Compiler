using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api
{
    public static partial class SystemController
    {
        public static List<ClassBuilder> BuildFetchOneRequest(string rootDir, string settingsDir)
        {
            // Load Shared Settings
            string buildSharedSettingFile = $"{settingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{settingsDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            string modelDir = $"{rootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");

            // List All File located to "Configuration";
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);

            List<ClassBuilder> _Models = new List<ClassBuilder>();
            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                List<PropertiesItemModel> props = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;

                // Build PredicateCondition Class
                ClassBuilder cBuild = new ClassBuilder($"{collection.Collection.Name}FetchOneRequest",
                $"{sharedSettings.Namespace}.Api.{collection.Collection.Name}.Messages", "public partial");
                cBuild.Attribute(new AttributeBuilder("ProtoContact"));
                // Dependencies
                cBuild.Using("OpenCodeDev.NetCMS.Core.Shared.Api.Messages", "ProtoBuf", "System", "System.ComponentModel.DataAnnotations");
                // Assign Fields as Enum Options
                cBuild.Property(Quickies.CreateGuidProp("Id", 1));


                _Models.Add(cBuild);
            }
            return _Models;
        }

        public static List<ClassBuilder> BuildFetchRequest(string rootDir, string settingsDir)
        {
            // Load Shared Settings
            string buildSharedSettingFile = $"{settingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{settingsDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            string modelDir = $"{rootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");

            // List All File located to "Configuration";
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);

            List<ClassBuilder> _Models = new List<ClassBuilder>();
            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                List<PropertiesItemModel> props = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;

                // Build PredicateCondition Class
                ClassBuilder cBuild = new ClassBuilder($"{collection.Collection.Name}FetchRequest",
                $"{sharedSettings.Namespace}.Api.{collection.Collection.Name}.Messages", "public partial");
                cBuild.Attribute(new AttributeBuilder("ProtoContact"));
                // Dependencies
                cBuild.Using($"{sharedSettings.Namespace}.Api.{collection.Collection.Name}.Messages", "ProtoBuf", "System", "System.ComponentModel.DataAnnotations", "System.Collections.Generic");
                // Assign Fields as Enum Options
                cBuild.Property(Quickies.CreateFetchConditionProp("Conditions", 1, collection.Collection.Name));

                // Limit Prop
                var _LimitProp = new PropertyBuilder("Limit", "System.Int32", true);
                _LimitProp.Attribute(new AttributeBuilder("ProtoMember", "2"));
                _LimitProp.Attribute(new AttributeBuilder("Required", ""));
                _LimitProp.Attribute(new AttributeBuilder("Range", @"10, 400, ErrorMessage = ""The field { 0 } must be greater than { 1}."""));
                cBuild.Property(_LimitProp);


                _Models.Add(cBuild);
            }
            return _Models;
        }

        public static List<ClassBuilder> BuildUpdateRequest(string rootDir, string sharedSettingsDir)
        {
            List<ClassBuilder> ListOfModel = BuildChangeRequestModel(rootDir, sharedSettingsDir, "UpdateOneRequest", "Update");
            foreach (var item in ListOfModel)
            {
                item.Property(Quickies.CreateGuidProp("Id", 1));
            }
            return ListOfModel;
        }

        public static List<ClassBuilder> BuildCreateRequest(string rootDir, string sharedSettingsDir)
        {
            return BuildChangeRequestModel(rootDir, sharedSettingsDir, "CreateRequest", "Create");
        }

        private static List<ClassBuilder>BuildChangeRequestModel (string rootDir, string sharedSettingsDir, string modelName, string argumentOf)
        {
            // Load Shared Settings
            string buildSharedSettingFile = $"{sharedSettingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));
            List<ClassBuilder> ListOfModel = new List<ClassBuilder>();
            string modelDir = $"{rootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");
            // List All File located to "Configuration";

            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            Console.WriteLine($"Parsing {files.Length} Public Models");
            foreach (var file in files)
            {
                // Load Full Model
                string json = File.ReadAllText(file);
                // Assign Base of Namespace.
                string baseNamespace = sharedSettings.Namespace;

                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                // Assign Model Name
                string name = collection.Collection.Name;
                var listAllowedProps = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;
                listAllowedProps = listAllowedProps.Where(p => p.ArgumentOf.Contains(argumentOf)).ToList();

                // Build full class.
                ClassBuilder cBuild = new ClassBuilder($"{name}{modelName}", $"{baseNamespace}.Api.{name}.Messages", "public partial");
                cBuild.Using(new UsingsExtractor(json).ToList());
                cBuild.Property(new PropertiesExtractor(listAllowedProps).ToList());
                ListOfModel.Add(cBuild);
            }
            return ListOfModel;
        }


        public static List<InterfaceBuilder> BuildControllersInterfaces(string rootDir, string sharedSettingsDir)
        {
            // Load Shared Settings
            string buildSharedSettingFile = $"{sharedSettingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));
            List<InterfaceBuilder> ListOfModel = new List<InterfaceBuilder>();
            string modelDir = $"{rootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");
            // List All File located to "Configuration";

            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            Console.WriteLine($"Parsing {files.Length} Public Models");
            foreach (var file in files)
            {
                // Load Full Model
                string json = File.ReadAllText(file);
                // Assign Base of Namespace.
                string baseNamespace = sharedSettings.Namespace;
                // Create the mandatory ID Property
                PropertyBuilder IdProp = Quickies.CreateGuidProp("Id", 1);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                // Assign Model Name
                string name = collection.Collection.Name;
                // Build Interface
                InterfaceBuilder iBuild = new InterfaceBuilder($"I{name}Controller", $"{baseNamespace}.Api.{name}.Controllers");
                iBuild.Method(Quickies.CreateDefaultGrpcsMethods(name, $"{baseNamespace}.Api.{name}.Models", $"{baseNamespace}.Api.{name}.Messages"));
                iBuild.Attribute(new AttributeBuilder("ServiceContract", ""));
                iBuild.Using("ProtoBuf.Grpc", "System.Collections.Generic", "System.ServiceModel", "System.Threading.Tasks");
                ListOfModel.Add(iBuild);
            }
            return ListOfModel;
        }


    }
}
