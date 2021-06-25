using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class EnumBuilderTest
    {
        [TestMethod]
        public void Inline_vs_Built_Success()
        {
            string code = $@"namespace test {{  public enum EnumTest {{ Alive, Dead, G_DMode }}  }}";
            EnumBuilder builder = new EnumBuilder(code);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void BuiltInline_vs_Built_Success()
        {
            string code = $@"namespace test {{  public enum EnumTest {{ Alive, Dead, G_DMode }}  }}";
            EnumBuilder builder = new EnumBuilder(code);
            EnumBuilder builderB = new EnumBuilder("EnumTest", "test", new List<string>() { "Alive", "Dead", "G_DMode" });

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(builderB.ToString().Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }
    }
}
