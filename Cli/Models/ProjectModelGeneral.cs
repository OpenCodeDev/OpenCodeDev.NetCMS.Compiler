using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenCodeDev.NetCMS.Compiler.Cli.Models
{
    public class ProjectModelGeneral
    {
        /// <summary>
        /// Name of the Assembly's project or plugin.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Display name of the project or plugin.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Generated unique guid (Global ID)
        /// </summary>
        public Guid GUID { get; set; }
        /// <summary>
        /// Current version of the project.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Revision of the project version.
        /// </summary>
        public string Revision { get; set; }
        /// <summary>
        /// Name of the creator, lead or author of the project.
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Company that own or runs the project. (optional)
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Repository url (optional)
        /// </summary>
        public string RepoURL { get; set; }

        /// <summary>
        /// Website of the project (optional)
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Does the project allow us to track its usage and information (not content or code wise).
        /// </summary>
        public bool AllowTracking { get; set; }

        private string WebPattern { get; set; } = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";

        /// <summary>
        /// Validate all with default settings (will validate name on NetCMS server)
        /// </summary>
        /// <returns></returns>
        public bool Valid(bool verbose = false, bool officialcheck = false)
        {
            return ValidateName(verbose) && ValidateDisplayName(verbose) && ValidateGuid(verbose) && ValidateName(verbose, officialcheck) && ValidateRepo(verbose) && ValidateWebsite(verbose) && ValidationVersion(verbose) && ValidateRevision(verbose);
        }

        /// <summary>
        /// Validate the name of the project Assembly's name.
        /// </summary>
        public bool ValidateName(bool verbose = false, bool officialcheck = true)
        {
            if (Name == null)
            {
                if (verbose)
                {
                    Console.WriteLine("Cannot validate name of type null...");
                }
                return false;
            }

            if (Name.Length > 50)
            {
                if (verbose)
                {
                    Console.WriteLine("Name cannot be mroe than 50 characters.");
                }
                return false;
            }

            if (Name.Length <= 10)
            {
                if (verbose)
                {
                    Console.WriteLine("Name must be minimum 10 characters.");
                }
                return false;
            }

            string pattern = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.";
            if (Name.Count(p => !pattern.Contains(p)) > 0)
            {
                if (verbose)
                {
                    Console.WriteLine("Name contains unallowed characters. Allowed = abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.");
                }
                return false;
            }
            if (Name.First() == '.' || Name.Last() == '.')
            {
                if (verbose)
                {
                    Console.WriteLine("Name cannot start nor end with .");
                }
                return false;
            }
            if (officialcheck)
            {
                // TODO: Check on the NetCMS Server for name validation.
            }
            return true;
        }

        /// <summary>
        /// Validate website pattern.
        /// </summary>
        public bool ValidateWebsite(bool verbose = false)
        {
            if (Website != null && !Regex.Match(Website, WebPattern).Success)
            {
                if (verbose)
                {
                    Console.WriteLine("Website is not a valid format.");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validate Repo website pattern.
        /// </summary>
        public bool ValidateRepo(bool verbose = false)
        {
            if (RepoURL != null && !Regex.Match(RepoURL, WebPattern).Success)
            {
                if (verbose)
                {
                    Console.WriteLine("RepoURL is not a valid format.");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if GUID was generated.
        /// </summary>
        public bool ValidateGuid(bool verbose = false)
        {
            if (GUID == Guid.Empty)
            {
                if (verbose)
                {
                    Console.WriteLine("GUID cannot be empty.");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if display name is null.
        /// </summary>
        /// <returns></returns>
        public bool ValidateDisplayName(bool verbose = false)
        {
            if (DisplayName.Length > 50)
            {
                if (verbose)
                {
                    Console.WriteLine("DisplayName cannot be mroe than 50 characters.");
                }
                return false;
            }

            if (DisplayName.Length <= 10)
            {
                if (verbose)
                {
                    Console.WriteLine("DisplayName must be minimum 10 characters.");
                }
                return false;
            }
            return DisplayName != null;
        }
        
        public bool ValidateRevision (bool verbose = false)
        {
            if (Revision == null)
            {
                if (verbose)
                {
                    Console.WriteLine("Revision cannot be null, generally incremented as unique and always 4 digits.");
                }
                return false;
            }

            if (Revision.Length != 4)
            {
                if (verbose)
                {
                    Console.WriteLine("Revision must be 4 digits.");
                }
                return false;
            }

            string pattern = @"0987654321";
            if (Revision.Count(p => !pattern.Contains(p)) > 0)
            {
                if (verbose)
                {
                    Console.WriteLine("Revision must be digit only.");
                }
                return false;
            }

            return true;


        }
       
        /// <summary>
        /// Validate Version Format and Rules
        /// </summary>
        /// <param name="verbose"></param>
        /// <returns></returns>
        public bool ValidationVersion(bool verbose = false)
        {
            if (Version == null)
            {
                if (verbose)
                {
                    Console.WriteLine("Version cannot be null");
                }
                return false;
            }
            string pattern = @"0987654321.";
            if (Version.Count(p => p == '.') != 2)
            {
                if (verbose)
                {
                    Console.WriteLine("Your current version has either more '.' or not enough '.' make sure to have x.x.x, xx.xx.xx or xxx.xx.xx format.");
                }
                return false;
            }

            if (Version.Count(p => !pattern.Contains(p)) > 0)
            {
                if (verbose)
                {
                    Console.WriteLine("Your current version has foreign character... 0987654321 only are allowed.");
                }
                return false;
            }

            if (Version.StartsWith(".") || Version.EndsWith("."))
            {
                if (verbose)
                {
                    Console.WriteLine("Version cannot start or end with .");
                }
                return false;
            }
            string[] splitVersion = Version.Split('.');
            if (splitVersion[0].Length > 2)
            {
                if (verbose)
                {
                    Console.WriteLine("Major (major.minor.fix) version cannot be above 99, if thats the case it is very much not normal behavior!");
                }
                return false;
            }

            if (splitVersion[1].Length > 2)
            {
                if (verbose)
                {
                    Console.WriteLine("Minor (major.minor.fix) version cannot be above 99, if thats the case it is very much not normal behavior! ");
                }
                return false;
            }

            if (splitVersion[2].Length > 2)
            {
                if (verbose)
                {
                    Console.WriteLine("Fix (major.minor.fix) version cannot be above 99, if thats the case it is very much not normal behavior!");
                }
                return false;
            }
            return true;
            }
    }
}
