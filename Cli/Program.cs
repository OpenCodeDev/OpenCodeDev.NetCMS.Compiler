using System;

namespace OpenCodeDev.NetCMS.Compiler.Cli
{
    public  class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                throw new Exception("No you must have at least one argument.");
            }
            CommandController.Run(args[0], args);
            //Console.ReadLine();
        }

        public static  void PrintLogo(){ 
        

        }
    }
}
