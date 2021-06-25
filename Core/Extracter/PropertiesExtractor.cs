using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Extracter
{
    public class PropertiesExtractor
    {
        public List<PropertyBuilder> _Props { get; private set; }
        public PropertiesExtractor(JObject json)
        {
            LoopProps(json.SelectToken("Properties")
            .ThrowOnNull("Cannot find property list in one the model.json. Most likely due to human editing or creating the file.").ToObject<JObject>());
        }

        private void LoopProps(JObject items){
            foreach (var item in items)
            {
                if (item.Value != null)
                {
                    string typeString = item.Value.SelectToken("Type").ThrowOnNull("Type of property is undefined, type must be defined.").ToString();
                    throw new Exception(typeString);
                    Type type = TypeHandler.ConvertStringToType(typeString);
                    List<AttributeBuilder> attributes = new List<AttributeBuilder>();
                    string defaultValue = null;
                    if (item.Value.SelectToken("Default") != null)
                    {
                        defaultValue = item.Value.SelectToken("Default").ToString();
                    }

                    // Attributes Data Annotation.
                    if (item.Value.SelectToken("Attributes") != null)
                    { foreach (var attrItem in item.Value.SelectToken("Attributes").ToObject<JObject>())
                        { attributes.Add(new AttributeBuilder(attrItem.Key, attrItem.Value.ToString())); } }

                    PropertyBuilder build = new PropertyBuilder(item.Key, type.ToString(), true);
                    build.Attribute(attributes);
                    build.Value(defaultValue);
                    _Props.Add(build);
                }else{
                    Debug.WriteLine($"Prop: {item.Key} has missing information in config, therefore will be ignored.");
                }
               
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
