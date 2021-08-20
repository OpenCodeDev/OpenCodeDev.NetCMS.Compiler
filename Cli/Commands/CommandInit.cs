using OpenCodeDev.NetCMS.Compiler.Cli.Models;
using System;
using System.Collections.Generic;
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
            SkipNameCheck = HasArgKey(args, "-nocheck");
            Verbose = HasArgKey(args, "-verbose");
            Project = new ProjectModel();
            Project.General = new ProjectModelGeneral();
            SetProjectName();
        }

        public void LoadProject(){

        }

        public void SetProjectName(){
            //TODO: Check if name exist on official repos.
            bool namePassed = false;
            string projName = "";
            string pattern = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";            
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
