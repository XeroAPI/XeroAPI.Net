using System;

using XSync.Integration.XeroAPI;
using XSync.Integration.XeroAPI.Integration;

namespace XeroApi.Integration
{
    public class NullIntegrationProxy : IIntegrationProxy
    {
        public string FindElements(ApiQueryDescription apiQueryDescription)
        {
            LastQueryDescription = apiQueryDescription;
            return "<Response><Id>" + Guid.NewGuid() + "</Id><Status>OK</Status><ProviderName>NullIntegrationProxy</ProviderName><DateTimeUTC>"+ DateTime.UtcNow.ToString("s") +"</DateTimeUTC><" + apiQueryDescription.ElementName + "s" + " /></Response>";
        }

        public ApiQueryDescription LastQueryDescription
        {
            get; 
            private set;
        }
    }
}
