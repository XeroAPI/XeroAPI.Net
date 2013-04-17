using DevDefined.OAuth.Consumer;
using XeroApi.Integration;
using XeroApi.Model.Serialize;

namespace XeroApi
{
    public class Repository : CoreRepository
    {
        public Repository(IOAuthSession oauthSession)
            : base(new CoreIntegrationProxy(oauthSession, MimeTypes.TextXml), new XmlModelSerializer())
        {
        }

        public Repository(IIntegrationProxy integrationProxy)
            : base(integrationProxy, new XmlModelSerializer())
        {
        }

        public Repository(IIntegrationProxy integrationProxy, IModelSerializer serializer)
            : base(integrationProxy, serializer)
        {
        }
    }
}
