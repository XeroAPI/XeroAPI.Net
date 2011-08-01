using System;

namespace XeroApi.Model
{
    public static class ModelTypeHelper
    {

        public static Type GetElementCollectionType(Type elementType)
        {
            // Forgive me...
            return Type.GetType(elementType.Namespace + "." + elementType.Name + "s");
        }

    }
}
