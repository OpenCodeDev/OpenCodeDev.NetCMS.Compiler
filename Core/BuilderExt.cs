using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core
{
    public static class BuilderExt
    {
        public static List<UsingBuilder> ToUsings(this string[] arr)
        {
            return arr.Select(p => new UsingBuilder(p)).ToList();
        }



    }
}
