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
        private readonly ApiQueryProvider _provider;
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
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public TModel FindById<TModel>(string id) 
            where TModel : ModelBase
        {
            var queryDescription = new ApiQueryDescription { ElementId = id, ElementType=typeof(TModel)  };
            string responseXml = _proxy.FindElements(queryDescription);

            return ModelSerializer.Deserialize<TModel>(responseXml, queryDescription.ElementListType).FirstOrDefault();
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


        public TModel FindOne<TModel>(string itemIdOrCode)
            where TModel : ModelBase
        {
            string responseXml = _proxy.GetElement(typeof(TModel).Name, itemIdOrCode);

            Type elementCollectionType = ModelTypeHelper.GetElementCollectionType(typeof(TModel));
            return ModelSerializer.Deserialize<TModel>(responseXml, elementCollectionType).FirstOrDefault();
        }


        public IQueryable<Invoice> Invoices
        {
            get { return new ApiQuery<Invoice>(_provider); }
        }

        public IQueryable<Contact> Contacts
        {
            get { return new ApiQuery<Contact>(_provider); }
        }

        public IQueryable<Organisation> Organisations
        {
            get { return new ApiQuery<Organisation>(_provider); }
        }

        public Organisation Organisation
        {
            get { return new ApiQuery<Organisation>(_provider).ToList().FirstOrDefault(); }
        }

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
            
            Response response = ModelSerializer.DeserializeResponse(responseXml);

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

            Response response = ModelSerializer.DeserializeResponse(responseXml);

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

            Response response = ModelSerializer.DeserializeResponse(responseXml);

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

            Response response = ModelSerializer.DeserializeResponse(responseXml);

            return response.GetTypedProperty<TModel>().First();
        }
    }

}
