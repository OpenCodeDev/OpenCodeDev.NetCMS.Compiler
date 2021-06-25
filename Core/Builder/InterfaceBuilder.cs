using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{

    public class InterfaceBuilder
    {
        public string _Name { get; private set; }

        public string _Namespace { get; set; }

        public List<UsingBuilder> _Usings { get; set; } = new List<UsingBuilder>();
        public List<AttributeBuilder> _Attributes { get; set; } = new List<AttributeBuilder>();

        public List<PropertyBuilder> _Properties { get; private set; } = new List<PropertyBuilder>();
        public List<MethodBuilder> _Methods { get; private set; } = new List<MethodBuilder>();

        public string _Inline { get; private set; }
        public InterfaceBuilder(string inline)
        {
            _Inline = inline;
        }

        public InterfaceBuilder(string pName, string pNamespace, List<MethodBuilder> methods,  List<PropertyBuilder> props, List<UsingBuilder> usings)
        {
            _Name = pName;
            _Namespace = pNamespace;
            Property(props);
            Method(methods);
            Using(usings);
        }

        public InterfaceBuilder(string pName, string pNamespace, MethodBuilder method, PropertyBuilder prop, UsingBuilder pUsing) : 
        this(pName, pNamespace, new List<MethodBuilder>() { method }, new List<PropertyBuilder>() { prop }, new List<UsingBuilder>() { pUsing }) {}

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
            return $@" {String.Join(" ", _Usings.Select(p=>p.ToString()))} namespace {_Namespace} {{ {String.Join(" ", _Attributes.Select(p=>p.ToString()))} public interface {_Name} {{ {String.Join("", _Properties.Select(p=>p.ToAbstact()))} {String.Join("", _Methods.Select(p=>p.ToAbstact()))} }} }}";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
