using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Shared
{
    [Generator]
    public class PublicModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            //string wd = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //wd = $"{wd}\\NetCMS\\{context.Compilation.Assembly.Identity.Name}.txt";
            //if (!File.Exists(wd))
            //{
            //    throw new Exception($"{wd} common config file doesn't exist. May be due to project not being properly generated.");
            //}
            //string basePath = String.Join("", File.ReadAllLines(wd)).Replace(" ", String.Empty);
            //string projConfigPath = $"{basePath}.netcms_config\\generated\\shared\\";
            //if (!Directory.Exists(basePath))
            //{
            //    throw new Exception($"{basePath} doesn't exist. Setting provided in {wd} are wrong and should be updated.");
            //}

            //DirectoryInfo dir = new DirectoryInfo($"{projConfigPath}");
            //dir.Create(); // If the directory already exists, this method does nothing.

            //string[] files = Directory.GetFiles(projConfigPath, "*.cs", SearchOption.AllDirectories);
            //foreach (var item in files)
            //{

            //    FileInfo fileInfo = new FileInfo(item);
            //    string code = String.Join("", File.ReadAllLines(item));
            //    context.AddSource(fileInfo.Name.Replace(".", "_"), SourceText.From(code, Encoding.UTF8));
            //}

        }

        public void Initialize(GeneratorInitializationContext context)
        {
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
            Debug.WriteLine("Initalize code generator");
        }
    }
}
