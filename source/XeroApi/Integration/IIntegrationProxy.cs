using XeroApi.Linq;

namespace XeroApi.Integration
{
    public interface IIntegrationProxy
    {
        /// <summary>
        /// Gets a collection of elements
        /// </summary>
        /// <param name="apiQueryDescription">The API query description.</param>
        /// <returns></returns>
        string FindElements(ApiQueryDescription apiQueryDescription);

        /// <summary>
        /// Gets one element using the specified MimeType.
        /// </summary>
        /// <remarks>
        /// This method is typically used for getting PDFs of invoices
        /// </remarks>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="acceptMimeType">the MimeType to request from the API server.</param>
        /// <returns></returns>
        byte[] FindOne(string endpointName, string itemId, string acceptMimeType);
        
        /// <summary>
        /// Updates the or creates the elements.
        /// </summary>
        /// <remarks>
        /// The equivilent of a POST http method
        /// </remarks>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        string UpdateOrCreateElements(string endpointName, string body);

        /// <summary>
        /// Creates the elements.
        /// </summary>
        /// <remarks>
        /// The equivilent of a PUT http method
        /// </remarks>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        string CreateElements(string endpointName, string body);
    }
}
