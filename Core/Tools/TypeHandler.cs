using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCodeDev.NetCMS.Compiler.Core.Tools
{
    public static class TypeHandler
    {
        /// <summary>
        /// If type is not allowed, exception will be thrown
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type ConvertStringToType(string type){
            if (typeof(string).ToString() == type) { return typeof(string); }
            if (typeof(double).ToString() == type) { return typeof(double); }
            if (typeof(float).ToString() == type) { return typeof(float); }
            if (typeof(int).ToString() == type) { return typeof(int); }
            if (typeof(long).ToString() == type) { return typeof(long); }
            if (typeof(Guid).ToString() == type) { return typeof(Guid); }
            throw new Exception($"Type {type} is not officially supported.");
        }

        public static bool IsAllowedStringType(string type)
        {
            try
            {
                ConvertStringToType(type);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
