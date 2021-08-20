using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Cli.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Api;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    public class CommandBuild : ChiefCommander
    {
        private string CurrentDirectory { get; set; }
        private CommandList ListAssets { get; set; }

        public CommandBuild(string currentDir)
        {
            CurrentDirectory = currentDir;
            ListAssets = new CommandList(currentDir);
            ListAssets.APIs(); // Find All API's server and its related models
        }

        
        public void Run(string[] args){            
            
        }


        //public static void Build(string[] args)
        //{
        //    string parts = "All"; // What to build?
        //    if (args.Length >= 2) { parts = args[1]; }
        //    Console.WriteLine("Building NetCMS Resources...");
        //    ValidateProject(); // All Config File must be available or throw
        //    if (Directory.Exists($"{CurrentDirectory}\\.netcms_config\\generated"))
        //    {
        //        Directory.Delete($"{CurrentDirectory}\\.netcms_config\\generated", true);
        //    }

        //    Console.WriteLine("Project is Valid.");

        //    string modelDir = $"{CurrentDirectory}\\server".Replace("\\\\", "\\"); // Remove Double Slashes
        //    string[] files = Directory.GetFiles($"{modelDir}", "*.model.json", SearchOption.AllDirectories);
        //    if (files.Length <= 0)
        //    {
        //        Console.WriteLine($"Cannot find any model in {modelDir}");
        //        return; // Cancel
        //    }

        //    // Load All Models
        //    string serverJson = File.ReadAllText($"{CurrentDirectory}\\.netcms_config\\server.json");
        //    JObject serverSettings = JObject.Parse(serverJson);
        //    var models = PublicModelController.Build(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory);

        //    SystemController.FingerprintInfo = Commenter.Info();

        //    ApiModels.BuildPublicModel(models, CurrentDirectory); // Build Public Models
        //                                                           // Build Private Models
        //    ApiModels.BuildPrivateModel(CurrentDirectory);

        //    // Build System from Models
        //    SystemController.BuildControllerEndpointsTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "server");
        //    SystemController.BuildControllerInterfaceEndpointsTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "shared");
        //    SystemController.BuildCoreServiceTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "server");

        //    SystemController.BuildCoreServiceExtensionTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "server");
        //    SystemController.BuildPredicateConditionExtTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "shared");
        //    SystemController.BuildPredicateConditions(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "shared");
        //    SystemController.BuildPredicateOrderBy(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "shared");
        //    SystemController.BuildControllerLogicTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "server");
        //    SystemController.BuildDatabaseTemplateClass(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "server");
        //    SystemController.BuildUpdateManyResponse(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory, "shared");

        //    //// Predicate Model
        //    //ApiModels.CreateModelCSFiles(SystemController.BuildPredicateModel(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory), CurrentDirectory, "shared");
        //    ApiModels.CreateModelCSFiles(SystemController.BuildFetchRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory), CurrentDirectory, "shared");
        //    ApiModels.CreateModelCSFiles(SystemController.BuildFetchOneRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory), CurrentDirectory, "shared");
        //    ApiModels.CreateModelCSFiles(SystemController.BuildUpdateRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory), CurrentDirectory, "shared");
        //    ApiModels.CreateModelCSFiles(SystemController.BuildCreateRequest(serverSettings.SelectToken("RootCode").ToString(), CurrentDirectory), CurrentDirectory, "shared");

        //    Console.WriteLine("Build Completed");
        //}

    }
}
