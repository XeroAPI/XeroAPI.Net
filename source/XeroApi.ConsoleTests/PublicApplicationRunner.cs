using System;
using System.Diagnostics;

using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Framework;
using Booyami.DevDefined.OAuth.Logging;
using Booyami.DevDefined.OAuth.Storage.Basic;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PublicApplicationRunner
    {
        private const string UserAgent = "Xero.API.ScreenCast v1.0 (Public App Testing)";
        private const string ConsumerKey = "ZGIXM2M1Y2RIZJGYNGY1Y2EWZGYZMW";
        private const string ConsumerSecret = "RZRCMBRPK57EAG6GRO4GPLYDH9REPX";

        public static Repository CreateRepository()
        {
            IOAuthSession consumerSession = new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret, new InMemoryTokenRepository());

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


            // Wrap the authenticated consumerSession in the repository...
            return new Repository(consumerSession);
        }
    }
}

