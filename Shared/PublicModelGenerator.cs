using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Shared
{
    /// <summary>
    /// Build Public Model
    /// </summary>
    [Generator]
    public class PublicModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            JObject modelFile = JObject.Parse(File.ReadAllText(@"C:\\Users\\Admin\\source\\repos\\OpenCodeDev.NetCMS\\Configurations\\Api\\Recipes\\Models\\recipes.model.json"));

            context.AddSource("RecipePublicModel", 
            SourceText.From("namespace Api.Shared {public class RecipePublicModel {public string MyTest {get; set;}}}", Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    //Debugger.Launch();
            //}

            Debug.WriteLine("Initalize code generator");
        }
    }
}
