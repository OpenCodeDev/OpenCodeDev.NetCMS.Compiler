using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Builder.JsonModel;
using OpenCodeDev.NetCMS.Compiler.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OpenCodeDev.NetCMS.Compiler.Core.Extracter
{
    public class PropertiesExtractor
    {
        public List<PropertyBuilder> _Props { get; private set; } = new List<PropertyBuilder>();
        public PropertiesExtractor(string json): this(JsonSerializer.Deserialize<PropertiesModel>(json).Properties) { }

        public PropertiesExtractor(List<PropertiesItemModel> props)
        {
            LoopProps(props);
        }

        private void LoopProps(List<PropertiesItemModel> items){
            foreach (var item in items)
            {
                    List<AttributeBuilder> attributes = new AttributesExtractor(item.Attributes).ToList();

                    string defaultValue = null;
                    if (item.Default != null)
                    {
                        defaultValue = item.Default;
                    }
                    
                    PropertyBuilder build = new PropertyBuilder(item.Name, item.Type, !item.Private);
                    
                    build.Attribute(attributes);
                    
                    build.Value(defaultValue);
             
                    _Props.Add(build);
               
            }
        }

        public List<PropertyBuilder> ToList(){
            return _Props;
        }
        public override string ToString()
        {
            return $"{String.Join(" ", _Props.Select(p => p.ToString()))}";
        }
    }
}
