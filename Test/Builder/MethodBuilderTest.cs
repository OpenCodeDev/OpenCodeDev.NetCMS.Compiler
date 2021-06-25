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
    public class MethodBuilderTest
    {

        [TestMethod]
        public void Inline_vs_Built_Success()
        {
            string code = $@"public void Method(){{}}";
            InterfaceBuilder builder = new InterfaceBuilder(code);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void BuiltInline_vs_Built_Success()
        {
            string code = $@"public void Method(){{}}";
            MethodBuilder builder = new MethodBuilder(code);
            MethodBuilder builderB = new MethodBuilder("Method", "void", true);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(builderB.ToString().Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_1()
        {
            string code = $@"public void Method(){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_2()
        {
            string code = $@"public void Method(string param1){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_3()
        {
            string code = $@"public void Method(string param1, string param2){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_4()
        {
            string code = $@"public static void Method(string param1, string param2){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2") { _IsStatic = true };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Integrity_5()
        {
            string code = $@"protected virtual void Method(string param1, string param2){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2") { _Accessor = "protected virtual" };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Attribute_1()
        {
            string code = $@"[OperationContract()]public void Method(string param1, string param2){{}}";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2");
            builder.Attribute(new AttributeBuilder("OperationContract"));
            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Attribute_2()
        {
            string code = $@"[OperationContract()]void Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2");
            builder.Attribute(new AttributeBuilder("OperationContract") {  _AbstractAllowed = true});
            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_1()
        {
            string code = $@"void Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "void", true, "string param1, string param2");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_2()
        {
            string code = $@"Task Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "async Task", true, "string param1, string param2");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_3()
        {
            string code = $@"Task<string> Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "async Task<string>", true, "string param1, string param2");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_4()
        {
            string code = $@"Task<string> Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "async Task<string>", true, "string param1, string param2") { _IsStatic = true };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Abstract_Integrity_5()
        {
            string code = $@"Task<string> Method(string param1, string param2);";
            MethodBuilder builder = new MethodBuilder("Method", "async Task<string>", true, "string param1, string param2") { _Accessor = "protected virtual" };

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToAbstact().Replace(" ", String.Empty));
        }
    }
}
