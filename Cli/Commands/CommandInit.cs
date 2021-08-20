using OpenCodeDev.NetCMS.Compiler.Cli.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    /// <summary>
    /// Create a brand new project, pull latest NetCMS starter project repos.
    /// </summary>
    public  class CommandInit: ChiefCommander
    {
        private bool SkipNameCheck { get; set; }
        private bool Verbose { get; set; }
        
        public ProjectModel Project { get; set; }

        private string CurrentDirectory { get; set; }
        public CommandInit(string currentDir){
            CurrentDirectory = currentDir;
        }

        public void Run(string[] args){
            PrintHeader();
            SkipNameCheck = HasArgKey(args, "-nocheck");
            Verbose = HasArgKey(args, "-verbose");
            Project = new ProjectModel();
            Project.General = new ProjectModelGeneral() { GUID = Guid.NewGuid(), Version = "0.0.0", Revision = "1000", AllowTracking = true };

            Project.Plugins = new List<ProjectModelPlugin>();
            SetProjectName();
            SetDisplayName();
            Project.Sides = new List<ProjectModelSide>() {
            new ProjectModelSide() { Namespace = $"{Project.General.Name}.Server", Target = "Debug", RootCode = $"{CurrentDirectory}\\{Project.General.Name}\\Server" },
            new ProjectModelSide() { Namespace = $"{Project.General.Name}.Shared", Target = "Debug", RootCode = $"{CurrentDirectory}\\{Project.General.Name}\\Shared" },
            new ProjectModelSide() { Namespace = $"{Project.General.Name}.Admin", Target = "Debug", RootCode = $"{CurrentDirectory}\\{Project.General.Name}\\Admin" },

            };
            try
            {
                string projPath = $"{CurrentDirectory}\\{Project.General.Name}";
                if (Directory.Exists(projPath))
                {

                    throw new Exception($"Directory {Project} already exist.");
                }
                Directory.CreateDirectory($"{CurrentDirectory}\\{Project.General.Name}");
                Project.Save($"{projPath}\\init.json");
            }
            catch (Exception ex)
            {
                Print(ex.Message, ConsoleColor.Red);
            }
            Print($"Project {Project.General.DisplayName} created!", ConsoleColor.Green);
            PrintFooter();
        }
               
        public void SetDisplayName(){
            bool oK = false;
            string input;
            do
            {
                Print("Enter a display name for the project/plugin: ");
                input = Console.ReadLine();
                Project.General.DisplayName = input;
                if (Project.General.ValidateDisplayName(Verbose))
                { oK = true; }
                else
                {
                    Console.WriteLine("Display Name is invalid.");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            } while (!oK);
        }

        public void SetProjectName(){
            bool namePassed = false;
            string projName;          
            do
            {
                Print("Enter a Namespace for your project/plugin (Eg: Company.NetCMS.PluginName)? ");
                projName = Console.ReadLine();
                Project.General.Name = projName;
                if (Project.General.ValidateName(Verbose, SkipNameCheck)) 
                { namePassed = true; }
                else{
                    Console.WriteLine("Name is invalid.");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            } while (!namePassed);
            
        }
    }
}
