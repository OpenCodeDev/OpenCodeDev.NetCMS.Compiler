using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Builder
{
    public class ClassBuilder
    {
        public string _Name { get; set; }
        public string _Modifier { get; set; }

        public List<PropertyBuilder> Properties { get; set; } = new List<PropertyBuilder>();

        public ClassBuilder(string cName, string cModifier){
            _Name = cName;
            _Modifier = cModifier;
        }

        /// <summary>
        /// Add a Property to the class (duplicate are ignored)
        /// </summary>
        public void Property(PropertyBuilder prop)
        {
            if (Properties.Count(p=>p._Name == prop._Name) <= 0)
            {
                Properties.Add(prop);
            }
        }

        /// <summary>
        /// Add a list of properties (duplicate are ignored)
        /// </summary>
        /// <param name="props"></param>
        public void Property(List<PropertyBuilder> props)
        {
            foreach (var item in props)
            {
                Property(item);
            }
        }
    }
}
