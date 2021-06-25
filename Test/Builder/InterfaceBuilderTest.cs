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
    public class InterfaceBuilderTest
    {

        [TestMethod]
        public void Inline_vs_Built_Success()
        {
            string code = $@"namespace test {{  public interface Test {{ string Prop {{get; set;}} }}  }}";
            InterfaceBuilder builder = new InterfaceBuilder(code);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void BuiltInline_vs_Built_Success()
        {
            string code = $@"namespace test {{  public interface Test {{ string Prop {{get; set;}} }}  }}";
            InterfaceBuilder builder = new InterfaceBuilder(code);
            InterfaceBuilder builderB = new InterfaceBuilder("Test", "test", new List<MethodBuilder>(), new List<PropertyBuilder>() { new PropertyBuilder("Prop", "string", true) }, new List<UsingBuilder>());

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(builderB.ToString().Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Attribute_1()
        {
            string code = $@"namespace test {{ [ServiceContract()] public interface Test {{ string Prop {{get; set;}} }}  }}";
            InterfaceBuilder builder = new InterfaceBuilder("Test", "test", new List<MethodBuilder>(), new List<PropertyBuilder>() { new PropertyBuilder("Prop", "string", true) }, new List<UsingBuilder>());
            builder.Attribute(new AttributeBuilder("ServiceContract"));
            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }
    }
}
