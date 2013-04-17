using System.Collections.Generic;
using ServiceStack.Text;
using ServiceStack.Text.Json;

namespace XeroApi.Model.Serialize
{
    public class JsonModelSerializer : IModelSerializer
    {
        public T DeserializeTo<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonReader<T>.Parse(json) as T;            
        }

        public string Serialize<TModel>(ICollection<TModel> itemsToSerialise) where TModel : ModelBase
        {
            return itemsToSerialise.ToJson();
        }

        public string Serialize<TModel>(TModel itemToSerialise) where TModel : ModelBase
        {
            return itemToSerialise.ToJson();
        }

        public string MimeType { get { return "application/json"; } }
    }
}
