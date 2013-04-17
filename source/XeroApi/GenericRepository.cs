using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi
{
    public abstract class GenericRepository<TResponse> : IRepository
        where TResponse : ResponseBase
    {
        private readonly IModelSerializer _serializer;
        private readonly XmlModelSerializer _xmlSerializer;
        private readonly QueryProvider _provider;
        private readonly IIntegrationProxy _proxy;

        protected GenericRepository(IIntegrationProxy integrationProxy, IModelSerializer serializer)
            : this (integrationProxy, new ApiQueryProvider<TResponse>(integrationProxy, serializer), serializer)
        {            
        }

        protected GenericRepository(IIntegrationProxy integrationProxy, QueryProvider provider, IModelSerializer serializer)
        {
            _proxy = integrationProxy;
            _provider = provider;
            _serializer = serializer;
            _xmlSerializer = new XmlModelSerializer();
        }

        public IModelSerializer Serializer
        {
            get { return _serializer; }
        }

        public XmlModelSerializer XmlSerializer
        {
            get { return _xmlSerializer; }
        }

        public QueryProvider Provider
        {
            get { return _provider; }
        }

        public IIntegrationProxy Proxy
        {
            get { return _proxy; }
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        public TModel FindById<TModel>(Guid id, bool single = false)
            where TModel : EndpointModelBase
        {
            return FindById<TModel>(id.ToString(), single);
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        public TModel FindById<TModel>(string id, bool single = false)
            where TModel : EndpointModelBase
        {
            var queryDescription = new LinqQueryDescription { ElementId = id, ElementType=typeof(TModel)  };
            string responseData = Proxy.FindElements(queryDescription);

            var response = Serializer.DeserializeTo<TResponse>(responseData);

            if (queryDescription.ElementListType == null || single)
            {
                return (TModel)response.GetSingleTypedProperty(queryDescription.ElementType);                    
            }

            return response.GetTypedProperty<TModel>().FirstOrDefault();
        }

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="contentType">The Content-Type to request</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        public byte[] FindById<TModel>(string id, string contentType, bool single = false)
            where TModel : EndpointModelBase
        {
            Type collectionType = ModelTypeHelper.GetElementCollectionType(typeof(TModel));
            return Proxy.FindOne(single ? typeof(TModel).Name : collectionType.Name, id, contentType);
        }

        /// <summary>
        /// Finds all items from the remote repository.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        public IQueryable<TModel> FindAll<TModel>()
            where TModel : EndpointModelBase
        {
            return new ApiQuery<TModel>(Provider);
        }

        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        public IEnumerable<TModel> Create<TModel>(ICollection<TModel> itemsToCreate)
            where TModel : EndpointModelBase
        {
            string requestData = XmlSerializer.Serialize(itemsToCreate);

            string responseData = Proxy.CreateElements(typeof(TModel).Name, requestData);

            var response = Serializer.DeserializeTo<TResponse>(responseData);

            return response.GetTypedProperty<TModel>();
        }


        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        public TModel Create<TModel>(TModel itemsToCreate)
            where TModel : EndpointModelBase
        {
            string requestData = XmlSerializer.Serialize(itemsToCreate);

            string responseData = Proxy.CreateElements(typeof(TModel).Name, requestData);

            var response = Serializer.DeserializeTo<TResponse>(responseData);

            return response.GetTypedProperty<TModel>().First();
        }


        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToUpdate">The items to update.</param>
        /// <returns></returns>
        public IEnumerable<TModel> UpdateOrCreate<TModel>(ICollection<TModel> itemsToUpdate)
            where TModel : EndpointModelBase
        {
            string request = XmlSerializer.Serialize(itemsToUpdate);

            string responseData = Proxy.UpdateOrCreateElements(typeof(TModel).Name, request);

            var response = Serializer.DeserializeTo<TResponse>(responseData);

            return response.GetTypedProperty<TModel>();
        }


        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemToUpdate">The item to update.</param>
        /// <returns></returns>
        public TModel UpdateOrCreate<TModel>(TModel itemToUpdate)
            where TModel : EndpointModelBase
        {
            string requestData = XmlSerializer.Serialize(itemToUpdate);

            string responseData = Proxy.UpdateOrCreateElements(typeof(TModel).Name, requestData);

            var response = Serializer.DeserializeTo<TResponse>(responseData);

            return response.GetTypedProperty<TModel>().First();
        }
    }
}
