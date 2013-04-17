using System;
using System.IO;
using System.Text;
using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;

internal abstract class StubIntegrationProxyBase : IIntegrationProxy
{
    public string FindElements(IApiQueryDescription apiQueryDescription)
    {
        LastQueryDescription = apiQueryDescription as LinqQueryDescription;
        return GenerateSampleResponse(apiQueryDescription.ElementName);
    }

    public string FindAttachments(string endpointName, string itemId)
    {
        throw new NotImplementedException();
    }

    public byte[] FindOne(string endpointName, string itemId, string acceptMimeType)
    {
        return Encoding.UTF8.GetBytes(GenerateSampleResponse(endpointName));
    }

    public Stream FindOneAttachment(string endpointName, string itemId, string attachmentIdOrFileName)
    {
        throw new NotImplementedException();
    }

    public string GetElement(string endpointName, string itemId)
    {
        LastEndpointName = endpointName;
        return GenerateSampleResponse(endpointName);
    }

    public string UpdateOrCreateElements(string endpointName, string body)
    {
        LastEndpointName = endpointName;
        return GenerateSampleResponse(endpointName);
    }

    public string UpdateOrCreateAttachment(string endpointName, string itemId, Attachment attachment)
    {
        throw new NotImplementedException();
    }

    public string CreateElements(string endpointName, string body)
    {
        return GenerateSampleResponse(endpointName);
    }

    public string CreateAttachment(string endpointName, string itemId, Attachment attachment)
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

    protected abstract string GenerateSampleResponse(string elementName);
}