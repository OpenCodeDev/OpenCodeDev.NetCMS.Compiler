using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class MethodBuilder
    {
        public string _Name { get; private set; }
        public string _Accessor { get; set; }
        public bool _IsPublic { get; private set; }
        public bool _IsStatic { get; set; }
        public string _ReturnType { get; private set; }
        public string _Params { get; private set; }
        public string _Body { get; private set; }

        public string _Inline { get; private set; }

        public List<AttributeBuilder> _Attributes { get; set; } = new List<AttributeBuilder>();

        public MethodBuilder(string inline)
        {
            _Inline = inline;
        }

        public MethodBuilder(string pName, string returnType, bool isPublic)
        {
            _Name = pName;
            _IsPublic = isPublic;
            _ReturnType = returnType;
        }

        public MethodBuilder(string pName, string returnType, bool isPublic, string parameters) : this(pName, returnType, isPublic)
        {
            _Params = parameters;
        }

        public MethodBuilder(string pName, string returnType, bool isPublic, string body, string parameters) : this(pName, returnType, isPublic, parameters)
        {
            _Body = body;
        }

        public virtual void SetStatic()
        {
            _IsStatic = true;
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

        public virtual string ToAbstact(){
            string rT = (_ReturnType.Contains("async") ? _ReturnType.Replace("async", String.Empty) : _ReturnType);
            return $@"{String.Join(" ", _Attributes.Where(p=>p._AbstractAllowed).Select(p => p.ToString()))} {rT} {_Name} ({_Params});";
        }

        /// <summary>
        /// Compile the Object to String
        /// </summary>
        public virtual string SquashToString()
        {
            return $@" {String.Join(" ", _Attributes.Select(p => p.ToString()))} {(_Accessor == null ? $"{(_IsPublic ? "public" : "private")} {(_IsStatic ? "static" : "")}" : _Accessor)} {_ReturnType} {_Name} ({_Params})  {{ {_Body} }}";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
