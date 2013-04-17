using System.Collections.Generic;

namespace XeroApi.Model.Serialize
{
    public interface IModelSerializer
    {
        T DeserializeTo<T>(string xml)
            where T : class;

        string Serialize<TModel>(ICollection<TModel> itemsToSerialise)
            where TModel : ModelBase;

        string Serialize<TModel>(TModel itemToSerialise)
            where TModel : ModelBase;

        string MimeType { get; }
    }

    public interface IXmlModelSerializer : IModelSerializer
    {
        string Serialize2<TModel>(TModel itemsToSerialise)
            where TModel : ModelBase;
    }
}