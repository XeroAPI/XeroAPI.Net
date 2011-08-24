using System;
using System.Linq;
using System.Reflection;
using XeroApi.Linq;

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

        public static string GetModelItemId<TModel>(TModel model)
            where TModel : ModelBase
        {
            PropertyInfo itemIdProperty = typeof (TModel).GetProperties().FirstOrDefault(prop => prop.HasAttribute(typeof (ItemIdAttribute)));

            if (itemIdProperty == null)
                throw new ArgumentException("The model type '' does not have an [ItemId] attribute specified on one of it's properties");

            var propValue = itemIdProperty.GetValue(model, new object[0]);

            return Convert.ToString(propValue);
        }
    }
}
