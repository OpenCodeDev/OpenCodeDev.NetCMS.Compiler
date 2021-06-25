using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class EnumBuilder
    {
        public string _Namespace { get; private set; }
        public string _Name { get; private set; }
        public List<string> Elements { get; set; } = new List<string>();

        public string _Inline { get; set; }
        public EnumBuilder(string inline){
            _Inline = inline;
        }

        public EnumBuilder(string pName, string pNameSpace, List<string> elements){
            Elements = elements;
            _Name = pName;
            _Namespace = pNameSpace;
        }
        public virtual string SquashToString()
        {
            return $@"namespace {_Namespace} {{ public enum {_Name} {{  {String.Join(",", Elements)} }} }}";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
