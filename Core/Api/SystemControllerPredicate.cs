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
        /// <summary>
        /// Build Predicate Model used for Custom Search
        /// </summary>
        /// <param name="rootDir"></param>
        /// <param name="settingsDir"></param>
        /// <returns></returns>
        public static List<ClassBuilder> BuildPredicateModel(string rootDir, string settingsDir)
        {
            // Load Shared Settings
            string buildSharedSettingFile = $"{settingsDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{settingsDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            string modelDir = $"{rootDir}\\_netcms_\\models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            Console.WriteLine($"Scanning Public Models In {modelDir}");

            // List All File located to "Configuration";
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);

            List<ClassBuilder> _PredicateModels = new List<ClassBuilder>();
            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                List<PropertiesItemModel> props = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;
                
                // Build PredicateCondition Class
                ClassBuilder cBuild = new ClassBuilder($"{collection.Collection.Name}PredicateCondition", 
                $"{sharedSettings.Namespace}.Api.{collection.Collection.Name}.Messages", "public partial")
                { _Inheritance = $"ConditionBase" };
                cBuild.Attribute(new AttributeBuilder("ProtoContract"));
                cBuild.Using("ProtoBuf"); // Add Parent's Namespace
                // Extract Allowed Fields
                List<PropertiesItemModel> allowedProps = props.Where(p=>p.ArgumentOf.Contains("Fetch")).ToList();
                allowedProps.Insert(0, new PropertiesItemModel() { Name = "Id", Type = "System.Guid", ArgumentOf = new List<string>() { "Fetch" } });
                // Assign Fields as Enum Options
                cBuild.Property(new PropertyBuilder("[ProtoMember(1)] public Fields Field { get; set; }"));
                cBuild.Property(new PropertyBuilder($"public enum Fields {{ {String.Join(", ", allowedProps.Select(p=>p.Name))} }}"));

                
                string getFieldTypeBody = $@"switch (Field) {{ {String.Join(" ", allowedProps.Select(p => $"case Fields.{p.Name}: return typeof({p.Type});"))} }} throw new RpcException(new Status(StatusCode.Unknown, ""Server misconfiguration tries to pass un supported type of data, contact support.""));";
                cBuild.Method(new MethodBuilder("GetFieldType", "Type", true, getFieldTypeBody, ""));


                _PredicateModels.Add(cBuild);
            }
            return _PredicateModels;
        }
    }
}
