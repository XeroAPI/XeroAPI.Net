using System;
using System.IO;
using System.Linq;

using XeroApi.Model;
using XeroApi.Model.Reporting;

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



            // Get a trial balance report (as per http://answers.xero.com/developer/question/36201/)
            //TrialBalanceReport trialBalanceReport = repository.Reports.RunDynamicReport(new TrialBalanceReport());



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
            Console.WriteLine(string.Format("The validation status was: {0}", contact.ValidationStatus));

            


            // Try to update the contact that's just been created, but this time use a POST method
            contact.EmailAddress = string.Format("{0}@nowhere.com", contact.Name.ToLower().Replace(" ", "."));
            
            contact = repository.UpdateOrCreate(contact);
            Console.WriteLine(string.Format("The contact '{0}' was updated with email address: {1}", contact.Name, contact.EmailAddress));

            

            // Get the contact by it's Id...
            var reReadContact = repository.Contacts.First(c => c.ContactID == contact.ContactID);
            Console.WriteLine(string.Format("The contact '{0}' was re-read using it's ContactID: {1}", reReadContact.Name, reReadContact.ContactID));



            // Construct a linq expression to call 'GET Contacts'...
            int invoiceCount = repository.Contacts
                .Where(c => c.UpdatedDateUTC >= DateTime.UtcNow.AddMonths(-1))
                .Count();

            Console.WriteLine(string.Format("There were {0} contacts created or updated in the last month.", invoiceCount));
            



            // Find out how many bank accounts are defined for the organisation...
            var bankAccounts = repository.Accounts
                .Where(account => account.Type == "BANK")
                .OrderBy(account => account.Name)
                .ToList();

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


            // Try and create an invoice - but using incorrect data. This should hopefully be rejected by the Xero API
            Invoice invoiceToCreate = new Invoice
            {
                Contact = contact,
                Type = "ACCREC",
                Date = DateTime.Today,
                LineItems = new LineItems
                {
                    new LineItem
                    {
                        AccountCode = "200",
                        Description = "Blue Widget",
                        UnitAmount = 10m,
                        TaxAmount = 2m,
                        LineAmount = 12m
                    }
                }
            };

            Console.WriteLine("Creating an invoice that should cause a validation error...");
            var createdInvoice = repository.Create(invoiceToCreate);

            if (createdInvoice.ValidationStatus == ValidationStatus.ERROR)
            {
                foreach (var message in createdInvoice.ValidationErrors)
                {
                    Console.WriteLine("Validation Error: " + message.Message);
                }
            } 


            // Download a PDF of the first AR invoice in the system
            Invoice firstInvoice = repository.Invoices.First(invoice => invoice.Type == "ACCREC");
            


            // Test the FindById to see if we can re-fetch the invoice WITH the line items this time
            firstInvoice = repository.FindById<Invoice>(firstInvoice.InvoiceID);



            if (firstInvoice != null)
            {
                Console.WriteLine(string.Format("Downloading the PDF of invoice {0}...", firstInvoice.InvoiceNumber));

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


            
            // Find all invoices that were against the same contact as the first AR invoice that we have
            if (firstInvoice != null)
            {
                try
                {
                    Console.WriteLine("Getting a list of all invoice created for {0}", firstInvoice.Contact.Name);

                    Guid contactId = firstInvoice.Contact.ContactID;
                    var invoicesForContact = repository.Invoices.Where(invoice => invoice.Contact.ContactID == contactId).ToList();

                    Console.WriteLine("There are {0} invoices raised for {1}", invoicesForContact.Count, firstInvoice.Contact.Name);

                    foreach (var invoiceForContact in invoicesForContact)
                    {
                        Console.WriteLine("Invoice {0} was raised against {1} on {2} for {3}{4}", invoiceForContact.InvoiceNumber,
                                          invoiceForContact.Contact.Name, invoiceForContact.Date, invoiceForContact.Total,
                                          invoiceForContact.CurrencyCode);
                    }
                }
                catch (ApiException ex)
                {
                    Console.WriteLine("Filtering on Guid types doesn't yet work.");
                    Console.WriteLine(ex.Message);
                }
            }



            // Find the subscriber for this organisation
            User subscriber = repository.Users.FirstOrDefault(user => user.IsSubscriber == true);

            if (subscriber == null)
            {
                Console.WriteLine("There is no subscriber for this organisation. Maybe a demo organisation? Maybe this endpoint hasn't been released yet?");
            }
            else
            {
                Console.WriteLine("The subscriber for this organisation is " + subscriber.FullName);


                // Create a receipt
                Receipt receipt = new Receipt
                {
                    Contact = new Contact { Name = "Mojo Coffee" },
                    User = subscriber,
                    Date = DateTime.Today.Date,
                    LineAmountTypes = LineAmountType.Inclusive,
                    LineItems = new LineItems
                    {
                        new LineItem
                            {
                                Description = "Flat White",
                                Quantity = 1m,
                                AccountCode = "429",
                                UnitAmount = 3.8m
                            },
                                           
                        new LineItem
                            {
                                Description = "Mocha",
                                Quantity = 1m,
                                AccountCode = "429",
                                UnitAmount = 4.2m
                            }
                    }
                };


                // Save the receipt to Xero
                receipt = repository.Create(receipt);

                Console.WriteLine("Receipt {0} was created for {1} for user {2}", receipt.ReceiptID, receipt.Contact.Name, receipt.User.FullName);



                // Upload an attachment against the newly creacted receipt
                FileInfo attachmentFileInfo = new FileInfo(@".\Attachments\Receipt.png");

                if (!attachmentFileInfo.Exists)
                {
                    Console.WriteLine("The Receipt.png file cannot be loaded from disk!" + subscriber.FullName);
                    return;
                }


                // Upload the attachment against the receipt
                Console.WriteLine("Attaching file {0} to Receipt {1}...", attachmentFileInfo.Name, receipt.ReceiptID);
                repository.Attachments.UpdateOrCreate(receipt, attachmentFileInfo);


                // Fetch the attachment that was just uploaded
                Attachment attachment = repository.Attachments.GetAttachmentFor(receipt);

                if (attachment.ContentLength != attachmentFileInfo.Length)
                {
                    Console.WriteLine("The uploaded attachment filesize {0} does not match the original filesize {1}", attachment.ContentLength, attachmentFileInfo.Length);
                }
                else if (attachment.Filename != attachmentFileInfo.Name)
                {
                    Console.WriteLine("The uploaded attachment filename '{0}' does not match the original filename '{1}'", attachment.Filename, attachmentFileInfo.Name);
                }
                else
                {
                    Console.WriteLine("Attachment succesfully uploaded!");
                }

            }


            // Get a list of all expense claims
            Console.WriteLine("Getting a list of all submitted expense claims...");

            foreach (var expenseClaim in repository.ExpenseClaims.Where(expenseClaim => expenseClaim.Status != "CURRENT"))
            {
                Console.WriteLine("Expense claim {0} for user {1} for amount {2} with status {3}", expenseClaim.ExpenseClaimID, expenseClaim.User.EmailAddress, expenseClaim.Total, expenseClaim.Status);
            }

            
            Console.WriteLine("All done!");
        }
    }
}
