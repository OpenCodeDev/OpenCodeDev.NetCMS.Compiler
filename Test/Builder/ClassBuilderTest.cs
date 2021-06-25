using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class ClassBuilderTest
    {

        [TestMethod]
        public void Test_Inline_Success()
        {
            string code = $@"namespace test.ns {{  public class Test {{ }}  }}";
            ClassBuilder cB = new ClassBuilder(code);

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), cB.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Inline_Failure()
        {
            string code = $@"namespace test.ns {{  public class Test {{ }}  }}";
            ClassBuilder cB = new ClassBuilder($@"namespace test.ns {{  public class Tests {{ }}  }}");

            //Assert.AreEqual(1, 1);
            Assert.AreNotEqual(code.Replace(" ", String.Empty), cB.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Inline_Vs_Built_Success()
        {
            string code = $@"namespace test.ns {{  public class Test {{ }}  }}";
            ClassBuilder cB = new ClassBuilder("Test", "test.ns", "public");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), cB.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Inline_Vs_Built_Failure()
        {
            string code = $@"namespace test.ns {{  public class Name {{ }}  }}";
            ClassBuilder cB = new ClassBuilder("Test", "test.ns", "public");

            //Assert.AreEqual(1, 1);
            Assert.AreNotEqual(code.Replace(" ", String.Empty), cB.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Built_Vs_Built_Success()
        {

            ClassBuilder cBuildA = new ClassBuilder("Test", "test.ns", "public");
            ClassBuilder cBuildB = new ClassBuilder("Test", "test.ns", "public",
            new List<MethodBuilder>() { }, new List<PropertyBuilder>(), new List<UsingBuilder>());

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(cBuildB.ToString().Replace(" ", String.Empty), cBuildA.ToString().Replace(" ", String.Empty));
        }

        [TestMethod]
        public void Test_Built_Vs_Built_Failure()
        {

            ClassBuilder cBuildA = new ClassBuilder("Test", "test.ns", "public");
            ClassBuilder cBuildB = new ClassBuilder("Test", "test.ns", "public",
            new List<MethodBuilder>() { new MethodBuilder("Method", "void", true)}, new List<PropertyBuilder>(), new List<UsingBuilder>());

            //Assert.AreEqual(1, 1);
            Assert.AreNotEqual(cBuildB.ToString().Replace(" ", String.Empty), cBuildA.ToString().Replace(" ", String.Empty));
        }

    }
}
