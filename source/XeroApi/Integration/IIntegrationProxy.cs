using XeroApi.Linq;

namespace XeroApi.Integration
{
    public interface IIntegrationProxy
    {
        string FindElements(ApiQueryDescription apiQueryDescription);

        string GetElement(string endpointName, string itemId);

        string UpdateOrCreateElements(string endpointName, string body);
        
        string CreateElements(string endpointName, string body);
    }
}
