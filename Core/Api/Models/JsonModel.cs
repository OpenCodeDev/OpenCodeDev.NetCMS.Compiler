using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api.Models
{
    public class JsonModel
    {
        public CollectionItemModel Collection { get; set; }
        public List<AttributesItemModel> Attributes { get; set; }
        public List<string> Usings { get; set; }
        public List<PropertiesItemModel> Properties { get; set; }
    }
}
