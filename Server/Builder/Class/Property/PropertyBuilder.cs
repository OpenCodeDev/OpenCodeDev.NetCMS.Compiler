using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Builder.Class.Property
{
    public class PropertyBuilder
    {
        public string _name { get; set; } = "";
        public string _modifier { get; set; } = "";

        public PropertyBuilder(string name, string modifier){
            _name = name;
            _modifier = modifier;
        }


    }
}
