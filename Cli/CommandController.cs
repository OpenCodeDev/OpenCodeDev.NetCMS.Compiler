using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Cli.Builder;
using OpenCodeDev.NetCMS.Compiler.Cli.Builder.Messages;
using OpenCodeDev.NetCMS.Compiler.Cli.Commands;
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
        public static string CurrentPluginDir { get; set; }

        public static List<string> Plugins { get; set; }
        public static void Run(string command, string[] args){
            // Set Current Working Diretory
            CurrentProjectDir = Environment.CurrentDirectory;
            string cmd = command.ToLower();
            if (cmd.Equals("build"))
            {
                //Build(args);
            }
            else if (cmd.Equals("new-project")) {
                var np = new CommandInit(CurrentProjectDir);
                np.Run(args);
            }
            else if (cmd.Equals("list"))
            {
                var ls = new CommandList(CurrentProjectDir);
                ls.Run(args);
            }
            else if (cmd.Equals("prebuild"))
            {
                var pb = new CommandPreBuild();
                pb.Run(args);
            }
            else if (cmd.Equals("validate"))
            {
                var val = new CommandIntergrityCheck(CurrentProjectDir);
                val.Run(args);
            }
            else
            {
                Console.WriteLine("Command doesn't exist.");
            }
        }
        public static void List(string[] args){

        }

        public static void ListPlugins(){

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
