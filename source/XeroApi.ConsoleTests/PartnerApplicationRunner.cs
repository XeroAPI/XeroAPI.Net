using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PartnerApplicationRunner
    {
        private const string UserAgent = "Xero.API.ScreenCast v1.0 (Partner App Testing)";
        private const string ConsumerKey = "My-consumer-key";

        private static readonly X509Certificate2 OAuthCertificate = new X509Certificate2(@"C:\my-path-to\public_privatekey.pfx", "my password");
        private static readonly X509Certificate2 ClientSslCertificate = null;

        public static Repository CreateRepository()
        {
            IOAuthSession consumerSession = new XeroApiPartnerSession(
                UserAgent, 
                ConsumerKey,
                OAuthCertificate,               // OAuth signing certificate
                ClientSslCertificate,           // Client SSL Certificate
                new InMemoryTokenRepository()); 

            consumerSession.MessageLogger = new DebugMessageLogger();

            // 1. Get a request token
            IToken requestToken = consumerSession.GetRequestToken();

            Console.WriteLine("Request Token Key: {0}", requestToken.Token);
            Console.WriteLine("Request Token Secret: {0}", requestToken.TokenSecret);


            // 2. Get the user to log into Xero using the request token in the querystring
            string authorisationUrl = consumerSession.GetUserAuthorizationUrl();
            Process.Start(authorisationUrl);


            // 3. Get the use to enter the authorisation code from Xero (4-7 digit number)
            Console.WriteLine("Please input the code you were given in Xero:");
            var verificationCode = Console.ReadLine();

            if (string.IsNullOrEmpty(verificationCode))
            {
                Console.WriteLine("You didn't type a verification code!");
                return null;
            }

            
            // 4. Use the request token and verification code to get an access token
            AccessToken accessToken;

            try
            {
                accessToken = consumerSession.ExchangeRequestTokenForAccessToken(verificationCode.Trim());
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return null;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);
            Console.WriteLine("Access Token Lasts for: {0}", accessToken.TokenTimespan);
            Console.WriteLine("Session Lasts for: {0}", accessToken.SessionTimespan);



            // 5. Wait a secon and try and renew the access token..
            Console.WriteLine("Renewing the access token...");
            System.Threading.Thread.Sleep(1000);

            try
            {
                accessToken = consumerSession.RenewAccessToken();
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return null;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);
            Console.WriteLine("Access Token Lasts for: {0}", accessToken.TokenTimespan);
            Console.WriteLine("Session Lasts for: {0}", accessToken.SessionTimespan);

            
            // Wrap the authenticated consumerSession in the repository...
            return new Repository(consumerSession);
        }

    }
}
