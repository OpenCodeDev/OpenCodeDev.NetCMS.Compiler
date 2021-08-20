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
        private Dictionary<string, Dictionary<string, string[]>> AssembliesModels = new Dictionary<string, Dictionary<string, string[]>>();
        private bool Problem { get; set; }
        private bool Verbose { get; set; }
        /// <summary>
        /// Run Silently, Without any output.
        /// </summary>
        public bool Silent { get; set; } = false;

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
            Silent = HasArgKey(args, "-silent");
            APIs(); // List APIS
            ReadAssemblyLinks();
            if (Problem && !Verbose)
            {
                Print("Problems were found during listing, run with -verbose for details.", ConsoleColor.Red);
            }
            PrintFooter();
        }

        /// <summary>
        /// Extract Model and ModelExtension files.
        /// </summary>
        public void ReadAssemblyLinks(){
            foreach (var assembly in AssembliesModels)
            {
                Print(!Silent, assembly.Key, ConsoleColor.Blue);
                Models(assembly);
                ModelExtensions(assembly);
            }
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
                    APISettingsModel server = JsonSerializer.Deserialize<APISettingsModel>(json);
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
                            string[] modelExts = Directory.GetFiles($"{server.RootCode}", "*.model_extension.json", SearchOption.AllDirectories);
                            Dictionary<string, string[]> elements = new Dictionary<string, string[]>() 
                            { 
                                {"Models", models }, {"ModelExtensions", modelExts }
                            };
                            AssembliesModels.Add(server.Namespace, elements);
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
        public void Models(KeyValuePair<string, Dictionary<string, string[]>> assembly) {

                string[] files = assembly.Value.Where(p => p.Key == "Models").Select(p=>p.Value).FirstOrDefault();
                if (files != null && files.Count() > 0)
                {
                    foreach (var modelFile in files)
                    {
                        string json = File.ReadAllText(modelFile);
                        try
                        {
                            CollectionModel model = JsonSerializer.Deserialize<CollectionModel>(json);
                            Print(!Silent,  $"\t -> {model.Collection.Name}", ConsoleColor.DarkGreen);
                        }
                        catch (Exception ex)
                        {
                            if (Verbose)
                            {
                                Print(!Silent, $"\t -> {modelFile}", ConsoleColor.Red);
                                Print(!Silent, $"{ex.Message}", ConsoleColor.Red);
                            }
                            else{
                                Print(!Silent, $"\t -> {modelFile}", ConsoleColor.Red);
                            }
                        }
                        
                    }
                }else{
                    Print(!Silent, $"\t -> No Api Models", ConsoleColor.DarkYellow);
                }
            
        }

        /// <summary>
        /// Read all json Model Extensions (Relations).
        /// </summary>
        public void ModelExtensions(KeyValuePair<string, Dictionary<string, string[]>> assembly)
        {                
                string[] files = assembly.Value.Where(p => p.Key == "ModelExtensions").Select(p => p.Value).FirstOrDefault();
                if (files != null && files.Count() > 0)
                {
                    foreach (var modelFile in files)
                    {
                        string json = File.ReadAllText(modelFile);
                        try
                        {
                            CollectionModel model = JsonSerializer.Deserialize<CollectionModel>(json);
                            Print(!Silent, $"\t -> {model.Collection.Name} (Extend Relationships)", ConsoleColor.DarkGreen);
                        }
                        catch (Exception ex)
                        {
                            if (Verbose)
                            {
                                Print(!Silent, $"\t -> {modelFile}", ConsoleColor.Red);
                                Print(!Silent, $"{ex.Message}", ConsoleColor.Red);
                            }
                            else
                            {
                                Print(!Silent, $"\t -> {modelFile}", ConsoleColor.Red);
                            }
                        }

                    }
                }
                else
                {
                    Print(!Silent, $"\t -> This Assembly is not Extending Any APIs", ConsoleColor.DarkYellow);
                }
            
        }

        public void Plugins(){

        }
    }
}
