using System;
using System.Diagnostics;
using System.Net;

using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Utility;

using XeroApi.Model;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PublicApplicationRunner
    {
        private const string UserAgent = "Xero.API.ScreenCast v1.0 (Public App Testing)";
        private const string ConsumerKey = "ZGIXM2M1Y2RIZJGYNGY1Y2EWZGYZMW";
        private const string ConsumerSecret = "RZRCMBRPK57EAG6GRO4GPLYDH9REPX";

        public static void Run()
        {
            var consumerSession = new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret);

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
                return;
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
                return;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);



            // Wrap the authenticated consumerSession in the repository...
            Repository repository = new Repository(consumerSession);


            // Make a call to api.xero.com to check that we can use the access token.
            var organisation = repository.Organisation;
            Console.WriteLine(string.Format("You have been authorised against organisation: {0}", organisation.Name));



            // Make a PUT call to the API - add a dummy contact
            Console.WriteLine("Please enter the name of a new contact to add to Xero");
            string contactName = Console.ReadLine();

            if (string.IsNullOrEmpty(contactName))
            {
                return;
            }
            

            Contact contact = new Contact
            { 
                Name = contactName
            };
            

            try
            {
                contact = repository.UpdateOrCreate(contact);
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return;
            }
            catch (WebException ex)
            {
                Console.WriteLine("A WebException was caught:");
                Console.WriteLine(ex.Response.GetResponseStream().ReadToEnd());
                return;
            }

            Console.WriteLine(string.Format("The contact '{0}' was created with id: {1}", contact.Name, contact.ContactID));



            // 7. Try to update the contact that's just been created, but this time use a POST method
            contact.EmailAddress = string.Format("{0}@nowhere.com", contact.Name.ToLower().Replace(" ", "."));
            

            try
            {
                contact = repository.UpdateOrCreate(contact);
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return;
            }
            catch (WebException ex)
            {
                Console.WriteLine("A WebException was caught:");
                Console.WriteLine(ex.Response.GetResponseStream().ReadToEnd());
                return;
            }

            Console.WriteLine(string.Format("The contact '{0}' was updated with email address: {1}", contact.Name, contact.EmailAddress));
        }

    }

}

