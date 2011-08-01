using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;

using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PartnerApplicationRunner
    {
        private const string UserAgent = "Xero.API.ScreenCast v1.0 (Partner App Testing)";
        private const string ConsumerKey = "ZWVJMWFMNZJMYJG0NDRIY2IYZGIWMZ";

        private static readonly X509Certificate2 OAuthCertificate = new X509Certificate2(@"D:\Stevie-Cert.pfx", "xero");
        private static readonly X509Certificate2 ClientSslCertificate = new X509Certificate2(@"D:\EnTrust-D4-Mk2.p12", "xero");

        public static Repository CreateRepository()
        {
            IOAuthSession consumerSession = new XeroApiPartnerSession(
                UserAgent, 
                ConsumerKey,
                OAuthCertificate,               // OAuth signing certificate
                ClientSslCertificate);              // Client SSL Certificate

            consumerSession.MessageLogger = new DebugMessageLogger();

            // 1. Get a request token
            IToken requestToken = consumerSession.GetRequestToken();

            Console.WriteLine("Request Token Key: {0}", requestToken.Token);
            Console.WriteLine("Request Token Secret: {0}", requestToken.TokenSecret);


            // 2. Get the user to log into Xero using the request token in the querystring
            string authorisationUrl = consumerSession.GetUserAuthorizationUrlForToken(requestToken);
            Process.Start(authorisationUrl);

            // 3. Get the use to enter the authorisation code from Xero (4-7 digit number)
            Console.WriteLine("Please input the code you were given in Xero:");
            var verificationCode = Console.ReadLine();

            if (string.IsNullOrEmpty(verificationCode))
            {
                Console.WriteLine("You didn't type a verification code!");
                return null;
            }

            verificationCode = verificationCode.Trim();


            // 4. Use the request token and verification code to get an access token
            IToken accessToken;

            try
            {
                accessToken = consumerSession.ExchangeRequestTokenForAccessToken(requestToken, verificationCode);
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return null;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);

            
            // Wrap the authenticated consumerSession in the repository...
            return new Repository(consumerSession);
        }

    }
}
