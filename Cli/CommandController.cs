using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli
{
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
                ValidateProject(); // All Config File must be available or throw
                string sharedJson = File.ReadAllText($"{CurrentProjectDir}\\.netcms_config\\shared.json");
                JObject sharedSettings = JObject.Parse(sharedJson);

                string serverJson = File.ReadAllText($"{CurrentProjectDir}\\.netcms_config\\server.json");
                JObject serverSettings = JObject.Parse(serverJson);

                foreach (var modelClass in PublicModelController.Build(serverSettings.SelectToken("RootCode").ToString(), CurrentProjectDir))
                {
                    FileInfo file = new FileInfo($"{CurrentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.{modelClass._Name}.cs");
                    file.Directory.Create(); // If the directory already exists, this method does nothing.
                    File.WriteAllText($"{CurrentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.cs", modelClass.ToString());
                }
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
