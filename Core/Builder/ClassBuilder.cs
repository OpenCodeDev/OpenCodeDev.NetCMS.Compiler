using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class ClassBuilder
    {
        public string _Name { get; private set; }
        public string _Modifier { get; private set; }
        public string _Namespace { get; private set; }

        public string _Inheritance { get; set; }



        public List<UsingBuilder> _Usings { get; private set; } = new List<UsingBuilder>();
        public List<MethodBuilder> _Methods { get; private set; } = new List<MethodBuilder>();
        public List<PropertyBuilder> _Properties { get; private set; } = new List<PropertyBuilder>();

        public List<AttributeBuilder> _Attributes { get; private set; } = new List<AttributeBuilder>();

        public string _Inline { get; private set; }

        public ClassBuilder(string pInline)
        {
            _Inline = pInline;
        }

        public ClassBuilder(string cName, string pNamespace, string cModifier)
        {
            _Name = cName;
            _Modifier = cModifier;
            _Namespace = pNamespace;
        }

        public ClassBuilder(string cName, string pNamespace, string cModifier, List<MethodBuilder> methods, List<PropertyBuilder> properties, List<UsingBuilder> usings) : this(cName, pNamespace, cModifier)
        {
            Method(methods);
            Property(properties);
            Using(usings);
        }

        public void Using(UsingBuilder item)
        {
            if (_Usings.Count(p => p._Using == item._Using) <= 0)
            {
                _Usings.Add(item);
            }
        }

        public void Using(List<UsingBuilder> items)
        {
            foreach (var item in items)
            {
                Using(item);
            }
        }

        public void Attribute(AttributeBuilder item)
        {
            if (_Attributes.Count(p => p._Name == item._Name) <= 0)
            {
                _Attributes.Add(item);
            }
        }

        public void Attribute(List<AttributeBuilder> items)
        {
            foreach (var item in items)
            {
                Attribute(item);
            }
        }

        public void Using(params string[] usings)
        {
            foreach (var item in usings)
            {
                Using(new UsingBuilder(item));
            }
        }

        public void Method(MethodBuilder item)
        {
            if (_Methods.Count(p => p._Name == item._Name) <= 0)
            {
                _Methods.Add(item);
            }
        }

        public void Method(List<MethodBuilder> items)
        {
            foreach (var item in items)
            {
                Method(item);
            }
        }

        public void Property(PropertyBuilder prop)
        {
            if (_Properties.Count(p => p._Name == prop._Name) <= 0)
            {
                _Properties.Add(prop);
            }
        }

        public void Property(List<PropertyBuilder> props)
        {
            foreach (var item in props)
            {
                Property(item);
            }
        }

        public virtual string SquashToString()
        {
            return $@"{String.Join(" ", _Usings.Select(p => p.ToString()))} namespace {_Namespace} {{ {String.Join(" ", _Attributes.Select(p=>p.ToString()))} {_Modifier} class {_Name} {(_Inheritance != null ? $": {_Inheritance}" : "")} {{ {String.Join(" ", _Properties.Select(p => p.ToString()))} {String.Join(" ", _Methods.Select(p => p.ToString()))} }} }}";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
