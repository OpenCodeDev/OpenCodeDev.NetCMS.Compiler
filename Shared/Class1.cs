using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Shared
{
    [Generator]
    public class TestGenerator : ISourceGenerator
    {




        public void Execute(GeneratorExecutionContext context)
        {

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
