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
    public class CommandList
    {
        private string CurrentDirectory { get; set; }
        public CommandList(string currentDir){
            CurrentDirectory = currentDir;
        }

        /// <summary>
        /// Run List Command
        /// </summary>
        public void Run(string[] args)
        {
            
        }

        public void Models(){
            string[] files = Directory.GetFiles($"{CurrentDirectory}", "server.json", SearchOption.AllDirectories);
            foreach (var item in files)
            {
                FileInfo info = new FileInfo(item);
                string json = File.ReadAllText(item);
                ProjectSettingsModel server = JsonSerializer.Deserialize<ProjectSettingsModel>(json);

            }
        }
        
        public void Plugins(){

        }
    }
}
