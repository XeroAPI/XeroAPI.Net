using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Model;

namespace XeroApi
{
    public interface IRepository
    {
        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        TModel FindById<TModel>(Guid id, bool single = false)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        TModel FindById<TModel>(string id, bool single = false)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Finds an item from the remote repository by Id
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="contentType">The Content-Type to request</param>
        /// <param name="single">Is this a single item not from a collection?</param>
        /// <returns></returns>
        byte[] FindById<TModel>(string id, string contentType, bool single = false)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Finds all items from the remote repository.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns></returns>
        IQueryable<TModel> FindAll<TModel>()
            where TModel : EndpointModelBase;

        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        IEnumerable<TModel> Create<TModel>(ICollection<TModel> itemsToCreate)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Creates the specified in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToCreate">The items to create.</param>
        /// <returns></returns>
        TModel Create<TModel>(TModel itemsToCreate)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemsToUpdate">The items to update.</param>
        /// <returns></returns>
        IEnumerable<TModel> UpdateOrCreate<TModel>(ICollection<TModel> itemsToUpdate)
            where TModel : EndpointModelBase;

        /// <summary>
        /// Updates the specified items in the remote repository
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="itemToUpdate">The item to update.</param>
        /// <returns></returns>
        TModel UpdateOrCreate<TModel>(TModel itemToUpdate)
            where TModel : EndpointModelBase;
    }
}
