using Newtonsoft.Json.Linq;
using OpenCodeDev.NetCMS.Compiler.Core;
using OpenCodeDev.NetCMS.Compiler.Core.Builder;
using OpenCodeDev.NetCMS.Compiler.Core.Extracter;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Api.Models
{
    public static class PublicModelController
    {
        public static ClassBuilder Build(JObject json){
            PropertyBuilder IdProp = Quickies.CreateIdProp();
            ClassBuilder cBuild = new ClassBuilder("OpenCodeDev.NetCms.Shared.Api.Recipe.Models", "Recipe", "public partial");
            cBuild.Using("ProtoBuf", "System", "System.Collections.Generic", "System.ComponentModel.DataAnnotations", "System.ComponentModel.DataAnnotations.Schema");
            cBuild.Property(new PropertiesExtractor(json).ToList());
            cBuild.Property(IdProp);
            return cBuild;
        }
    }
}
