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
    public class PropertiesExtractorTest
    {
        [TestMethod]
        public void Test_Property_Integrity()
        {
            string path = Directory.GetCurrentDirectory();
            JObject code = JObject.Parse(File.ReadAllText($"{path}\\_Test_Data\\good.model.json"));

            PropertiesExtractor extracted = new PropertiesExtractor(code);
            var prop = extracted._Props.Where(p => p._Name.Equals("Name")).First();
            Assert.AreEqual(@"= ""dsdasd"";", prop._Value);
        }
    }
}
