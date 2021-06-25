using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCodeDev.NetCMS.Compiler.Core.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class TypeHandlerTest
    {
        [TestMethod]
        public void Test_TypeString_To_TypeString()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.String"), typeof(string));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeDouble()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.Double"), typeof(double));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeFloat()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.Single"), typeof(float));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeInt()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.Int32"), typeof(int));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeGuid()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.Guid"), typeof(Guid));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeLong()
        {
            Assert.AreEqual(TypeHandler.ConvertStringToType("System.Int64"), typeof(long));
        }

        [TestMethod]
        public void Test_TypeString_To_TypeUnSupported()
        {
        
            Assert.ThrowsException<Exception>(()=>TypeHandler.ConvertStringToType("System.Array"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeString()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.String"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeDouble()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.Double"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeFloat()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.Single"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeInt()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.Int32"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeGuid()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.Guid"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeLong()
        {
            Assert.IsTrue(TypeHandler.IsAllowedStringType("System.Int64"));
        }

        [TestMethod]
        public void Test_IsAllowed_TypeUnSupported()
        {

            Assert.IsFalse(TypeHandler.IsAllowedStringType("System.Array"));
        }
    }
}
