using System.Linq;

using DevDefined.OAuth.Consumer;

using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi
{
    public class CoreRepository : GenericRepository<Response>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="serializer">The serializer to be used</param>
        public CoreRepository(IIntegrationProxy integrationProxy, IModelSerializer serializer)
            : base(integrationProxy, serializer)
        {
        }

        public Organisation Organisation
        {
            get { return Organisations.FirstOrDefault(); }
        }

        public IQueryable<Organisation> Organisations
        {
            get { return new ApiQuery<Organisation>(Provider); }
        }

        public IQueryable<Invoice> Invoices
        {
            get { return new ApiQuery<Invoice>(Provider); }
        }

        public IQueryable<Contact> Contacts
        {
            get { return new ApiQuery<Contact>(Provider); }
        }

        public IQueryable<TaxRate> TaxRates
        {
            get { return new ApiQuery<TaxRate>(Provider); }
        }

        public IQueryable<Account> Accounts
        {
            get { return new ApiQuery<Account>(Provider); }
        }

        public IQueryable<TrackingCategory> TrackingCategories
        {
            get { return new ApiQuery<TrackingCategory>(Provider); }
        }

        public IQueryable<CreditNote> CreditNotes
        {
            get { return new ApiQuery<CreditNote>(Provider); }
        }

        public IQueryable<Currency> Currencies
        {
            get { return new ApiQuery<Currency>(Provider); }
        }

        public IQueryable<Payment> Payments
        {
            get { return new ApiQuery<Payment>(Provider); }
        }

        public IQueryable<ManualJournal> ManualJournals
        {
            get { return new ApiQuery<ManualJournal>(Provider); }
        }

        public IQueryable<BankTransaction> BankTransactions
        {
            get { return new ApiQuery<BankTransaction>(Provider); }
        }

        public IQueryable<Item> Items
        {
            get { return new ApiQuery<Item>(Provider); }
        }

        public IQueryable<BrandingTheme> BrandingThemes
        {
            get { return new ApiQuery<BrandingTheme>(Provider); }
        }

        public IQueryable<Journal> Journals
        {
            get { return new ApiQuery<Journal>(Provider); }
        }

        public IQueryable<Employee> Employees
        {
            get { return new ApiQuery<Employee>(Provider); }
        }

        public IQueryable<User> Users
        {
            get { return new ApiQuery<User>(Provider); }
        }

        public IQueryable<Receipt> Receipts
        {
            get { return new ApiQuery<Receipt>(Provider); }
        }

        public IQueryable<ExpenseClaim> ExpenseClaims
        {
            get { return new ApiQuery<ExpenseClaim>(Provider); }
        }

        public AttachmentRepository Attachments
        {
            get { return new AttachmentRepository(Proxy, Serializer); }
        }

        public ReportRepository Reports
        {
            get { return new ReportRepository(Proxy, Provider, Serializer); }
        }
    }
}
