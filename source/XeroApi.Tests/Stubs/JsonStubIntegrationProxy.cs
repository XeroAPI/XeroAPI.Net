using System;

namespace XeroApi.Tests.Stubs
{
    internal class JsonStubIntegrationProxy : StubIntegrationProxyBase
    {
        protected override string GenerateSampleResponse(string elementName)
        {
            return "{\"Id\":\"" + Guid.NewGuid() +
                   "\",\"Status\":\"OK\",\"ProviderName\":\"NullIntegrationProxy\",\"DateTimeUTC\":\"" +
                   DateTime.UtcNow.ToString("s") + "\"," + elementName + "s\":[{}]";
        }
    }
}