using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Core.Compiler
{
    [Generator]
    public class TestGenerator : ISourceGenerator
    {



    
        public void Execute(GeneratorExecutionContext context)
        {

            context.AddSource("RecipeModel",
            SourceText.From("using Api.Shared; namespace Api.Server {public class RecipeModel : RecipePublicModel {public string Priv {get; set;}}}", Encoding.UTF8));
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
