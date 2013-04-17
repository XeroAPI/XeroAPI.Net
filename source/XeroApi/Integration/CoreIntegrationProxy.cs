using DevDefined.OAuth.Consumer;
using XeroApi.Model.Serialize;

namespace XeroApi.Integration
{
    public class CoreIntegrationProxy : IntegrationProxy
    {
        public CoreIntegrationProxy(IOAuthSession oauthSession, string mimeType) :
            base(oauthSession, "api.xro/2.0/", mimeType)
        {
        }
    }
}