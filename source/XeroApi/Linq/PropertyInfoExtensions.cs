using System;
using System.Linq;
using System.Reflection;

namespace XeroApi.Linq
{
    public static class PropertyInfoExtensions
    {

        public static string SafeName(this PropertyInfo input)
        {
            return (input == null) ? null : input.Name;
        }

        public static Type SafeType(this PropertyInfo input)
        {
            return (input == null) ? null : input.PropertyType;
        }

        public static bool HasAttribute(this PropertyInfo input, Type attributeType)
        {
            var attributes = input.GetCustomAttributes(attributeType, false);
            return attributes.Count() > 0;
        }

    }
}
