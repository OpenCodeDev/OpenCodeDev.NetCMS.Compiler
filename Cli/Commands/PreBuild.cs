using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace OpenCodeDev.NetCMS.Compiler.Cli.Commands
{
    public class PreBuild : ChiefCommander
    {
        public void Run(string[] args)
        {
            
            if (!HasArg(args, "-output")) { throw new Exception("The argument 'output' is missing."); }
            if (!HasArg(args, "-side")) { throw new Exception("The argument 'side' is missing."); }
            if (!HasArg(args, "-root")) { throw new Exception("The argument 'root' is missing."); }
            if (!HasArg(args, "-target")) { throw new Exception("The argument 'target' is missing."); }
            if (!HasArg(args, "-namespace")) { throw new Exception("The argument 'namespace' is missing."); }
            string side = GetArg(args, "-side"), rootCode = GetArg(args, "-root"), 
            output = GetArg(args, "-output"), target = GetArg(args, "-target"), ns = GetArg(args, "-namespace");

            Console.WriteLine("PreBuild arguments seems okay.");

            side = side.ToLower().Trim();
            if (side != "server" && side != "shared" && side != "client" && side != "admin")
            {
                throw new Exception($"PreBuild first argument must be 'server, shared, admin or client' NOT {side}");
            }
            ValidateFolder(output);
            ValidateFolder(rootCode);

            ProjectSettingsModel projectFile = new ProjectSettingsModel() 
            { Namespace = ns, Output = output, RootCode = rootCode, Target= target };
            string jString = JsonSerializer.Serialize(projectFile);
            ValidateFolder($"{projectFile.RootCode}..\\.netcms_config");
            try
            {
                File.WriteAllText($"{projectFile.RootCode}..\\.netcms_config\\{side}.json", jString);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception("NetCMS cannot write setting files due to lack of permission maybe?");
            }catch{
                throw;
            }
            Console.WriteLine($"PreBuild was successfull {side}.json was created.");
        }


    }
}
