using System;

namespace XeroApi.Model
{
    public static class ModelTypeHelper
    {

        public static Type GetElementCollectionType(Type elementType)
        {
            return Type.GetType(elementType.Namespace + "." + Pluralize(elementType.Name));
        }

        public static string Pluralize(string elementName)
        {
            elementName = elementName.Trim();

            if (elementName.EndsWith("s"))
            {
                return elementName;
            }

            // Fugly is as fugly does...

            if (elementName.EndsWith("y"))
            {
                // e.g. currency -> currencies
                return elementName.TrimEnd('y') + "ies";
            }
            
            return elementName + "s";
        }
    }
}
