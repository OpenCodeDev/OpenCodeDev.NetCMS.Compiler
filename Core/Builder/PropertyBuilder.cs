using OpenCodeDev.NetCMS.Compiler.Core.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    
    public class PropertyBuilder
    {
        public string _Name { get; private set; }
        public bool _IsPublic { get; private set; }
        public bool _HasPrivateGet { get; private set; }
        public bool _HasPrivateSet { get; private set; }

        public Type _Type { get; private set; }

        /// <summary>
        /// If set, other prop are ignored.
        /// </summary>
        public string _Inline { get; private set; }

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
            _Type = TypeHandler.ConvertStringToType(type);
        }
        public PropertyBuilder(string pName, string type, bool isPublic, bool hasPrivateGet, bool hasPrivateSet) : this(pName, type, isPublic)
        {
            // Only properly set to avoid compile time error
            if (isPublic)
            {
                if ((hasPrivateGet && !hasPrivateGet) || (!hasPrivateGet && hasPrivateGet))
                {
                    _HasPrivateGet = hasPrivateGet;
                    _HasPrivateSet = hasPrivateSet;
                }
            }

        }


        /// <summary>
        /// Compile the Object to String
        /// </summary>
        public virtual string SquashToString(){
            return "";
        }

        public override string ToString()
        {
            return (_Inline == null ? SquashToString() : _Inline);
        }
    }
}
