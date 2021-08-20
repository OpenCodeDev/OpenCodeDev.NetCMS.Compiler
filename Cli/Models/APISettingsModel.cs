using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli
{
   public class APISettingsModel
    {
        public string Namespace { get; set; } = null;
        public string RootCode { get; set; } = null;
        public string Output { get; set; } = null;
        public string Target { get; set; } = null;

        public bool Valid(){
            return Namespace != null && RootCode != null && Output != null && Target != null;
        }
    }
}
