using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel
{
   public class PropertiesItemModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Default { get; set; }
        public bool Private { get; set; }
        public bool Unique { get; set; }
        public List<AttributesItemModel> Attributes { get; set; }
    }
}
