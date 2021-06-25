using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core
{
    public static class ExceptionExt
    {
        public static JToken ThrowOnNull(this JToken jToken, string message)
        {
            if (jToken == null)
            {
                throw new Exception(message);
            }
            return jToken;
        }


        public static bool FalseOnNullOrCheckBool(this JToken jToken){
            if (jToken == null || jToken.Type != JTokenType.Boolean)
            {
                return false;
            }
            return jToken.ToObject<bool>();
        }
        
    }
}
