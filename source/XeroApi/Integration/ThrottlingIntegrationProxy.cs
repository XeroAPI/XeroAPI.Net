using System;
using System.IO;
using XeroApi.Model;

namespace XeroApi.Integration
{
    public class ThrottlingIntegrationProxy : IIntegrationProxy
    {
        private readonly IIntegrationProxy _innerIntegrationProxy;
        private readonly IRateLimiter _rateLimiter;

        public ThrottlingIntegrationProxy(IIntegrationProxy innerIntegrationProxy, IRateLimiter rateLimiter)
        {
            _innerIntegrationProxy = innerIntegrationProxy;
            _rateLimiter = rateLimiter;
        }

        #region Implementation of IIntegrationProxy

        public string FindElements(IApiQueryDescription apiQueryDescription)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.FindElements(apiQueryDescription);
        }

        public string FindAttachments(string endpointName, string itemId)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.FindAttachments(endpointName, itemId);
        }

        public byte[] FindOne(string endpointName, string itemId, string acceptMimeType)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.FindOne(endpointName, itemId, acceptMimeType);
        }

        public Stream FindOneAttachment(string endpointName, string itemId, string attachmentIdOrFileName)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.FindOneAttachment(endpointName, itemId, attachmentIdOrFileName);
        }

        public string UpdateOrCreateElements(string endpointName, string body)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.UpdateOrCreateElements(endpointName, body);
        }

        public string UpdateOrCreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.UpdateOrCreateAttachment(endpointName, itemId, attachment);
        }

        public string CreateElements(string endpointName, string body)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.CreateElements(endpointName, body);
        }

        public string CreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            EnforceRateLimit();
            return _innerIntegrationProxy.CreateAttachment(endpointName, itemId, attachment);
        }

        private void EnforceRateLimit()
        {
            _rateLimiter.CheckAndEnforceRateLimit(DateTime.Now);
        }

        #endregion
    }
}