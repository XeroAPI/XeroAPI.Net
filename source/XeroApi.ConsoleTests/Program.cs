using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevDefined.OAuth.Utility;
using XeroApi.Model;
using XeroApi.Model.Reporting;

namespace XeroApi.ConsoleApp
{
    class Program
    {
        private const string AnyAttachmentFilename = @".\Attachments\Receipt.png";
        private const string TestContactName = "Joe Bloggs (Test)";

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
        }

        static void ExerciseOrganisation(Repository repository)
        {
            if (repository == null)
            {
                return;
            }

            // Make a call to api.xero.com to check that we can use the access token.
            Organisation organisation = repository.Organisation;

            Console.WriteLine("You have been authorised against organisation: {0}", organisation.Name);

            TestGetAccountsByFilter(repository);
            TestCreatingAndUpdatingContacts(repository);
            TestGetContactsUsingLinq(repository);
            TestFindingCustumersThatAreContacts(repository);
            TestGetOrganisaitonUsingLinqSingle(repository);
            TestFindingBankAccounts(repository);
            TestFindingTrackingCategories(repository);
            TestFindingItemsUsingLinqSyntax(repository, organisation);
            TestCreatingInvoiceWithValidationErrors(repository);
            TestAttachmentFromByteArray(repository);
            TestAttachmentsAgainstPurchaseInvoice(repository);
            
            // Download a PDF of the first AR invoice in the system
            var anySalesInvoice = repository.Invoices.First(invoice => invoice.Type == "ACCREC");

            TestDownloadingPrintedInvoicePdf(repository, anySalesInvoice);
            TestFindingInvoicesByContactName(repository, anySalesInvoice.Contact);

            TestFindingTheSubscriberUser(repository);
            TestCreatingReceiptsForAUser(repository);
            TestGettingAListOfExpenseClaims(repository);
            TestGetAllJournalsWithPagination(repository);
            TestGettingATrialBalance(repository);
            TestGettingAListOfReports(repository);

            Console.WriteLine("All done!");
        }

        private static void TestAttachmentsAgainstPurchaseInvoice(Repository repository)
        {
            Console.WriteLine("Creating an invoice and loading attachment...");

            var anyPurchasesInvoice = repository.Invoices.FirstOrDefault(it => it.Type == "ACCPAY" && it.Status == "DRAFT")
                ?? CreateAnyPurchasesInvoice(repository);

            // Upload an attachment against the invoice
            var newAttachment = repository.Attachments.Create(anyPurchasesInvoice, new FileInfo(AnyAttachmentFilename));

            Console.WriteLine("Attachment {0} was added to invoice {1}", newAttachment.FileName, anyPurchasesInvoice.InvoiceID);
        }

        private static void TestAttachmentFromByteArray(Repository repository)
        {
            Console.WriteLine("Creating an invoice and loading attachment...");

            var anyPurchasesInvoice = CreateAnyPurchasesInvoice(repository);

            byte[] image;

            //create a byte array from the file and use that to upload
            using (var stream = new FileStream(AnyAttachmentFilename, FileMode.Open))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                image = ms.ToArray();
            }

            var info = new FileInfo(AnyAttachmentFilename);
            var mimeType = MimeTypes.GetMimeType(info);

            var newAttachment = repository.Attachments.Create(anyPurchasesInvoice, new Attachment
            {
                Content = image,
                ContentLength = image.Length,
                FileName = info.Name,
                MimeType = mimeType
            });

            Console.WriteLine("Attachment {0} was added to invoice {1}", newAttachment.FileName,
                              anyPurchasesInvoice.InvoiceID);

            Console.WriteLine("Finding invoice and saving attachment...");

            var invoice = repository.FindById<Invoice>(anyPurchasesInvoice.InvoiceID.ToString());
            Attachment attachment = repository.Attachments.GetAttachmentFor(invoice);            
            string attachmentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                              attachment.FileName);

            var file = new FileInfo(attachmentPath);

            if (file.Exists)
            {
                file.Delete();
            }

            using (FileStream fs = file.OpenWrite())
            {
                fs.Write(attachment.Content, 0, attachment.ContentLength);
            }

            Console.WriteLine("Attachment {0} was retrieved for invoice {1}", newAttachment.FileName,
                anyPurchasesInvoice.InvoiceID);
        }

        private static Invoice CreateAnyPurchasesInvoice(Repository repository)
        {
            var invoice = new Invoice
                {
                    Type = "ACCPAY",
                    Contact = new Contact {Name = TestContactName},
                    Date = DateTime.Today,
                    DueDate = DateTime.Today.AddDays(14),
                    Status = "DRAFT",
                    LineItems = new LineItems
                        {
                            new LineItem
                                {
                                    Description = "Services Rendered",
                                    Quantity = 1,
                                    UnitAmount = 1,
                                }
                        }
                };

            return repository.Create(invoice);
        }

        private static void TestGettingATrialBalance(Repository repository)
        {
            // Get a trial balance report (as per http://answers.xero.com/developer/question/36201/)
            Console.WriteLine("Running Trial Balance Report...");
            Report trialBalance = repository.Reports.RunDynamicReport(new TrialBalanceReport());

            if (trialBalance != null)
            {
                foreach (var reportTitle in trialBalance.ReportTitles)
                {
                    Console.WriteLine("\t" + reportTitle);
                }

                foreach (var reportRow in trialBalance.Rows)
                {
                    Console.WriteLine("    " + reportRow.Title);

                    if (reportRow.Rows != null)
                    {
                        foreach (var subRow in reportRow.Rows)
                        {
                            Console.Write("         Row: " + subRow.RowType);

                            foreach (var cell in subRow.Cells)
                            {
                                Console.Write(cell.Value + ", ");
                            }

                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        private static void TestGettingAListOfExpenseClaims(Repository repository)
        {
            // Get a list of all expense claims
            Console.WriteLine("Getting a list of all submitted expense claims...");

            foreach (var expenseClaim in repository.ExpenseClaims.Where(expenseClaim => expenseClaim.Status != "CURRENT"))
            {
                Console.WriteLine("Expense claim {0} for user {1} for amount {2} with status {3}", expenseClaim.ExpenseClaimID,
                                  expenseClaim.User.FullName, expenseClaim.Total, expenseClaim.Status);
            }
        }

        private static void TestCreatingReceiptsForAUser(Repository repository)
        {
            User[] allUsers = repository.Users.ToArray();

            var anyUserWithAnId = allUsers.FirstOrDefault(it => it.UserID != null);
            
            if (anyUserWithAnId == null)
            {
                Console.WriteLine("Again, this is probably a demo organisation. There are no UserIDs returned for these organisations.");
                return;
            }

            // Create a receipt
            Receipt receipt = new Receipt
                {
                    Contact = new Contact {Name = "Mojo Coffee"},
                    User = anyUserWithAnId,
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

            if (receipt.ReceiptID == Guid.Empty)
            {
                string message = string.Format("The receipt was not succesfully created: {0}", receipt.ValidationErrors.Select(it => it.Message).Aggregate((s1, s2) => string.Concat(s1, ";", s2)));
                throw new ApplicationException(message);
            }

            Console.WriteLine("Receipt {0} was created for {1} for user {2}", receipt.ReceiptID, receipt.Contact.Name, receipt.User.FullName);


            // Upload an attachment against the newly creacted receipt
            FileInfo attachmentFileInfo = new FileInfo(AnyAttachmentFilename);

            if (!attachmentFileInfo.Exists)
            {
                throw new ApplicationException("The Receipt.png file cannot be loaded from disk!");
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
            else if (attachment.FileName != attachmentFileInfo.Name)
            {
                Console.WriteLine("The uploaded attachment filename '{0}' does not match the original filename '{1}'", attachment.FileName, attachmentFileInfo.Name);
            }
            else
            {
                Console.WriteLine("Attachment succesfully uploaded!");
            }
        }

        private static void TestFindingTheSubscriberUser(Repository repository)
        {
            // Find the subscriber for this organisation
            User subscriber = repository.Users.FirstOrDefault(user => user.IsSubscriber);

            string message = (subscriber == null)
                ? "There is no subscriber for this organisation. Maybe this is a demo organisation?"
                : "The subscriber for this organisation is " + subscriber.FullName;

            Console.WriteLine(message);
        }

        private static void TestDownloadingPrintedInvoicePdf(Repository repository, Invoice anySalesInvoice)
        {
            // Test the FindById to see if we can re-fetch the invoice WITH the line items this time
            //anySalesInvoice = repository.FindById<Invoice>(anySalesInvoice.InvoiceID);


            Console.WriteLine("Downloading the PDF of invoice {0}...", anySalesInvoice.InvoiceNumber);

            byte[] invoicePdf = repository.FindById<Invoice>(anySalesInvoice.InvoiceID.ToString(), MimeTypes.ApplicationPdf);
            string invoicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                              anySalesInvoice.InvoiceNumber + ".pdf");

            FileInfo file = new FileInfo(invoicePath);

            if (file.Exists)
            {
                file.Delete();
            }

            using (FileStream fs = file.OpenWrite())
            {
                fs.Write(invoicePdf, 0, invoicePdf.Length);
            }

            Console.WriteLine("PDF for invoice '{0}' has been saved to:", anySalesInvoice.InvoiceNumber);
            Console.WriteLine(invoicePath);
        }

        private static void TestFindingInvoicesByContactName(Repository repository, Contact contact)
        {
            // Find all invoices that were against the same contact as the first AR invoice that we've just found (http://answers.xero.com/developer/question/36911/)
            Console.WriteLine("Getting a list of all invoice created for {0}", contact.Name);

            //Guid contactId = anySalesInvoice.Contact.ContactID;
            var invoicesForContact = repository.Invoices.Where(invoice => invoice.Contact.ContactID == contact.ContactID).ToList();

            Console.WriteLine("There are {0} invoices raised for {1}", invoicesForContact.Count, contact.Name);

            foreach (var invoiceForContact in invoicesForContact)
            {
                Console.WriteLine("Invoice {0} was raised against {1} on {2} for {3}{4}", invoiceForContact.InvoiceNumber,
                                  invoiceForContact.Contact.Name, invoiceForContact.Date, invoiceForContact.Total,
                                  invoiceForContact.CurrencyCode);
            }
        }

        private static void TestCreatingInvoiceWithValidationErrors(Repository repository)
        {
            // Try and create an invoice - but using incorrect data. This should hopefully be rejected by the Xero API
            Invoice invoiceToCreate = new Invoice
                {
                    Contact = new Contact
                        {
                            Name = TestContactName
                        },
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
        }

        private static void TestFindingItemsUsingLinqSyntax(Repository repository, Organisation organisation)
        {
            // Try the linq syntax to select items with sales details..
            var itemQuery = from item in repository.Items
                            where item.SalesDetails != null
                            select item;

            var itemList = itemQuery.ToList();

            Console.WriteLine("There are {0} inventory items", itemList.Count);

            foreach (var item in itemList)
            {
                Console.WriteLine(string.Format("   Item {0} is sold at price: {1} {2}", item.Description, item.SalesDetails.UnitPrice, organisation.BaseCurrency));
            }
        }

        private static void TestFindingTrackingCategories(Repository repository)
        {
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
        }

        private static void TestFindingBankAccounts(Repository repository)
        {
            // Find out how many bank accounts are defined for the organisation...
            var bankAccounts = repository.Accounts
                .Where(account => account.Type == "BANK")
                .OrderBy(account => account.Name)
                .ToList();

            Console.WriteLine(string.Format("There were {0} bank accounts in this organisation.", bankAccounts.Count()));

            foreach (var bankAaccount in bankAccounts)
            {
                Console.WriteLine(string.Format("Bank Account Name:{0} Code:{1} Number:{2}", bankAaccount.Name,
                                                bankAaccount.Code, bankAaccount.BankAccountNumber));
            }
        }

        private static void TestFindingCustumersThatAreContacts(Repository repository)
        {
            // Construct a linq expression to call 'GET Contacts'...
            var customers = repository.Contacts.Where(c => c.IsCustomer == true).ToList();

            Console.WriteLine(string.Format("There are {0} contacts that are customers.", customers.Count));

            if (customers.Any(c => !c.IsCustomer))
            {
                Console.WriteLine("Filtering contacts on the IsCustomer flag didn't work!");
            }
        }

        private static void TestGetContactsUsingLinq(Repository repository)
        {
            // Construct a linq expression to call 'GET Contacts'...
            int invoiceCount = repository.Contacts.Count(c => c.UpdatedDateUTC >= DateTime.UtcNow.AddMonths(-1));

            Console.WriteLine(string.Format("There were {0} contacts created or updated in the last month.", invoiceCount));
        }

        private static void TestCreatingAndUpdatingContacts(Repository repository)
        {
            // Make a PUT call to the API - add a dummy contact
            var contact = new Contact { Name = TestContactName };

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
        }

        private static void TestGetAllJournalsWithPagination(Repository repository)
        {
            // Get all journals from the general ledger using the ?offset=xxx parameter
            List<Journal> allJournals = new List<Journal>();
            List<Journal> batchOfJournals;
            int skip = 0;

            while ((batchOfJournals = repository.Journals.Skip(skip).ToList()).Count > 0)
            {
                Console.WriteLine("Fetched {0} journals from API using skip={1}", batchOfJournals.Count, skip);

                allJournals.AddRange(batchOfJournals);
                skip += batchOfJournals.Count;
            }

            if (allJournals.Any())
            {
                Console.WriteLine(
                    "There are {0} journals in the general ledger, starting with #{1} and ending with #{2}",
                    allJournals.Count, allJournals.First().JournalNumber, allJournals.Last().JournalNumber);
            }
            else
            {
                Console.WriteLine("There are no journals in the general ledger");
            }
        }

        private static void TestGetAccountsByFilter(Repository repository)
        {
            // API v2.15 Get a list of accounts that can be used when creating expense claims
            var expenseClaimAccounts = repository.Accounts.Where(a => a.ShowInExpenseClaims == true).ToList();

            Console.WriteLine("There are {0} accounts that can be used in expense claim line items", expenseClaimAccounts.Count);

            foreach (var account in expenseClaimAccounts)
            {
                Console.WriteLine("Expense Account, name:{0} code:{1}", account.Name, account.Code);
            }
        }

        private static void TestGetOrganisaitonUsingLinqSingle(Repository repository)
        {
            // Try out the 'Single' linq method (http://answers.xero.com/developer/question/36501/)
            var organisation = repository.Organisations.Single();

            Console.WriteLine("The organisation is still called: " + organisation.Name);
        }

        private static void TestGettingAListOfReports(Repository repository)
        {
            // Hint: Only NZ GST Reports and AU BAS Reports are exposed via the 'GET Reports' method.
            var allPublishedReports = repository.Reports.ListAllPublishedReports.ToArray();

            foreach (var report in allPublishedReports)
            {
                Console.WriteLine("Found published report, name:{0}, type:{1}", report.ReportName, report.ReportType);
            }
        }
    }
}
