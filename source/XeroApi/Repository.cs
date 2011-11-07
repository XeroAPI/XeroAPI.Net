using System;
using System.Collections.Generic;
using System.Linq;

using DevDefined.OAuth.Consumer;

using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;

namespace XeroApi
{
    public class Repository
    {
        private readonly QueryProvider _provider;
        private readonly IIntegrationProxy _proxy;


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="oauthSession">The oauth session.</param>
        public Repository(IOAuthSession oauthSession)
        {
            _proxy = (new IntegrationProxy(oauthSession));
            _provider = new ApiQueryProvider(_proxy);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        public Repository(IIntegrationProxy integrationProxy)
        {
            _proxy = integrationProxy;
            _provider = new ApiQueryProvider(_proxy);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="provider">The query provider proxy.</param>
        public Repository(IIntegrationProxy integrationProxy, QueryProvider provider)
        {
            _proxy = integrationProxy;
            _provider = provider;
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public TModel FindById<TModel>(Guid id) 
            where TModel : ModelBase
        {
            return FindById<TModel>(id.ToString());
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public TModel FindById<TModel>(string id) 
            where TModel : ModelBase
        {
            var queryDescription = new LinqQueryDescription { ElementId = id, ElementType=typeof(TModel)  };
            string responseXml = _proxy.FindElements(queryDescription);

            Response response = ModelSerializer.DeserializeTo<Response>(responseXml);
            return response.GetTypedProperty<TModel>().FirstOrDefault();

            //return ModelSerializer.Deserialize<TModel>(responseXml, queryDescription.ElementListType).FirstOrDefault();
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="contentType">The Content-Type to request</param>
        /// <returns></returns>
        public byte[] FindById<TModel>(string id, string contentType)
            where TModel : ModelBase
        {
            Type collectionType = ModelTypeHelper.GetElementCollectionType(typeof(TModel));
            return _proxy.FindOne(collectionType.Name, id, contentType);
        }



        /// <summary>
        /// Finds all items from the remote repository.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IQueryable<TModel> FindAll<TModel>() 
            where TModel : ModelBase
        {
            return new ApiQuery<TModel>(_provider);
        }


        public Organisation Organisation { get { return Organisations.FirstOrDefault(); } }

        public IQueryable<Organisation> Organisations { get { return new ApiQuery<Organisation>(_provider); } }

        public IQueryable<Invoice> Invoices { get { return new ApiQuery<Invoice>(_provider); } }

        public IQueryable<Contact> Contacts { get { return new ApiQuery<Contact>(_provider); } }
        
        public IQueryable<TaxRate> TaxRates { get { return new ApiQuery<TaxRate>(_provider); } }

        public IQueryable<Account> Accounts { get { return new ApiQuery<Account>(_provider); } }

        public IQueryable<TrackingCategory> TrackingCategories { get { return new ApiQuery<TrackingCategory>(_provider); } }

        public IQueryable<CreditNote> CreditNotes { get { return new ApiQuery<CreditNote>(_provider); } }

        public IQueryable<Currency> Currencies { get { return new ApiQuery<Currency>(_provider); } }

        public IQueryable<Payment> Payments { get { return new ApiQuery<Payment>(_provider); } }

        public IQueryable<ManualJournal> ManualJournals { get { return new ApiQuery<ManualJournal>(_provider); } }

        public IQueryable<BankTransaction> BankTransactions { get { return new ApiQuery<BankTransaction>(_provider); } }
        
        public IQueryable<Item> Items { get { return new ApiQuery<Item>(_provider); } }

        public IQueryable<BrandingTheme> BrandingThemes { get { return new ApiQuery<BrandingTheme>(_provider); } }

        public IQueryable<Journal> Journals { get { return new ApiQuery<Journal>(_provider); } }
        
        public IQueryable<Employee> Employees { get { return new ApiQuery<Employee>(_provider); } }

        //public IQueryable<Report> Reports { get { return new ApiQuery<Report>(_provider); } }

        public IQueryable<User> Users { get { return new ApiQuery<User>(_provider); } }

        public IQueryable<Receipt> Receipts { get { return new ApiQuery<Receipt>(_provider); } }

        public IQueryable<ExpenseClaim> ExpenseClaims { get { return new ApiQuery<ExpenseClaim>(_provider); } }

        public AttachmentRepository Attachments { get { return new AttachmentRepository(_proxy); } }

        // In the pipeline...
        public ReportRepository Reports {get {return new ReportRepository(_proxy, _provider);}}

        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        public IEnumerable<TModel> Create<TModel>(ICollection<TModel> itemsToCreate) 
            where TModel : ModelBase
        {
            string requestXml = ModelSerializer.Serialize(itemsToCreate);

            string responseXml = _proxy.CreateElements(typeof(TModel).Name, requestXml);

            Response response = ModelSerializer.DeserializeTo<Response>(responseXml);

            return response.GetTypedProperty<TModel>();
        }


        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        public TModel Create<TModel>(TModel itemsToCreate)
            where TModel : ModelBase
        {
            string requestXml = ModelSerializer.Serialize(itemsToCreate);

            string responseXml = _proxy.CreateElements(typeof(TModel).Name, requestXml);

            Response response = ModelSerializer.DeserializeTo<Response>(responseXml);

            return response.GetTypedProperty<TModel>().First();
        }


        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToUpdate">The items to update.</param>
        /// <returns></returns>
        public IEnumerable<TModel> UpdateOrCreate<TModel>(ICollection<TModel> itemsToUpdate)
            where TModel : ModelBase
        {
            string requestXml = ModelSerializer.Serialize(itemsToUpdate);

            string responseXml = _proxy.UpdateOrCreateElements(typeof(TModel).Name, requestXml);

            Response response = ModelSerializer.DeserializeTo<Response>(responseXml);

            return response.GetTypedProperty<TModel>();
        }


        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemToUpdate">The item to update.</param>
        /// <returns></returns>
        public TModel UpdateOrCreate<TModel>(TModel itemToUpdate)
            where TModel : ModelBase
        {
            string requestXml = ModelSerializer.Serialize(itemToUpdate);

            string responseXml = _proxy.UpdateOrCreateElements(typeof(TModel).Name, requestXml);

            Response response = ModelSerializer.DeserializeTo<Response>(responseXml);

            return response.GetTypedProperty<TModel>().First();
        }
    }

}
