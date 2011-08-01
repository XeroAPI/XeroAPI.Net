using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;
using DevDefined.OAuth.Utility;

using Xero.ScreencastWeb.Models;
using Xero.ScreencastWeb.Services.Interfaces;
using XeroApi.OAuth;

namespace Xero.ScreencastWeb.Services
{
    public class ApiRepository
    {
        static ApiRepository()
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            // This essentially instructs the ServicePointManager to ignore any certificate errors. Not ideal in a live environment.
            return true;
        }
        

        public IOAuthSession GetOAuthSession()
        {
            const string userAgent = "Xero.API.ScreenCastWeb v2.0";

            if (ConfigurationManager.AppSettings["XeroApiSignatureMethod"] == "HMAC-SHA1")
            {
                // Public App
                return new XeroApiPublicSession(
                    userAgent,
                    ConfigurationManager.AppSettings["XeroApiConsumerKey"],         // Consumer Key
                    ConfigurationManager.AppSettings["XeroApiConsumerSecret"]);     // Consumer Secret
            }
            
            if (ConfigurationManager.AppSettings["XeroApiSignatureMethod"] == "RSA-SHA1")
            {
                if (ConfigurationManager.AppSettings["XeroApiBaseUrl"].ToLower().IndexOf("partner") > 0)
                {
                    // Partner App
                    return new XeroApiPartnerSession(
                        userAgent,
                        ConfigurationManager.AppSettings["XeroApiConsumerKey"],         // Consumer Key
                        CertificateRepository.GetOAuthSigningCertificate(),             // OAuth Signing Certificate
                        CertificateRepository.GetClientSslCertificate());               // Client SSL Certificate
                }
                else
                {
                    // Private App
                    return new XeroApiPrivateSession(
                        userAgent,
                        ConfigurationManager.AppSettings["XeroApiConsumerKey"],         // Consumer Key
                        CertificateRepository.GetOAuthSigningCertificate());            // OAuth Signing Certificate
                }
            }

            throw new ConfigurationErrorsException("The configuration for a Public/Private/Partner app cannot be determined.");
        }


        /// <summary>
        /// Tests the connection to xero API.
        /// </summary>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <returns>
        /// Returns true if the Xero API succesfully returns a valid response
        /// </returns>
        /// <remarks>
        /// The simplest test is to call the 'GET Organisation' endpoint. This should always return the authenticated organisation details.
        /// </remarks>
        public bool TestConnectionToXeroApi(ITokenRepository<AccessToken> accessTokenRepository)
        {
            Trace.WriteLine("Entering ApiRepository.TestConnectionToXeroApi(..)");

            AccessToken accessToken = accessTokenRepository.GetToken("");

            if (accessToken == null)
            {
                return false;
            }
            
            IConsumerRequest consumerRequest = GetOAuthSession()
                .Request()
                .ForMethod("GET")
                .ForUri(GenerateFullEndpointUri(ConfigurationManager.AppSettings["XeroApiBaseUrl"], new ApiGetRequest<Organisation>()));

            try
            {
                Response response = CallXeroApiInternal(consumerRequest, accessTokenRepository);
                return (response != null && response.Organisations != null && response.Organisations.Count > 0);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// Renews the curent access token for a new one.
        /// </summary>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <returns>Returns a new access token</returns>
        /// <remarks>
        /// This method will only be successful if we're running as a partner application.
        /// </remarks>
        public AccessToken RenewAccessToken(ITokenRepository<AccessToken> accessTokenRepository)
        {
            Trace.WriteLine("Entering ApiRepository.RenewAccessToken(..)");

            AccessToken accessToken = accessTokenRepository.GetToken("");

            Trace.WriteLine("Old accessToken.Token:" + accessToken.Token);

            // Take the existing access token and replace for a new one
            AccessToken newAccessToken = GetOAuthSession().RenewAccessToken(
                accessToken,
                accessToken.SessionHandle);

            if (newAccessToken == null)
            {
                throw new ApplicationException("The access token could not be renewed for a new one.");
            }

            Trace.WriteLine("New accessToken.Token:" + accessToken.Token);

            // Write the new access token to session state
            accessTokenRepository.SaveToken(newAccessToken);

            return newAccessToken;
        }


        public Response GetItemByIdOrCode<TModel>(HttpSessionStateBase session, string resourceId)
            where TModel : ModelBase, new()
        {
            ITokenRepository<AccessToken> accessTokenRepository = new HttpSessionAccessTokenRepository(session);
            ApiGetRequest<TModel> listRequest = new ApiGetRequest<TModel> { ResourceId = resourceId };
            
            return Get(accessTokenRepository, listRequest);
        }


        /// <summary>
        /// Makes a GET request to the API
        /// </summary>
        /// <remarks>
        /// This method can GET-one or GET-many items
        /// </remarks>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <param name="getRequest">The get request.</param>
        /// <returns></returns>
        public Response Get<TModel>(ITokenRepository<AccessToken> accessTokenRepository, ApiGetRequest<TModel> getRequest)
            where TModel : ModelBase, new()
        {
            string xeroApiBaseUri = ConfigurationManager.AppSettings["XeroApiBaseUrl"];

            IConsumerRequest consumerRequest = GetOAuthSession()
                .Request()
                .ForMethod("GET")
                .ForUri(GenerateFullEndpointUri(xeroApiBaseUri, getRequest));
            
            // Set the If-Modified-Since http header - if specified
            getRequest.ApplyModifiedSinceDate(consumerRequest);

            return CallXeroApiInternal(consumerRequest, accessTokenRepository);
        }


        /// <summary>
        /// Makes a PUT request to the API
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <param name="putRequest">The put request.</param>
        /// <returns></returns>
        public Response Put<TModel>(ITokenRepository<AccessToken> accessTokenRepository, ApiPutRequest<TModel> putRequest)
            where TModel : ModelBase, new()
        {
            string xeroApiBaseUri = ConfigurationManager.AppSettings["XeroApiBaseUrl"];

            IConsumerRequest consumerRequest = GetOAuthSession()
                .Request()
                .ForMethod("PUT")
                .WithBody(putRequest.Payload.ToString())
                .ForUri(GenerateFullEndpointUri(xeroApiBaseUri, putRequest));

            // Set the If-Modified-Since http header - if specified
            putRequest.ApplyModifiedSinceDate(consumerRequest);

            return CallXeroApiInternal(consumerRequest, accessTokenRepository);
        }


        /// <summary>
        /// Makes a POST request to the API
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <param name="putRequest">The put request.</param>
        /// <returns></returns>
        public Response Post<TModel>(ITokenRepository<AccessToken> accessTokenRepository, ApiPutRequest<TModel> putRequest)
            where TModel : ModelBase, new()
        {
            string xeroApiBaseUri = ConfigurationManager.AppSettings["XeroApiBaseUrl"];

            IConsumerRequest consumerRequest = GetOAuthSession()
                .Request()
                .ForMethod("POST")
                .WithBody(putRequest.Payload.ToString())
                .ForUri(GenerateFullEndpointUri(xeroApiBaseUri, putRequest));

            // Set the If-Modified-Since http header - if specified
            putRequest.ApplyModifiedSinceDate(consumerRequest);

            return CallXeroApiInternal(consumerRequest, accessTokenRepository);
        }

        /// <summary>
        /// Calls the Xero API.
        /// </summary>
        /// <param name="consumerRequest">The consumer request.</param>
        /// <param name="accessTokenRepository">The access token repository.</param>
        /// <returns></returns>
        private Response CallXeroApiInternal(IConsumerRequest consumerRequest, ITokenRepository<AccessToken> accessTokenRepository)
        {
            AccessToken accessToken = accessTokenRepository.GetToken("");

            if (accessToken == null)
            {
                return new Response { Status = "NotConnected" };
            }

            if (accessToken.HasExpired())
            {
                accessToken = RenewAccessToken(accessTokenRepository);
            }

            if (accessToken.HasExpired())
            {
                return new Response { Status = "AccessTokenExpired" };
            }

            // At this point, we should have a valid a
            consumerRequest.SignWithToken(accessToken);

            HttpWebResponse webResponse;

            try
            {
                webResponse = consumerRequest.ToWebResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response;

                    if (httpWebResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        return new Response { Status = "NotFound" };
                    }
                }

                return new Response { Status = "Web Exception: " + ex.Message };
            }
            catch (OAuthException ex)
            {
                return new Response { Status = "OAuth Exception: " + ex.Report };
            }
            catch (Exception ex)
            {
                return new Response { Status = "Exception: " + ex.Message };
            }

            return ModelSerializer.DeSerializer<Response>(webResponse.GetResponseStream().ReadToEnd());
        }


        /// <summary>
        /// Gets the full endpoint URI.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="baseApiUri">The base API URI.</param>
        /// <param name="listRequest">The list request.</param>
        /// <returns></returns>
        private static Uri GenerateFullEndpointUri<TModel>(string baseApiUri, ApiGetRequest<TModel> listRequest)
            where TModel : ModelBase, new()
        {
            string endpointUri = baseApiUri;

            if (!endpointUri.EndsWith("/")) { endpointUri += "/"; }

            if (!string.IsNullOrEmpty(listRequest.ResourceName))
            {
                endpointUri += listRequest.ResourceName;
            }

            if (!endpointUri.EndsWith("/")) { endpointUri += "/"; }

            if (!string.IsNullOrEmpty(listRequest.ResourceId))
            {
                endpointUri += listRequest.ResourceId;
            }

            if (listRequest.RequiresQuerystring)
            {
                endpointUri += "?" + listRequest.ToQuerystring();
            }
            
            return new Uri(endpointUri);
        }

    }
}