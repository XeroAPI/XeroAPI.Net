using System;
using System.IO;
using System.Text;
using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;

namespace XeroApi.Tests.Stubs
{
    internal class StubIntegrationProxy : IIntegrationProxy
    {
        public string FindElements(IApiQueryDescription apiQueryDescription)
        {
            LastQueryDescription = apiQueryDescription as LinqQueryDescription;
            return GenerateSampleResponseXml(apiQueryDescription.ElementName);
        }

        public string FindAttachments(string endpointName, string itemId)
        {
            throw new NotImplementedException();
        }

        public byte[] FindOne(string endpointName, string itemId, string acceptMimeType)
        {
            return Encoding.UTF8.GetBytes(GenerateSampleResponseXml(endpointName));
        }

        public Stream FindOneAttachment(string endpointName, string itemId, string attachmentIdOrFileName)
        {
            throw new NotImplementedException();
        }

        public string GetElement(string endpointName, string itemId)
        {
            LastEndpointName = endpointName;
            return GenerateSampleResponseXml(endpointName);
        }

        public string UpdateOrCreateElements(string endpointName, string body)
        {
            LastEndpointName = endpointName;
            return GenerateSampleResponseXml(endpointName);
        }

        public string UpdateOrCreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public string CreateElements(string endpointName, string body)
        {
            return GenerateSampleResponseXml(endpointName);
        }

        public string CreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public string ApplyAllocation(CreditNote creditNote, string requestXml)
        {
            throw new NotImplementedException();
        }

        public LinqQueryDescription LastQueryDescription
        {
            get;
            private set;
        }

        public string LastEndpointName
        {
            get; 
            private set; 
        }

        private static string GenerateSampleResponseXml(string elementName)
        {
            return @"
<Response>
    <Id>" + Guid.NewGuid() + @"</Id>
    <Status>OK</Status>
    <ProviderName>NullIntegrationProxy</ProviderName>
    <DateTimeUTC>" + DateTime.UtcNow.ToString("s") + @"</DateTimeUTC><" + elementName + "s" + @" />
    <" + elementName + @"s>
        <" + elementName + @"/>
    </" +elementName+@"s>
</Response>";
        }
    }

}
