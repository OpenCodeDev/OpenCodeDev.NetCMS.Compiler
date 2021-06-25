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
    public class PropertyBuilderTest
    {
        [TestMethod]
        public void Inline_vs_Built_Success()
        {
            string code = $@"public string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder(code);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void BuiltInline_vs_Built_Success()
        {
            string code = $@"public string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder(code);
            PropertyBuilder builderB = new PropertyBuilder("Test", "string", true);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(builderB.ToString().Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_1()
        {
            string code = $@"public string Test {{get; private set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true, false, true);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_2()
        {
            string code = $@"public string Test {{private get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true, true, false);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_3()
        {
            string code = $@"private string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true, true, true);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_4()
        {
            string code = $@"private string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", false, false, false);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_5()
        {
            string code = $@"public static string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true) { _IsStatic = true };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_6()
        {
            string code = $@"protected virtual string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true) { _Accessor = "protected virtual" };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_7()
        {
            string code = $@"public string Test {{get; set;}} =""TrulGh'or"";";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true);
            builder.Value(@"""TrulGh'or""");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Attribute_1()
        {
            string code = $@"[ProtoMember(1)] public string Test {{get; set;}} =""TrulGh'or"";";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true);
            builder.Value(@"""TrulGh'or""");
            builder.Attribute(new AttributeBuilder("ProtoMember", "1"));
            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Attribute_2()
        {
            string code = $@"[ProtoMember(1)] string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true);
            builder.Attribute(new AttributeBuilder("ProtoMember", "1") {  _AbstractAllowed = true});
            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_1()
        {
            string code = $@"string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true) { _IsStatic = true };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_2()
        {
            string code = $@"string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true) { _Accessor = "protected" };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_3()
        {
            string code = $@"string Test {{get; set;}}";
            PropertyBuilder builder = new PropertyBuilder("Test", "string", true);
            builder.Value(@"""test""");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

    }
}
