using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Core.Compiler
{
    public class ClassBuilder : Object
    {
        private string Name { get; set; } = "";
        private string Namespace { get; set; } = "";
        private string ClassModifier { get; set; } = "";
        private string Inheritance { get; set; } = "";
        private List<string> Interfaces { get; set; } = new List<string>();
        private List<string> Methods { get; set; } = new List<string>();
        private List<string> Properties { get; set; } = new List<string>();
        private List<string> Usings { get; set; } = new List<string>();


        /// <summary>
        /// {0} = Namespace, {1} = Class Modifier (Default: private), {2} = Class Name, <br/>
        /// {3} = Inheritance and Implementations (Default: Object), {4} = Class Body
        /// </summary>
        const string TEMPLATE = @"{5} namespace {0} { {1} class {2} : {3} { {4} } }";

        public ClassBuilder(string name, string _namespace, string classModifier){
            Name = name;
            Namespace = _namespace;
            ClassModifier = classModifier;
        }

        public void MethodSet(string fullMethod) { Methods.Add(fullMethod); }
        public void MethodAdd(string fullMethod) { MethodSet(fullMethod); }

        public void UsingAdd(List<string> list)
        {
            foreach (var item in list)
            {
                UsingAdd(item);
            }
        }

        public void UsingAdd(string full)
        {
            if (!Usings.Contains(full))
            {
                Usings.Add(full);
            }
        }
        public void InterfacesAdd(List<string> interfaces)
        {
            foreach (var item in interfaces)
            {
                if (!Interfaces.Contains(item))
                {
                    Interfaces.Add(item);
                }
            }
        }

        public void PropertyAdd(string fullprops)
        {
            if (!Properties.Contains(fullprops))
            {
                Properties.Add(fullprops);
            }
        }

        public void PropertyAdd(List<string> props){
            foreach (var item in props)
            {
                PropertyAdd(item);
            }
        }

        public override string ToString()
        {
            string inheeritTrail = (Interfaces.Count > 0 ? ", " : "");
            string implement = (Inheritance == String.Empty ? "Object" + inheeritTrail : Inheritance + inheeritTrail) + String.Join(",", Interfaces);
            
            return String.Format(TEMPLATE, Namespace, ClassModifier, Name, implement, 
            $"{String.Join("", Properties)} {String.Join("", Methods)}", String.Join("", Usings));
        }
    }
}
