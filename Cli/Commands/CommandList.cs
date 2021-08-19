using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    public class CommandList : ChiefCommander
    {
        private string CurrentDirectory { get; set; }
        private Dictionary<string, string[]> AssembliesModels = new Dictionary<string, string[]>();
        private bool Problem { get; set; }
        private bool Verbose { get; set; }
        public CommandList(string currentDir){
            CurrentDirectory = currentDir;
        }

        /// <summary>
        /// Run List Command
        /// </summary>
        public void Run(string[] args)
        {
            PrintHeader();
            Verbose = HasArgKey(args, "-verbose");
            APIs();
            Models();
            if (Problem && !Verbose)
            {
                Print("Problems were found during listing, run with -verbose for details.", ConsoleColor.Red);
            }
            PrintFooter();
        }
        /// <summary>
        /// Search for All server.json and *.model.json related to server info.
        /// </summary>
        public void APIs()
        {
            string[] files = Directory.GetFiles($"{CurrentDirectory}", "server.json", SearchOption.AllDirectories);
           
            foreach (var file in files)
            {
                string json = File.ReadAllText(file);
                try
                {
                    ProjectSettingsModel server = JsonSerializer.Deserialize<ProjectSettingsModel>(json);
                    if (!server.Valid()) {
                        Print(Verbose, $"{file} (Ignored)", ConsoleColor.DarkYellow);
                        Problem = true;
                    }
                    else
                    {
                        if (AssembliesModels.ContainsKey(server.Namespace))
                        {
                            Print(Verbose, $"{file} (Duplicate of {server.Namespace}, Please Fix It)", ConsoleColor.Red);
                            Problem = true;
                        }
                        else{
                            string[] models = Directory.GetFiles($"{server.RootCode}", "*.model.json", SearchOption.AllDirectories);
                            AssembliesModels.Add(server.Namespace, models);
                            Print(Verbose, $"{file} ({server.Namespace}, OK!)", ConsoleColor.DarkGreen);

                        }
                        
                        
                    }
                }
                catch (Exception ex)
                {
                    Print(Verbose, $"{file} (Error)", ConsoleColor.Red);
                    Print(Verbose, $"{ex.Message}", ConsoleColor.Red);
                    Problem = true;
                }

            }
  
        }
        
        /// <summary>
        /// Read All Json Model located in AssembliesModels Dictionary.
        /// </summary>
        public void Models() {
            foreach (var assembly in AssembliesModels)
            {
                Print(assembly.Key, ConsoleColor.Blue);
                if (assembly.Value.Length > 0)
                {
                    foreach (var modelFile in assembly.Value)
                    {
                        string json = File.ReadAllText(modelFile);
                        try
                        {
                            CollectionModel model = JsonSerializer.Deserialize<CollectionModel>(json);
                            Print($"\t -> {model.Collection.Name}", ConsoleColor.DarkGreen);
                        }
                        catch (Exception ex)
                        {
                            if (Verbose)
                            {
                                Print($"\t -> {modelFile}", ConsoleColor.Red);
                                Print($"{ex.Message}", ConsoleColor.Red);
                            }
                            else{
                                Print($"\t -> {modelFile}", ConsoleColor.Red);
                            }
                        }
                        
                    }
                }else{
                    Print($"\t -> No Api Models", ConsoleColor.DarkYellow);
                }
            }
        }
        public void Plugins(){

        }
    }
}
