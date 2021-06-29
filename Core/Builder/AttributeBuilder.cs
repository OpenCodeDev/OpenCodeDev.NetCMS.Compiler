using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class AttributeBuilder
    {
        public string _Name { get; private set; }
        public string _Params { get; private set; }

        public bool _AbstractAllowed { get; set; }

        public AttributeBuilder(string pName)
        {
            _Name = pName;
        }

        public AttributeBuilder(string pName, string parameters) : this(pName)
        {
            _Params = parameters;
        }

        public override string ToString()
        {
            return $@"[{_Name}({_Params})]{Environment.NewLine}";
        }
    }
}
