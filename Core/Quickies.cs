using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core
{
    public static class Quickies
    {
        /// <summary>
        /// Build an Identifier field for EF CORE <br/>
        /// Includes [Key] [Column], [Required(ErrorMessage = "Field 'Id' is required")],<br/>
        /// [ProtoMember(x)] and [RegularExpression(GUID Default Protection, ErrorMessage="Field 'Id' must be defined.")] 
        /// </summary>
        /// <returns></returns>
        public static PropertyBuilder CreateIdProp(){
            PropertyBuilder identifierGuid = new PropertyBuilder("Id", "System.Guid", true);
            identifierGuid.Attribute("Key", "");
            identifierGuid.Attribute("Column", "");
            identifierGuid.Attribute("Required", "");
            identifierGuid.Attribute("ProtoMember", "1");
            identifierGuid.Attribute("DatabaseGenerated", "DatabaseGeneratedOption.Identity");
            identifierGuid.Attribute("RegularExpression", @"""^((?!00000000-0000-0000-0000-000000000000).)*$"", ErrorMessage = ""Field 'Id' must be defined.""");
            return identifierGuid;
        }

        /// <summary>
        /// Create Guid Property. <br/>
        /// Includes [Column], [Required(ErrorMessage = "Field 'name' is required")],<br/>
        /// [ProtoMember(x)] and [RegularExpression(GUID Default Protection, ErrorMessage="Field 'name' must be defined.")] 
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="order">ProtoMember Order.</param>
        /// <returns></returns>
        public static PropertyBuilder CreateGuidProp(string name, int order)
        {
            PropertyBuilder identifierGuid = new PropertyBuilder(name, "System.Guid", true);
            identifierGuid.Attribute("Column", "");
            identifierGuid.Attribute("Required", $@"ErrorMessage = ""Field '{name}' is required""");
            identifierGuid.Attribute("ProtoMember", $"{order}");
            identifierGuid.Attribute("RegularExpression", $@"""^((?!00000000-0000-0000-0000-000000000000).)*$"", ErrorMessage = ""Field '{name}' must be defined.""");
            return identifierGuid;
        }






    }
}
