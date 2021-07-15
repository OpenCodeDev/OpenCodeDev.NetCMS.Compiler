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
        public static PropertyBuilder CreateIdEFProp(){
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
        /// Create Guid Property. (EF Core<br/>
        /// Includes [Column], [Required(ErrorMessage = "Field 'name' is required")],<br/>
        /// [ProtoMember(x)] and [RegularExpression(GUID Default Protection, ErrorMessage="Field 'name' must be defined.")] 
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="order">ProtoMember Order.</param>
        /// <returns></returns>
        public static PropertyBuilder CreateGuidEFProp(string name, int order)
        {
            PropertyBuilder identifierGuid = new PropertyBuilder(name, "System.Guid", true);
            identifierGuid.Attribute("Column", "");
            identifierGuid.Attribute("Required", $@"ErrorMessage = ""Field '{name}' is required""");
            identifierGuid.Attribute("ProtoMember", $"{order}");
            identifierGuid.Attribute("RegularExpression", $@"""^((?!00000000-0000-0000-0000-000000000000).)*$"", ErrorMessage = ""Field '{name}' must be defined.""");
            return identifierGuid;
        }


        public static PropertyBuilder CreateGuidProp(string name, int order)
        {
            PropertyBuilder identifierGuid = new PropertyBuilder(name, "System.Guid", true);
            identifierGuid.Attribute("Required", $@"ErrorMessage = ""Field '{name}' is required""");
            identifierGuid.Attribute("ProtoMember", $"{order}");
            identifierGuid.Attribute("RegularExpression", $@"""^((?!00000000-0000-0000-0000-000000000000).)*$"", ErrorMessage = ""Field '{name}' must be defined.""");
            return identifierGuid;
        }

        public static PropertyBuilder CreateFetchConditionProp(string name, int order, string ApiName)
        {
            PropertyBuilder prop = new PropertyBuilder(name, $"List<{ApiName}PredicateConditions>", true);
            prop.Attribute("ProtoMember", $"{order}");
            return prop;
        }

        public static PropertyBuilder CreateFetchOrderByProp(string name, int order, string ApiName)
        {
            PropertyBuilder prop = new PropertyBuilder(name, $"List<{ApiName}PredicateOrdering>", true);
            prop.Attribute("ProtoMember", $"{order}");
            return prop;
        }

        public static MethodBuilder CreateSimplePublicMethod(string name, string returnType, string body = "", string parameter = "")
        {
            return new MethodBuilder(name, returnType, true, body, parameter);
        }
        public static List<MethodBuilder> CreateDefaultGrpcsMethods (string name, string modelNS, string messageNS)
        {
            List<MethodBuilder> _Methods = new List<MethodBuilder>();
            _Methods.Add(CreateSimplePublicMethod("Create", $"async Task<{modelNS}.{name}PublicModel>", "", $"{messageNS}.{name}CreateRequest request, CallContext context = default"));
            _Methods.Add(CreateSimplePublicMethod("Fetch", $"async Task<List<{modelNS}.{name}PublicModel>>", "", $"{messageNS}.{name}FetchRequest request, CallContext context = default"));
            _Methods.Add(CreateSimplePublicMethod("FetchOne", $"async Task<{modelNS}.{name}PublicModel>", "", $"{messageNS}.{name}FetchOneRequest request, CallContext context = default"));
            _Methods.Add(CreateSimplePublicMethod("Update", $"async Task<{modelNS}.{name}PublicModel>", "", $"{messageNS}.{name}UpdateOneRequest request, CallContext context = default"));
            _Methods.Add(CreateSimplePublicMethod("UpdateMany", $"async Task<List<{modelNS}.{name}PublicModel>>", "", $"{messageNS}.{name}UpdateManyRequest request, CallContext context = default"));
            _Methods.Add(CreateSimplePublicMethod("Delete", $"async Task<{modelNS}.{name}PublicModel>", "", $"{messageNS}.{name}DeleteRequest request, CallContext context = default"));
            foreach (var item in _Methods)
            {
                item.Attribute(new AttributeBuilder("OperationContract", "") { _AbstractAllowed = true });
            }
            return _Methods;
        }








    }
}
