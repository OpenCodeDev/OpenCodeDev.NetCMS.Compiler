using OpenCodeDev.NetCMS.Compiler.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    
    public class PropertyBuilder
    {
        public string _Name { get; private set; }
        public bool _IsPublic { get; private set; }
        public bool _HasPrivateGet { get; private set; }
        public bool _HasPrivateSet { get; private set; }

        public string _Accessor { get; set; }
        public bool _IsStatic { get; set; }

        public string _Type { get; private set; }
        public string _Value { get; private set; }

        /// <summary>
        /// If set, other prop are ignored.
        /// </summary>
        public string _Inline { get; private set; }
        public List<AttributeBuilder> _Attributes { get; set; } = new List<AttributeBuilder>();

        /// <summary>
        /// Pass a full inline property string (Disable any other feature)
        /// </summary>
        /// <param name="inline"></param>
        public PropertyBuilder(string inline)
        {
            _Inline = inline;
        }
        public PropertyBuilder(string pName, string type, bool isPublic)
        {
            _Name = pName;
            _IsPublic = isPublic;
            _Type = type;
        }
        
        public PropertyBuilder(string pName, string type, bool isPublic, bool hasPrivateGet, bool hasPrivateSet) : this(pName, type, (hasPrivateGet && hasPrivateSet ? false : isPublic))
        {
            // Only properly set to avoid compile time error
            if (isPublic)
            {
                if ((hasPrivateSet && !hasPrivateGet) || (!hasPrivateSet && hasPrivateGet))
                {
                    _HasPrivateGet = hasPrivateGet;
                    _HasPrivateSet = hasPrivateSet;
                }
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

        public virtual void SetStatic(){
            _IsStatic = true;
        }

        public virtual void Value(string val){
            _Value = $" = {val};";
        }


        public virtual string ToAbstact()
        {
            return $@"{String.Join(" ", _Attributes.Where(p => p._AbstractAllowed).Select(p => p.ToString()))} {_Type} {_Name} {{ get; set;}}";
        }

        /// <summary>
        /// Compile the Object to String
        /// </summary>
        public virtual string SquashToString(){
            return $@" {String.Join(" ", _Attributes.Select(p => p.ToString()))} {(_Accessor == null ? $"{(_IsPublic ? "public" : "private")} {(_IsStatic ? "static" : "")}" : _Accessor)} {_Type} {_Name} {{ {(_HasPrivateGet ? "private" : "")} get; {(_HasPrivateSet ? "private" : "")} set;}} {_Value}";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
