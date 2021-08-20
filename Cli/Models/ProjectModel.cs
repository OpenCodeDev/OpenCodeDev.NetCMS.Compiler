using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Models
{
    public class ProjectModel
    {
        public ProjectModelGeneral General { get; set; }
        public List<ProjectModelPlugin> Plugins {get; set;}
        public List<ProjectModelSide> Sides {get; set; }


        /// <summary>
        /// Validate if Top Level is not null (General)
        /// </summary>
        /// <returns></returns>
        public bool ValidTopLevel(){
            return General != null && Sides != null && Sides.Count == 3;
        }

        public void Load(string location){
            if (!File.Exists(location))
            {
                throw new Exception($"Cannot locate {location}");
            }
            
            string json = File.ReadAllText(location);
            var loaded = JsonSerializer.Deserialize<ProjectModel>(json);
            General = loaded.General;
            Plugins = loaded.Plugins;
        }

        public void Save(string location)
        {
            string json = JsonSerializer.Serialize(this, typeof(ProjectModel), new JsonSerializerOptions() { WriteIndented = true } );
            File.WriteAllText(location, json);
        }
    }
}
