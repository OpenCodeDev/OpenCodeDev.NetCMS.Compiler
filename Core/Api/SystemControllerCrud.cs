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

        public static CommentBuilder FingerprintInfo { get; set; }
        /// <summary>
        /// Load a given Template
        /// </summary>
        /// <param name="solutionDir">Solution directory</param>
        /// <param name="name">Template's name.</param>
        /// <returns></returns>
        public static string LoadTemplate(string solutionDir, string name)
        {
            return File.ReadAllText($"{solutionDir}\\.netcms_config\\templates\\{name}.cs");
        }

        /// <summary>
        /// Fill basic template information.
        /// </summary>
        /// <param name="solutionDir">Solution directory</param>
        /// <param name="modelFile">Project directory</param>
        /// <param name="templateCode">Template String Code</param>
        /// <returns></returns>
        public static string FillTemplate(string solutionDir, string modelFile, string templateCode)
        {
            string json = File.ReadAllText(modelFile); // Load Model JSON

            CollectionItemModel collection = JsonSerializer.Deserialize<CollectionModel>(json).Collection;

            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            return FillTemplate(templateCode, new Dictionary<string, string>() { 
            { "_API_NAME_", collection.Name }, 
            { "//_NETCMS_HEADER_", FingerprintInfo.ToString() },
            { "_NAMESPACE_BASE_SERVER_", serverSettings.Namespace },
            { "_NAMESPACE_BASE_SHARED_", sharedSettings.Namespace}
            });
        }

        public static string FillTemplate(string template, Dictionary<string, string> replacer)
        {

            return template
                    .Replace("_API_NAME_", replacer.ContainsKey("_API_NAME_") ? replacer["_API_NAME_"] : "")
                    .Replace("//_NETCMS_HEADER_", FingerprintInfo.ToString())
                    .Replace("_NAMESPACE_BASE_SERVER_", replacer.ContainsKey("_NAMESPACE_BASE_SERVER_") ? replacer["_NAMESPACE_BASE_SERVER_"] : "")
                    .Replace("_NAMESPACE_BASE_SHARED_", replacer.ContainsKey("_NAMESPACE_BASE_SHARED_") ? replacer["_NAMESPACE_BASE_SHARED_"] : "");
        }

        /// <summary>
        /// Build Given Template, Will Call FillTemplate()
        /// </summary>
        /// <param name="solutionDir">Solution directory</param>
        /// <param name="projectRootDir">Project directory</param>
        /// <param name="templateName">Template filename without .cs</param>
        /// <param name="classNameCode">Class Name code, eg: _API_NAME_Model</param>
        /// <param name="nscode">Namespace code: _NAMESPACE_BASE_SERVER_.Api._API_NAME_.Models</param>
        /// <returns></returns>
        public static List<ClassBuilder> BuildTemplate(string solutionDir, string projectRootDir, string templateName, string classNameCode, string nscode)
        {
            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            string modelDir = $"{projectRootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            List<ClassBuilder> cBuilds = new List<ClassBuilder>();
            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                List<PropertiesItemModel> props = JsonSerializer.Deserialize<PropertiesModel>(json).Properties;

                string template = FillTemplate(solutionDir, file, LoadTemplate(solutionDir, templateName));
                cBuilds.Add(new ClassBuilder(template)
                {
                    _Name = FillTemplate(solutionDir, file, classNameCode),
                    _Namespace = FillTemplate(solutionDir, file, nscode)
                });
            }
            return cBuilds;
        }

        /// <summary>
        /// Build Common Class, filling only basic template info.
        /// </summary>
        /// <param name="projectRootDir"></param>
        /// <param name="solutionDir"></param>
        /// <param name="side"></param>
        public static void BuildControllerEndpointsTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            List<ClassBuilder> builds =
            BuildTemplate(solutionDir, projectRootDir, "ApiControllerEndpoints",
            "_API_NAME_ControllerEndpoints", "_NAMESPACE_BASE_SERVER_.Api._API_NAME_.Controllers");

            foreach (var item in builds)
            {
                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs", item.ToString());
            }

        }

        public static void BuildControllerLogicTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            List<ClassBuilder> builds =
            BuildTemplate(solutionDir, projectRootDir, "ApiControllerLogic",
            "_API_NAME_Controller", "_NAMESPACE_BASE_SERVER_.Api._API_NAME_.Controllers");

            foreach (var item in builds)
            {
                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs", item.ToString());
            }

        }

        public static void BuildCoreServiceTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            List<ClassBuilder> builds =
            BuildTemplate(solutionDir, projectRootDir, "ApiCoreServices",
            "_API_NAME_CoreServices", "_NAMESPACE_BASE_SERVER_.Api._API_NAME_.Services");

            foreach (var item in builds)
            {

                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs", item.ToString());
            }

        }
        public static void BuildCoreServiceExtensionTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));



            Dictionary<string, PropertiesModel> _ApiList = GetProps(solutionDir, projectRootDir);
            foreach (var item in _ApiList)
            {
                string template = FillTemplate(LoadTemplate(solutionDir, "ApiCoreServicesExt"),
                new Dictionary<string, string>() {
                { "_NAMESPACE_BASE_SERVER_", serverSettings.Namespace },
                { "_API_NAME_", item.Key },
                { "_NAMESPACE_BASE_SHARED_", sharedSettings.Namespace}
                });

                // Extract Valid Props
                var props = item.Value.Properties.Where(p => !p.ServerSideOnly && !p.Private && p.ArgumentOf.Contains("Fetch")).ToList();
                
                string casePropsOrderBy = $@"{String.Join(Environment.NewLine, props.Select(p => $"case {item.Key}PredicateOrdering.Fields.{p.Name}: {Environment.NewLine} return orderType == OrderType.Ascending ? query.OrderBy(p => p.{p.Name}) : query.OrderByDescending(p => p.{p.Name});"))}";
                string casePropsThenBy = $@"{String.Join(Environment.NewLine, props.Select(p => $"case {item.Key}PredicateOrdering.Fields.{p.Name}: {Environment.NewLine} return orderType == OrderType.Ascending ? query.ThenBy(p => p.{p.Name}) : query.ThenByDescending(p => p.{p.Name});"))}";
                string casePropsWhere = $@"{String.Join(Environment.NewLine, props.Select(p => $"case {item.Key}PredicateConditions.Fields.{p.Name}: {Environment.NewLine} nonRelationField = p => myServiceBase.ConditionTypeDelegator(item.Conditions, p.{p.Name}, item.Value, typeof({p.Type})); break;"))}";
                // Filling Template.
                template = template
                .Replace("//_MODEL_ORDERABLE_FIELD_ORDERBY_", casePropsOrderBy)
                .Replace("//_MODEL_ORDERABLE_FIELD_THENBY_", casePropsThenBy)
                .Replace("//_WHERE_CONDITION_PUBLIC_FETCH_FIELDS_", casePropsWhere);

                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{serverSettings.Namespace}.Api.{item.Key}.Services.{item.Key}CoreServicesExt.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{serverSettings.Namespace}.Api.{item.Key}.Services.{item.Key}CoreServicesExt.cs", template);
            }

        }

        public static void BuildControllerInterfaceEndpointsTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            List<ClassBuilder> builds =
            BuildTemplate(solutionDir, projectRootDir, "ApiControllerInterfaceEndpoints",
            "I_API_NAME_Controller", "_NAMESPACE_BASE_SHARED_.Api._API_NAME_.Controllers");

            foreach (var item in builds)
            {

                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs", item.ToString());
            }

        }



        /// <summary>
        /// Get Props of all Models
        /// </summary>
        public static Dictionary<string, PropertiesModel> GetProps(string solutionDir, string projectRootDir)
        {
            // Load Shared Settings
            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));
            string modelDir = $"{projectRootDir}\\_Models\\".Replace("\\\\", "\\"); // Remove Double Slashes
            string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
            Dictionary<string, PropertiesModel> _ApiList = new Dictionary<string, PropertiesModel>();
            foreach (var fileItem in files)
            {
                // Load Full Model
                string json = File.ReadAllText(fileItem);
                // Assign Base of Namespace.
                string baseNamespace = serverSettings.Namespace;
                // Create the mandatory ID Property
                PropertyBuilder IdProp = Quickies.CreateGuidProp("Id", 1);
                CollectionModel collection = JsonSerializer.Deserialize<CollectionModel>(json);
                var _Props = JsonSerializer.Deserialize<PropertiesModel>(json);
                // Assign Model Name
                _ApiList.Add(collection.Collection.Name, _Props);
            }
            return _ApiList;
        }

        /// <summary>
        /// Build Code-First Database
        /// </summary>
        public static void BuildDatabaseTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));

            string template = FillTemplate(LoadTemplate(solutionDir, "DatabaseBase"), new Dictionary<string, string>() {
            { "_NAMESPACE_BASE_SERVER_", serverSettings.Namespace },
            { "_NAMESPACE_BASE_SHARED_", sharedSettings.Namespace}
            });

            Dictionary<string, PropertiesModel> _ApiList = GetProps(solutionDir, projectRootDir);

            // Filling Template.
            template = template
            .Replace("//_USINGS_", String.Join($"{Environment.NewLine}", _ApiList.Select(p => $"using {serverSettings.Namespace}.Api.{p.Key}.Models;")))
            .Replace("//_DBSET_", String.Join($"{Environment.NewLine}", _ApiList.Select(p => $"public virtual DbSet<{p.Key}Model> {p.Key} {{ get; set; }}")));

            FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{serverSettings.Namespace}.Database.DatabaseBase.cs");
            file.Directory.Create();
            File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{serverSettings.Namespace}.Database.DatabaseBase.cs", template);

        }


        public static void BuildPredicateConditionExtTemplateClass(string projectRootDir, string solutionDir, string side)
        {
            List<ClassBuilder> builds =
            BuildTemplate(solutionDir, projectRootDir, "PredicateConditionsExt",
            "_API_NAME_PredicateConditionsExt", $"_NAMESPACE_BASE_{(side == "shared" ? "SHARED" : "SERVER")}_.Api._API_NAME_.Messages");

            foreach (var item in builds)
            {
                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{item._Namespace}.{item._Name}.cs", item.ToString());
            }

        }

        /// <summary>
        /// Build Fetch Condition Message Data Contract.
        /// </summary>
        public static void BuildPredicateConditions(string projectRootDir, string solutionDir, string side)
        {
            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));



            Dictionary<string, PropertiesModel> _ApiList = GetProps(solutionDir, projectRootDir);
            foreach (var item in _ApiList)
            {
                string template = FillTemplate(LoadTemplate(solutionDir, "PredicateConditions"), 
                new Dictionary<string, string>() {
                { "_NAMESPACE_BASE_SERVER_", serverSettings.Namespace },
                { "_API_NAME_", item.Key },
                { "_NAMESPACE_BASE_SHARED_", sharedSettings.Namespace}
                });
                var props = item.Value.Properties.Where(p => !p.ServerSideOnly && !p.Private && p.ArgumentOf.Contains("Fetch")).ToList();
                props.Insert(0, new PropertiesItemModel() { Name = "Id", Type = "System.Guid", ArgumentOf = new List<string>() { "Fetch" } });
                string getFieldTypeBody = $@"{String.Join(Environment.NewLine, props.Select(p => $"case Fields.{p.Name}: {Environment.NewLine} return typeof({p.Type});"))}";
                // Filling Template.
                template = template
                .Replace("//_FIELDS_ENUM_", String.Join($", {Environment.NewLine}", props.Select(p=>p.Name)))
                .Replace("//_GET_TYPE_SWITCH_", getFieldTypeBody);

                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{sharedSettings.Namespace}.Api.{item.Key}.Messages.{item.Key}PredicateConditions.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{sharedSettings.Namespace}.Api.{item.Key}.Messages.{item.Key}PredicateConditions.cs", template);
            }

        }

        /// <summary>
        /// Build a Fetch OrderBy Definition.
        /// </summary>
        /// <param name="projectRootDir"></param>
        /// <param name="solutionDir"></param>
        /// <param name="side"></param>
        public static void BuildPredicateOrderBy(string projectRootDir, string solutionDir, string side)
        {
            string buildSharedSettingFile = $"{solutionDir}\\.netcms_config\\shared.json";
            Console.WriteLine($"Loading Shared Settings In {buildSharedSettingFile}");
            SettingModel sharedSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildSharedSettingFile));

            string buildServerSettingFile = $"{solutionDir}\\.netcms_config\\server.json";
            Console.WriteLine($"Loading Server Settings In {buildServerSettingFile}");
            SettingModel serverSettings = JsonSerializer.Deserialize<SettingModel>(File.ReadAllText(buildServerSettingFile));



            Dictionary<string, PropertiesModel> _ApiList = GetProps(solutionDir, projectRootDir);
            foreach (var item in _ApiList)
            {
                string template = FillTemplate(LoadTemplate(solutionDir, "ApiPredicateOrdering"),
                new Dictionary<string, string>() {
                { "_NAMESPACE_BASE_SERVER_", serverSettings.Namespace },
                { "_API_NAME_", item.Key },
                { "_NAMESPACE_BASE_SHARED_", sharedSettings.Namespace}
                });
                var props = item.Value.Properties.Where(p => !p.ServerSideOnly && !p.Private && p.ArgumentOf.Contains("Fetch")).ToList();
                props.Insert(0, new PropertiesItemModel() { Name = "Id", Type = "System.Guid", ArgumentOf = new List<string>() { "Fetch" } });
                string getFieldTypeBody = $@"{String.Join(Environment.NewLine, props.Select(p => $"case Fields.{p.Name}: {Environment.NewLine} return typeof({p.Type});"))}";
                // Filling Template.
                template = template
                .Replace("//_FIELDS_ENUM_", String.Join($", {Environment.NewLine}", props.Select(p => p.Name)))
                .Replace("//_GET_TYPE_SWITCH_", getFieldTypeBody);

                FileInfo file = new FileInfo($"{solutionDir}\\.netcms_config\\generated\\{side}\\{sharedSettings.Namespace}.Api.{item.Key}.Messages.{item.Key}PredicateOrdering.cs");
                file.Directory.Create();
                File.WriteAllText($"{solutionDir}\\.netcms_config\\generated\\{side}\\{sharedSettings.Namespace}.Api.{item.Key}.Messages.{item.Key}PredicateOrdering.cs", template);
            }

        }
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
                cBuild.Attribute(new AttributeBuilder("ProtoContract"));
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
                cBuild.Attribute(new AttributeBuilder("ProtoContract"));
                // Dependencies
                cBuild.Using($"{sharedSettings.Namespace}.Api.{collection.Collection.Name}.Messages", "ProtoBuf", "System", "System.ComponentModel.DataAnnotations", "System.Collections.Generic");
                // Assign Fields as Enum Options
                cBuild.Property(Quickies.CreateFetchConditionProp("Conditions", 1, collection.Collection.Name));
                cBuild.Property(Quickies.CreateFetchOrderByProp("OrderBy", 2, collection.Collection.Name));

                // Limit Prop
                var _LimitProp = new PropertyBuilder("Limit", "System.Int32", true);
                _LimitProp.Attribute(new AttributeBuilder("ProtoMember", "3"));
                _LimitProp.Attribute(new AttributeBuilder("Required", ""));
                _LimitProp.Attribute(new AttributeBuilder("Range", @"10, 400, ErrorMessage = ""The field {0} must be greater than {1}."""));
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

        private static List<ClassBuilder> BuildChangeRequestModel(string rootDir, string sharedSettingsDir, string modelName, string argumentOf)
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


    }
}
