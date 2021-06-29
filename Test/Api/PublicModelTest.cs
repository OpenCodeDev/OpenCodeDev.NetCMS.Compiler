using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core.Api.Models;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class PublicModelTest
    {
        [TestMethod]
        public void Test_ClassBuilder_1()
        {
            string path = Directory.GetCurrentDirectory();
            JObject code = JObject.Parse(File.ReadAllText($"{path}\\_Test_Data\\good.model.json"));
            // Build Public Model Controller.
            //List<ClassBuilder> cBuild = PublicModelController.Build($"C:\\Users\\Admin\\source\\repos\\OpenCodeDev.NetCMS\\Shared\\");
        }
    }
}
