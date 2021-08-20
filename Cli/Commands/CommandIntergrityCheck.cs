using OpenCodeDev.NetCMS.Compiler.Cli.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    /// <summary>
    /// Check the project file format, project namespaces, models and relationships for any broken links
    /// </summary>
    public class CommandIntergrityCheck: ChiefCommander
    {
        public string CurrentDirectory { get; set; }
        public CommandIntergrityCheck(string currentDirectory) 
        { CurrentDirectory = currentDirectory; }

        public Dictionary<string, Dictionary<string, string>> AssembliesAndMore { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        private bool Verbose { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args){
            PrintHeader();
            Verbose = HasArgKey(args,  "-verbose");
            CommandList ls = new CommandList(CurrentDirectory) {  Silent = true };
            ls.APIs(); // List All Assemblies
            ls.ReadAssemblyLinks(); // List All Assemblies APIs Model and Relation Files
            ValidateProjectFiles();
            PrintFooter();
        }

        /// <summary>
        /// Check init.json
        /// </summary>
        public void ValidateProjectFiles(){
            try
            {
                string initLocation = $"{CurrentDirectory}\\init.json";
                if (!File.Exists(initLocation))
                {
                    if (Verbose) { throw new Exception($"Cannot locate {initLocation}"); }
                    throw new Exception($"Cannot located project file.");
                }
                string json = File.ReadAllText(initLocation);
                ProjectModel pj = JsonSerializer.Deserialize<ProjectModel>(json);
                if (!pj.ValidTopLevel() || !pj.General.Valid(Verbose)) { throw new Exception($"Init is invalid."); }
            }
            catch (Exception ex)
            {
                if (Verbose)
                {
                    Print(ex.ToString(), ConsoleColor.Red);
                }else{
                    Print(ex.Message, ConsoleColor.Red);
                }
            }

            Print("Project Init has passed", ConsoleColor.Green);

        }
    }
}
