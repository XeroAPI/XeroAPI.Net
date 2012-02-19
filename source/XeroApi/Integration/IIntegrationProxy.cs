using System.IO;
using XeroApi.Linq;
using XeroApi.Model;

namespace XeroApi.Integration
{
    public interface IIntegrationProxy
    {
        /// <summary>
        /// Gets a collection of elements
        /// </summary>
        /// <param name="apiQueryDescription">The API query description.</param>
        /// <returns></returns>
        string FindElements(IApiQueryDescription apiQueryDescription);

        string FindAttachments(string endpointName, string itemId);

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
        /// Finds one attachment.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="attachmentIdOrFileName">Name of the attachment id or file.</param>
        /// <returns></returns>
        Stream FindOneAttachment(string endpointName, string itemId, string attachmentIdOrFileName);

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
        /// Updates the or create attachment.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="attachment">The attachment.</param>
        /// <returns></returns>
        string UpdateOrCreateAttachment(string endpointName, string itemId, Attachment attachment);

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

        /// <summary>
        /// Creates the attachment.
        /// </summary>
        /// <param name="endpointName">Name of the endpoint.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="attachment">The attachment.</param>
        /// <returns></returns>
        string CreateAttachment(string endpointName, string itemId, Attachment attachment);
    }
}
