using System;
using System.Linq;
using System.Net;

using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Utility;
using XeroApi.Model;

namespace XeroApi.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to run as a public or private application?");
            Console.WriteLine(" Press 1 for a public application");
            Console.WriteLine(" Press 2 for a private application");
            Console.WriteLine(" Press 3 for a partner application");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();

            if (keyInfo.KeyChar == '1')
            {
                Console.WriteLine("Running as a public application...");
                ExerciseOrganisation(PublicApplicationRunner.CreateRepository());
            }
            if (keyInfo.KeyChar == '2')
            {
                Console.WriteLine("Running as a private application...");
                ExerciseOrganisation(PrivateApplicationRunner.CreateRepository());
            }
            if (keyInfo.KeyChar == '3')
            {
                Console.WriteLine("Running as a partner application...");
                ExerciseOrganisation(PartnerApplicationRunner.CreateRepository());
            }

            Console.WriteLine("");
            Console.WriteLine(" Press Enter to Exit");
            Console.ReadLine();
        }

        static void ExerciseOrganisation(Repository repository)
        {
            if (repository == null)
            {
                return;
            }

            // Make a call to api.xero.com to check that we can use the access token.
            try
            {
                Organisation organisation = repository.Organisation;
                Console.WriteLine(string.Format("You have been authorised against organisation: {0}", organisation.Name));
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
                Console.WriteLine(string.Format("The contact '{0}' was created with id: {1}", contact.Name, contact.ContactID));
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




            // Try to update the contact that's just been created, but this time use a POST method
            contact.EmailAddress = string.Format("{0}@nowhere.com", contact.Name.ToLower().Replace(" ", "."));

            try
            {
                contact = repository.UpdateOrCreate(contact);
                Console.WriteLine(string.Format("The contact '{0}' was updated with email address: {1}", contact.Name, contact.EmailAddress));
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


            // Construct a linq expression to call 'GET Invoices'
            try
            {
                int invoiceCount = repository.Contacts
                    .Where(c => c.UpdatedDateUTC >= DateTime.UtcNow.AddMonths(-1))
                    .Count();

                Console.WriteLine(string.Format("There were {0} contacts created or updated in the last month.", invoiceCount));
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
        }
    }
}
