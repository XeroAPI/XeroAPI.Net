using System;

namespace XeroApi.Tests.Stubs
{
    internal class XmlStubIntegrationProxy : StubIntegrationProxyBase
    {
        protected override string GenerateSampleResponse(string elementName)
        {
            return @"
            <Response>
                <Id>" + Guid.NewGuid() + @"</Id>
                <Status>OK</Status>
                <ProviderName>NullIntegrationProxy</ProviderName>
                <DateTimeUTC>" + DateTime.UtcNow.ToString("s") + @"</DateTimeUTC><" + elementName + "s" + @" />
                <" + elementName + @"s>
                    <" + elementName + @"/>
                </" + elementName + @"s>
            </Response>";
        }
    }
}
