using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dennis.Infrastructure.Core.Extensions
{
    static class GenericTypeExtension
    {
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;
            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(p => p.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }
}
