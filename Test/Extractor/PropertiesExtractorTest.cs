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
            PropertiesExtractor extracted = new PropertiesExtractor(File.ReadAllText($"{path}\\_Test_Data\\good.model.json"));
            var prop = extracted._Props.Where(p => p._Name.Equals("Name")).First();
            Assert.AreEqual(@"= ""dsdasd"";".Replace(" ", String.Empty), prop._Value.Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Prop_Integrity_Attributes()
        {
            string path = Directory.GetCurrentDirectory();
            PropertiesExtractor extracted = new PropertiesExtractor(File.ReadAllText($"{path}\\_Test_Data\\good.model.json"));
            var prop = extracted._Props.Where(p => p._Name.Equals("Name")).First();
            Assert.AreEqual(@"[Required()]", prop._Attributes.Where(p => p._Name.Equals("Required")).First().ToString().Replace(" ", String.Empty));
        }


    }
}
