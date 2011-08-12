using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            Organisation organisation = repository.Organisation;
            Console.WriteLine(string.Format("You have been authorised against organisation: {0}", organisation.Name));



            // Make a PUT call to the API - add a dummy contact
            Console.WriteLine("Please enter the name of a new contact to add to Xero");
            string contactName = Console.ReadLine();

            if (string.IsNullOrEmpty(contactName))
            {
                return;
            }
            
            Contact contact = new Contact { Name = contactName };
            
            contact = repository.UpdateOrCreate(contact);
            Console.WriteLine(string.Format("The contact '{0}' was created with id: {1}", contact.Name, contact.ContactID));

            


            // Try to update the contact that's just been created, but this time use a POST method
            contact.EmailAddress = string.Format("{0}@nowhere.com", contact.Name.ToLower().Replace(" ", "."));
            
            contact = repository.UpdateOrCreate(contact);
            Console.WriteLine(string.Format("The contact '{0}' was updated with email address: {1}", contact.Name, contact.EmailAddress));




            // Construct a linq expression to call 'GET Invoices'...
            int invoiceCount = repository.Contacts
                .Where(c => c.UpdatedDateUTC >= DateTime.UtcNow.AddMonths(-1))
                .Count();

            Console.WriteLine(string.Format("There were {0} contacts created or updated in the last month.", invoiceCount));
            



            // Find out how many bank accounts are defined for the organisation...
            IQueryable<Account> bankAccounts = repository.Accounts.Where(account => account.Type == "BANK");

            Console.WriteLine(string.Format("There were {0} bank accounts in this organisation.", bankAccounts.Count()));

            foreach (var bankAaccount in bankAccounts)
            {
                Console.WriteLine(string.Format("Bank Account Name:{0} Code:{1} Number:{2}", bankAaccount.Name, bankAaccount.Code, bankAaccount.BankAccountNumber));
            }




            // Get the tracking categories in this org
            IQueryable<TrackingCategory> trackingCategories = repository.TrackingCategories;

            foreach (var trackingCategory in trackingCategories)
            {
                Console.WriteLine(string.Format("Tracking Category: {0}", trackingCategory.Name));

                foreach (var trackingOption in trackingCategory.Options)
                {
                    Console.WriteLine(string.Format("    Option: {0}", trackingOption.Name));
                }
            }



            // Try the linq syntax to select items with sales details..
            var items = from item in repository.Items
                        where item.SalesDetails != null
                        select item;

            foreach (var item in items)
            {
                Console.WriteLine(string.Format("Item {0} is sold at price: {1} {2}", item.Description, item.SalesDetails.UnitPrice, organisation.BaseCurrency));
            }


            // Download a PDF of the first AR invoice in the system
            Invoice firstInvoice = repository.Invoices.First(invoice => invoice.Type == "ACCREC");
            
            if (firstInvoice != null)
            {
                byte[] invoicePdf = repository.FindById<Invoice>(firstInvoice.InvoiceID.ToString(), MimeTypes.ApplicationPdf);
                string invoicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), firstInvoice.InvoiceNumber + ".pdf");

                FileInfo file = new FileInfo(invoicePath);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (FileStream fs = file.OpenWrite())
                {
                    fs.Write(invoicePdf, 0, invoicePdf.Length);
                }

                Console.WriteLine("PDF for invoice '{0}' has been saved to:", firstInvoice.InvoiceNumber);
                Console.WriteLine(invoicePath);

                // This commented-out line of code will try and start a PDF viewer to view the invoice PDF.
                //Process.Start(invoicePath);
            }
            
        }
    }
}
