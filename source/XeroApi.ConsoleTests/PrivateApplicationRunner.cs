using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Security.Cryptography.X509Certificates;

using System.Xml.Linq;
using System.Xml.XPath;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Utility;
using XeroApi.Model;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PrivateApplicationRunner
    {
        private const string ConsumerKey = "ZWZMNWNMZME2NJMWNDDINMIXOWY0NT";
        private const string UserAgentString = "Xero.API.ScreenCast v1.0 (Private App Testing)";
        
        
        public static void Run()
        {
            // Load the private certificate from disk using the password used to create it
            X509Certificate2 privateCertificate = new X509Certificate2(@"D:\Stevie-Cert.pfx", "xero");
            var consumerSession = new XeroApiPrivateSession(UserAgentString, ConsumerKey, privateCertificate);



            // Wrap the authenticated consumerSession in the repository...
            Repository repository = new Repository(consumerSession);


            // Make a call to api.xero.com to check that we can use the access token.
            Organisation organisation = repository.Organisation;
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
