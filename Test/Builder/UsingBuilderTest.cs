using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class UsingBuilderTest
    {
        [TestMethod]
        public void Test_Integrity_1()
        {
            string code = $@"using System.Text;";
            UsingBuilder builder = new UsingBuilder("System.Text");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }
    }
}
