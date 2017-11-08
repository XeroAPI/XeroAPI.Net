using System;
using System.Configuration;

using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Storage.Basic;

using XeroApi;
using XeroApi.OAuth;

namespace Xero.ScreencastWeb.Services
{
    public class ServiceProvider
    {
        [ThreadStatic]
        private static IOAuthSession _oauthSession;

        [ThreadStatic]
        private static ITokenRepository _tokenRepository;

        /// <summary>
        /// Gets the current instance of Repository (used for getting data from the Xero API)
        /// </summary>
        /// <returns></returns>
        public static Repository GetCurrentRepository()
        {
            var session = GetCurrentSession();

            return (session != null)
                ? new Repository(session) 
                : null;
        }

        /// <summary>
        /// Gets the current OAuthSession (used for getting request tokens, access tokens, etc.)
        /// </summary>
        /// <returns></returns>
        public static IOAuthSession GetCurrentSession()
        {
            return _oauthSession ?? (_oauthSession = CreateOAuthSession());
        }


        /// <summary>
        /// Creates the OAuth session.
        /// </summary>
        /// <returns></returns>
        private static IOAuthSession CreateOAuthSession()
        {
            const string userAgent = "Xero.API.ScreenCastWeb v2.0";

            if (ConfigurationManager.AppSettings["XeroApiSignatureMethod"] == "HMAC-SHA1")
            {
                // Public App
                return new XeroApiPublicSession(
                    userAgent,
                    ConfigurationManager.AppSettings["XeroApiConsumerKey"],             // Consumer Key
                    ConfigurationManager.AppSettings["XeroApiConsumerSecret"],          // Consumer Secret
                    CurrentTokenRepository);                                            // Token Repository
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
                        CertificateRepository.GetClientSslCertificate(),                // Client SSL Certificate
                        CurrentTokenRepository);                                        // Token Repository
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
        /// Gets or sets the current token repository.
        /// </summary>
        /// <value>The current token repository.</value>
        public static ITokenRepository CurrentTokenRepository
        {
            get { return _tokenRepository; }
            set { _tokenRepository = value; }
        }

    }
}