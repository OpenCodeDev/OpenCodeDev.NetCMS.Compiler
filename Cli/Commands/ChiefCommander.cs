using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    public class ChiefCommander
    {
        protected virtual string GetArg(string[] args, string input) {
            try
            {
                string arg = args.SkipWhile(p => p != input).Skip(1).FirstOrDefault();
                return arg;
            }
            catch (Exception)
            {

                throw new Exception($"Couldn't get the argument '{input}' or the argument is invalid.") ;
            }
            
        }
        protected virtual bool HasArg(string[] args, string input)
        {
            string val = args.SkipWhile(p => p != input).Skip(1).FirstOrDefault();
            // Either null or if start with - mean next element is arg so the arg is unset.
            return val == null || val.StartsWith("-") ? false : true;
        }

        protected virtual void ValidateFolder(string path)
        {
            Console.WriteLine($"Validating {path}");
            if (!Directory.Exists(path))
            {
                throw new Exception($"Folder {path} doesn't exist.");
            }
        }
    }
}
