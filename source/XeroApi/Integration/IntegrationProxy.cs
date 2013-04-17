using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using DevDefined.OAuth.Consumer;
using XeroApi.Exceptions;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi.Integration
{
    public abstract class IntegrationProxy : IIntegrationProxy
    {
        private readonly IOAuthSession _oauthSession;
        private readonly string _acceptType;
        private readonly string _endpointSuffix;

        protected IntegrationProxy(IOAuthSession oauthSession, string endpointSuffix, string acceptType)
        {
            _oauthSession = oauthSession;
            _endpointSuffix = endpointSuffix;
            _acceptType = acceptType;
        }

        protected Uri GetUrl(Uri baseUri)
        {
            return new Uri(_oauthSession.ConsumerContext.BaseEndpointUri, _endpointSuffix);
        }

        #region Structured Data Read/Write methods

        public string FindElements(IApiQueryDescription apiQueryDescription)
        {
            IConsumerResponse consumerResponse = CallApi(
                "GET",
                string.Empty,
                GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri),
                GetEndpointName(apiQueryDescription.ElementName),
                apiQueryDescription.ElementId,
                apiQueryDescription.UpdatedSinceDate,
                apiQueryDescription.QueryStringParams,
                _acceptType);

            if (consumerResponse.ResponseCode == HttpStatusCode.NotFound)
            {
                return string.Empty;
            }

            if (consumerResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                throw new ApiResponseException(consumerResponse);
            }

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public byte[] FindOne(string endpointName, string itemId, string acceptMimeType)
        {
            IConsumerResponse consumerResponse = CallApi(
                "GET",
                string.Empty,
                GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri),
                GetEndpointName(endpointName),
                itemId,
                null,
                null,
                acceptMimeType);

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.ByteArray;
            }

            throw new ApiResponseException(consumerResponse);
        }


        public string UpdateOrCreateElements(string endpointName, string body)
        {
            IConsumerResponse consumerResponse = CallApi(
                "POST",
                body,
                GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri),
                GetEndpointName(endpointName),
                null,
                null,
                new NameValueCollection { { "summarizeErrors", "false" } },
                _acceptType);

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public string CreateElements(string endpointName, string body)
        {
            IConsumerResponse consumerResponse = CallApi(
                "PUT",
                body,
                GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri),
                GetEndpointName(endpointName),
                null,
                null,
                new NameValueCollection { { "summarizeErrors", "false" } },
                _acceptType);

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        #endregion


        #region Attachment Read/Write Methods

        public string UpdateOrCreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            Uri uri = ConstructChildResourceUri(GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri), endpointName, itemId, "Attachments", attachment.FileName);

            IConsumerRequest oauthRequest = _oauthSession.Request()
                .ForMethod("POST")
                .WithAcceptHeader(_acceptType)
                .ForUri(uri)
                .WithRequestStream(attachment.ContentStream)
                .SignWithToken();

            var consumerResponse = oauthRequest.ToConsumerResponse();

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public string CreateAttachment(string endpointName, string itemId, Attachment attachment)
        {
            Uri uri = ConstructChildResourceUri(GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri), endpointName, itemId, "Attachments", attachment.FileName);

            IConsumerRequest oauthRequest = _oauthSession.Request()
                .ForMethod("PUT")
                .WithAcceptHeader(_acceptType)
                .ForUri(uri)
                .WithRequestStream(attachment.ContentStream)
                .SignWithToken();

            var consumerResponse = oauthRequest.ToConsumerResponse();

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public string FindAttachments(string endpointName, string itemId)
        {
            Uri uri = ConstructChildResourceUri(GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri), endpointName, itemId, "Attachments", null);

            IConsumerRequest oauthRequest = _oauthSession.Request()
                .ForMethod("GET")
                .WithAcceptHeader(MimeTypes.Unknown)
                .ForUri(uri)
                .SignWithToken();

            var consumerResponse = oauthRequest.ToConsumerResponse();

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public Stream FindOneAttachment(string endpointName, string itemId, string attachmentIdOrFileName)
        {
            Uri uri = ConstructChildResourceUri(GetUrl(_oauthSession.ConsumerContext.BaseEndpointUri), endpointName, itemId, "Attachments", attachmentIdOrFileName);

            IConsumerRequest oauthRequest = _oauthSession.Request()
                .ForMethod("GET")
                .WithAcceptHeader(MimeTypes.Unknown)
                .ForUri(uri)
                .SignWithToken();

            var consumerResponse = oauthRequest.ToConsumerResponse();

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Stream;
            }

            throw new ApiResponseException(consumerResponse);
        }

        #endregion


        private IConsumerResponse CallApi(string method, string body, Uri baseUrl, string endpointName, string itemId, DateTime? lastModifiedDate, NameValueCollection additionalQueryParams, string acceptMimeType)
        {
            method = string.IsNullOrEmpty(method) ? "GET" : method.ToUpper();

            NameValueCollection allQueryParams = additionalQueryParams ?? new NameValueCollection();

            Uri uri = ConstructUri(baseUrl, endpointName, itemId, allQueryParams);

            IConsumerRequest request = _oauthSession.Request()
                .ForMethod(method)
                .ForUri(uri)
                .WithAcceptHeader(acceptMimeType ?? "text/xml")
                .WithIfModifiedSince(lastModifiedDate)
                .SignWithToken();

            if ((method == "PUT" || method == "POST"))
            {
                request = request.WithBody(body);                
            }

            if (method == "GET")
            {
                request = request.WithAcceptEncodingHeader("gzip");
            }

            IConsumerResponse consumerResponse = request.ToConsumerResponse();

            // Check for <ApiException> response message
            if (consumerResponse.Content.StartsWith("<ApiException") || consumerResponse.Content.Contains("ApiException"))
            {
                ApiExceptionDetails details = consumerResponse.Content.StartsWith("<ApiException") ?
                                                  new XmlModelSerializer().DeserializeTo<ApiExceptionDetails>(consumerResponse.Content) :
                                                  new JsonModelSerializer().DeserializeTo<ApiExceptionDetails>(consumerResponse.Content);

                throw new ApiException(details);
            }

            return consumerResponse;
        }


        /// <summary>
        /// Constructs the URI.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="additionalQueryParams">The additional query params.</param>
        /// <returns></returns>
        public static Uri ConstructUri(Uri baseUrl, string endpointName, string itemId, NameValueCollection additionalQueryParams)
        {
            var uriBuilder = new UriBuilder(baseUrl);

            if (!baseUrl.AbsoluteUri.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += endpointName;

            if (!string.IsNullOrEmpty(itemId))
            {
                uriBuilder.Path += ("/");
                uriBuilder.Path += (itemId);
            }

            string queryString = DevDefined.OAuth.Framework.UriUtility.FormatQueryString(additionalQueryParams);

            if (!string.IsNullOrEmpty(queryString))
                uriBuilder.Query = queryString;

            return uriBuilder.Uri;
        }


        /// <summary>
        /// Constructs the child resource URI.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="childResourceName">Name of the child resource.</param>
        /// <param name="childResourceId">The child resource id.</param>
        /// <returns></returns>
        public static Uri ConstructChildResourceUri(Uri baseUrl, string endpointName, string itemId, string childResourceName, string childResourceId)
        {
            if (string.IsNullOrEmpty(endpointName)) throw new ArgumentNullException("endpointName");
            if (string.IsNullOrEmpty(itemId)) throw new ArgumentNullException("itemId");
            if (string.IsNullOrEmpty(childResourceName)) throw new ArgumentNullException("childResourceName");

            var uriBuilder = new UriBuilder(baseUrl);

            if (!baseUrl.AbsoluteUri.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += endpointName;
            uriBuilder.Path += ("/");
            uriBuilder.Path += (itemId);
            uriBuilder.Path += ("/");
            uriBuilder.Path += (childResourceName);

            if (!string.IsNullOrEmpty(childResourceId))
            {
                uriBuilder.Path += ("/");
                uriBuilder.Path += (childResourceId);
            }

            return uriBuilder.Uri;
        }

        protected virtual string GetEndpointName(string elementName)
        {
            return ModelTypeHelper.Pluralize(elementName);
        }
    }
}
