﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;

namespace OpenCodeDev.NetCMS.Compiler.Test
{
    [TestClass]
    public class AttributeBuilderTest
    {
        [TestMethod]
        public void Test_Integrity_1()
        {
            string code = $@"[Column(""Test Col"", ErrorMessage=""This is an error msg."")]";
            AttributeBuilder builder = new AttributeBuilder("Column", @"""Test Col"", ErrorMessage=""This is an error msg.""");

            //Assert.AreEqual(1, 1);
            Assert.AreEqual(code.Replace(" ", String.Empty), builder.ToString().Replace(" ", String.Empty));
        }
    }
}
