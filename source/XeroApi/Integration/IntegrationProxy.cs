using System;
using System.Collections.Specialized;

using DevDefined.OAuth.Consumer;
using XeroApi.Exceptions;
using XeroApi.Linq;
using XeroApi.Model;

namespace XeroApi.Integration
{
    public class IntegrationProxy : IIntegrationProxy
    {
        private readonly IOAuthSession _oauthSession;


        public IntegrationProxy (IOAuthSession oauthSession)
        {
            _oauthSession = oauthSession;
        }

        public string FindElements(ApiQueryDescription apiQueryDescription)
        {
            IConsumerResponse consumerResponse = CallApi(
                _oauthSession, 
                "GET", 
                string.Empty, 
                _oauthSession.ConsumerContext.BaseEndpointUri,
                ModelTypeHelper.Pluralize(apiQueryDescription.ElementName), 
                null,
                apiQueryDescription.Where,
                apiQueryDescription.Order,
                apiQueryDescription.ElementUpdatedDate,
                null);

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public string GetElement(string endpointName, string itemId)
        {
            IConsumerResponse consumerResponse = CallApi(
                _oauthSession, 
                "GET", 
                string.Empty, 
                _oauthSession.ConsumerContext.BaseEndpointUri,
                ModelTypeHelper.Pluralize(endpointName), 
                itemId, 
                null,
                null, 
                null,
                null);

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }

        public string UpdateOrCreateElements(string endpointName, string body)
        {
            IConsumerResponse consumerResponse = CallApi(
                _oauthSession,
                "POST",
                body,
                _oauthSession.ConsumerContext.BaseEndpointUri,
                ModelTypeHelper.Pluralize(endpointName),
                null,
                null,
                null,
                null,
                new NameValueCollection { { "summarizeErrors", "false" } });

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }


        public string CreateElements(string endpointName, string body)
        {
            IConsumerResponse consumerResponse = CallApi(
                _oauthSession,
                "PUT",
                body,
                _oauthSession.ConsumerContext.BaseEndpointUri,
                ModelTypeHelper.Pluralize(endpointName),
                null,
                null,
                null,
                null,
                new NameValueCollection { { "summarizeErrors", "false" } });

            if (consumerResponse.IsGoodResponse || consumerResponse.IsClientError)
            {
                return consumerResponse.Content;
            }

            throw new ApiResponseException(consumerResponse);
        }


        public static IConsumerResponse CallApi(IOAuthSession oauthSession, string method, string body, Uri baseUrl, string endpointName, string itemId, string whereClause, string orderBy, DateTime? lastModifiedDate, NameValueCollection additionalQueryParams)
        {
            method = string.IsNullOrEmpty(method) ? "GET" : method.ToUpper();

            Uri uri = ConstructUri(baseUrl, endpointName, itemId, whereClause, orderBy, additionalQueryParams);

            IConsumerRequest oauthRequest = oauthSession.Request()
                .ForMethod(method)
                .ForUri(uri)
                .WithAcceptHeader("text/xml")
                .SignWithToken();

            if ((method == "PUT" || method == "POST"))
            {
                oauthRequest = oauthRequest.WithBody(body);
            }

            if (lastModifiedDate.HasValue)
            {
                oauthRequest.Context.Headers.Add("If-Modified-Since", lastModifiedDate.Value.ToString("yyyy-MM-dd hh:mm:ss"));
            }

            return oauthRequest.ToConsumerResponse();
        }

        public static Uri ConstructUri(Uri baseUrl, string endpointName, string itemId, string whereClause, string orderBy, NameValueCollection additionalQueryParams)
        {
            UriBuilder uriBuilder = new UriBuilder(baseUrl);

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

            NameValueCollection queryStringParameters = additionalQueryParams ?? new NameValueCollection();

            if (!string.IsNullOrEmpty(whereClause))
                queryStringParameters.Add("WHERE", whereClause.Trim());

            if (!string.IsNullOrEmpty(orderBy))
                queryStringParameters.Add("ORDER", orderBy);

            string queryString = DevDefined.OAuth.Framework.UriUtility.FormatQueryString(queryStringParameters);

            if (!string.IsNullOrEmpty(queryString))
                uriBuilder.Query = queryString;
            
            return uriBuilder.Uri;
        }
        
    }
}
