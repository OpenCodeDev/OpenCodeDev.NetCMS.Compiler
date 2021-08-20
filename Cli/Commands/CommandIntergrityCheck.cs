using OpenCodeDev.NetCMS.Compiler.Cli.Models;
using OpenCodeDev.NetCMS.Compiler.Cli.Modules;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
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
    public class CommandIntergrityCheck : ChiefCommander
    {
        public string CurrentDirectory { get; set; }
        public CommandIntergrityCheck(string currentDirectory)
        { CurrentDirectory = currentDirectory; }

        public Dictionary<string, Dictionary<string, string>> AssembliesAndMore { get; set; } = new Dictionary<string, Dictionary<string, string>>();
        private bool Verbose { get; set; }
        CommandList ListOfApi { get; set; }
        ProjectModel ProjectBase { get; set; }
        LoadModels ModelLoader { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args)
        {
            PrintHeader();
            Verbose = HasArgKey(args, "-verbose");
            ListOfApi = new CommandList(CurrentDirectory) { Silent = true, Verbose = Verbose };
            ListOfApi.APIs(); // List All Assemblies
            ListOfApi.ReadAssemblyLinks(); // List All Assemblies APIs Model and Relation Files
            
            ValidateProjectFiles();
            ModelLoader = new LoadModels(ListOfApi.AssembliesModels); // Load Model JSON to Object.
            ValidateModelFiles();
            PrintFooter();
        }

        /// <summary>
        /// Check init.json
        /// </summary>
        public void ValidateProjectFiles()
        {
            try
            {
                string initLocation = $"{CurrentDirectory}\\init.json";
                if (!File.Exists(initLocation))
                {
                    if (Verbose) { throw new Exception($"Cannot locate {initLocation}"); }
                    throw new Exception($"Cannot located project file.");
                }
                string json = File.ReadAllText(initLocation);
                ProjectBase = JsonSerializer.Deserialize<ProjectModel>(json);
                if (!ProjectBase.ValidTopLevel() || !ProjectBase.General.Valid(Verbose)) { throw new Exception($"Init is invalid."); }
            }
            catch (Exception ex)
            {
                if (Verbose)
                {
                    Print(ex.ToString(), ConsoleColor.Red);
                }
                else
                {
                    Print(ex.Message, ConsoleColor.Red);
                }
            }

            Print("Project Configuration File ................................. PASSED!", ConsoleColor.Green);

        }
        public void ValidateModelFiles()
        {
            try
            {
                string ns = $"{ProjectBase.General.Name}.Server";
                if (!ListOfApi.AssembliesModels.ContainsKey(ns))
                { throw new Exception($"Couldn't find {ns} in Assembly List. You may have renamed the project without NetCMS-CLI?"); }
                foreach (var assembly in ModelLoader.Models)
                {
                    
                    foreach (var model in assembly.Value)
                    {
                        Print($"{assembly.Key}.{model.Collection.Name}  ................................. PASSED!", ConsoleColor.Green);

                    }
                    Print($"{assembly.Key} ................................. PASSED!", ConsoleColor.Cyan);
                }
                
            }
            catch (Exception ex)
            {
                if (Verbose)
                {
                    Print(ex.ToString(), ConsoleColor.Red);
                }
                else
                {
                    Print(ex.Message, ConsoleColor.Red);
                }
            }

        }
    }
}
