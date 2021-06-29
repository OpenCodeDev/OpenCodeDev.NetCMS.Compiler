using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Builder.Messages
{
    /// <summary>
    /// Build Various Part of the Search Concept in NetCMS.
    /// </summary>
    public static class ApiSearchConcept
    {
        public static void BuildPredicateConditionModels(List<ClassBuilder> publicModels, string currentProjectDir)
        {
            Console.WriteLine("Building Public Models");
            foreach (var modelClass in publicModels)
            {
                
                FileInfo file = new FileInfo($"{currentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.{modelClass._Name}.cs");
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText($"{currentProjectDir}\\.netcms_config\\generated\\shared\\{modelClass._Namespace}.{modelClass._Name}.cs", modelClass.ToString());

            }

        }
    }
}
