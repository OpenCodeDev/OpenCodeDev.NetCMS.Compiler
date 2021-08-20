using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Modules
{
    public class LoadModels
    {
        public Dictionary<string, JsonModel[]> Models = new Dictionary<string, JsonModel[]>();
        public LoadModels(Dictionary<string, Dictionary<string, string[]>> assemblies)
        {
            LoadAllModels(assemblies);
        }
        public void LoadAllModels(Dictionary<string, Dictionary<string, string[]>> assemblies)
        {
            
            foreach (var assembly in assemblies)
            {
                // Load All Models
                foreach (var item in assembly.Value.Where(p => p.Key == "Models"))
                {
                    Models.Add(assembly.Key, item.Value.Select(p => JsonSerializer.Deserialize<JsonModel>(File.ReadAllText(p))).ToArray());
                }
            }
        }
    }
}
