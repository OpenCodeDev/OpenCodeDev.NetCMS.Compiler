using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Extracter
{
    public class AttributesExtractor
    {
        public List<AttributeBuilder> _Attributes { get; set; } = new List<AttributeBuilder>();
        public AttributesExtractor(string json)
        {
            LoopAttributes(JsonSerializer.Deserialize<AttributesModel>(json));
        }

        public AttributesExtractor(List<AttributesItemModel> items)
        {
            LoopAttributes(new AttributesModel() { Attributes = items });
        }
        public void LoopAttributes(AttributesModel items)
        {
            foreach (var item in items.Attributes)
            {
                _Attributes.Add(new AttributeBuilder(item.Name, item.Value));
            }


        }

        public List<AttributeBuilder> ToList()
        {
            return _Attributes;
        }
        public override string ToString()
        {
            return $"{String.Join(" ", _Attributes.Select(p => p.ToString()))}";
        }
    }
}
