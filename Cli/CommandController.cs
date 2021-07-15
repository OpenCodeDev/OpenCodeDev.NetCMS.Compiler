using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Cli.Builder;
using OpenCodeDev.NetCMS.Compiler.Cli.Builder.Messages;
using OpenCodeDev.NetCMS.Compiler.Core.Api;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli
{
/// <summary>
/// 
/// </summary>
    public static class CommandController
    {
        public static string CurrentProjectDir { get; set; }
        public static void Run(string command, string[] args){
            // Set Current Working Diretory
            CurrentProjectDir = Environment.CurrentDirectory;
            if (command.ToLower().Equals("build"))
            {
                string parts = "All"; // What to build?
                if (args.Length >= 2)
                {
                    parts = args[1];
                }
                Console.WriteLine("Building NetCMS Resources...");
                ValidateProject(); // All Config File must be available or throw
                Directory.Delete($"{CurrentProjectDir}\\.netcms_config\\generated", true);
                Console.WriteLine("Project is Valid.");

                // Load All Models
                string serverJson = File.ReadAllText($"{CurrentProjectDir}\\.netcms_config\\server.json");
                JObject serverSettings = JObject.Parse(serverJson);
                var models = PublicModelController.Build(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir);

                SystemController.FingerprintInfo = Commenter.Info();

                ApiModels.BuildPublicModel(models, CurrentProjectDir); // Build Public Models
                // Build Private Models
                ApiModels.BuildPrivateModel(CurrentProjectDir);
                
                // Build System from Models
                SystemController.BuildControllerEndpointsTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "server");
                SystemController.BuildControllerInterfaceEndpointsTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "shared");
                SystemController.BuildCoreServiceTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "server");
                
                SystemController.BuildCoreServiceExtensionTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "server");
                SystemController.BuildPredicateConditions(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "shared");
                SystemController.BuildPredicateOrderBy(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "shared");
                SystemController.BuildControllerLogicTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "server");
                SystemController.BuildDatabaseTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir, "server");

                //// Predicate Model
                //ApiModels.CreateModelCSFiles(SystemController.BuildPredicateModel(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
                ApiModels.CreateModelCSFiles(SystemController.BuildFetchRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
                ApiModels.CreateModelCSFiles(SystemController.BuildFetchOneRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
                ApiModels.CreateModelCSFiles(SystemController.BuildUpdateRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
                ApiModels.CreateModelCSFiles(SystemController.BuildUpdateRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
                ApiModels.CreateModelCSFiles(SystemController.BuildCreateRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir), CurrentProjectDir, "shared");
               
                Console.WriteLine("Build Completed");
            }
        }


        public static void ValidateProject(){
            if (!File.Exists($"{CurrentProjectDir}\\.netcms_config\\server.json"))
            {
                throw new Exception($"{CurrentProjectDir}\\.netcms_config\\server.json was not found. You must execute the cli from the root project.");
            }

            if (!File.Exists($"{CurrentProjectDir}\\.netcms_config\\admin.json"))
            {
                throw new Exception($"{CurrentProjectDir}\\.netcms_config\\admin.json was not found. You must execute the cli from the root project.");
            }

            if (!File.Exists($"{CurrentProjectDir}\\.netcms_config\\shared.json"))
            {
                throw new Exception($"{CurrentProjectDir}\\.netcms_config\\shared.json was not found. You must execute the cli from the root project.");
            }
        }
    }
}
