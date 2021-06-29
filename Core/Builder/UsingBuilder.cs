using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class UsingBuilder
    {
        public string _Using { get; set; }

        public UsingBuilder(string pUsing)
        {
            _Using = pUsing;
        }

        public override string ToString()
        {
            return $"using {_Using};{Environment.NewLine}";
        }
    }
}
