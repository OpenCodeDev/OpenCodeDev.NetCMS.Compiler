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
        protected virtual bool HasArgKey(string[] args, string input)
        {
            string val = args.SkipWhile(p => p != input).FirstOrDefault();
            // Either null or if start with - mean next element is arg so the arg is unset.
            return val == null ? false : true;
        }

        protected virtual void ValidateFolder(string path)
        {
            Console.WriteLine($"Validating {path}");
            if (!Directory.Exists(path))
            {
                throw new Exception($"Folder {path} doesn't exist.");
            }
        }
    
            /// <summary>
            /// Print to console with possibly of quickly define color for single line.
            /// </summary>
            /// <param name="message"></param>
            /// <param name="color"></param>
            /// <param name="bg"></param>
        protected virtual void Print(string message, ConsoleColor color = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black){
            Console.ForegroundColor = color;
            Console.BackgroundColor = bg;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Verbose mode wont actually print unless TRUE.
        /// </summary>
        protected virtual void Print(bool verboseMode, string message, ConsoleColor color = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            if (verboseMode)
            {
                Print(message, color, bg);
            }
        }


        protected void PrintHeader(){
            Version version = typeof(Commenter).Assembly.GetName().Version;
            string v = $"v{version.Major}.{version.Minor}.{version.MajorRevision} Rev.{version.Revision}";
            Print(@"╔══════════════════════════════════════╗");
            Print(@"║  _   _      _    ____ __  __ ____    ║");
            Print(@"║ | \ | | ___| |_ / ___|  \/  / ___|   ║");
            Print(@"║ |  \| |/ _ \ __| |   | |\/| \___ \   ║");
            Print(@"║ | |\  |  __/ |_| |___| |  | |___) |  ║");
            Print(@"║ |_| \_|\___|\__|\____|_|  |_|____/   ╚══");
            Print($"║                           {v}    ");
            Print($"╚═════════════════════════════════════════════");
            Print(@"║ Tracking : True");
            Print(@"╚════════════════════════════════════════════════");
            Print(@"PRIVACY NOTICE: We are currently tracking errors and usage to improve a tackle issues. (Run: 'tracking -status off' to disable)", ConsoleColor.Yellow);
            Console.WriteLine(" ");
            Console.WriteLine(" ");
        }

        protected void PrintFooter(){
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Print(@"╔══════════════════════════════════════╗", ConsoleColor.Magenta);
            Print(@"║   ♥ Thanks for using NetCMS-CLI ♥    ║", ConsoleColor.Magenta);
            Print($"║     Visit https://netcms.opencodedev.com/ for more stuff.   ", ConsoleColor.Magenta);
            Print($"╚═════════════════════════════════════════════════════════════", ConsoleColor.Magenta);
        }
    }
}
