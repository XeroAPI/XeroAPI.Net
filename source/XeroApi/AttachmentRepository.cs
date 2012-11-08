using System.IO;
using System.Linq;
using XeroApi.Integration;
using XeroApi.Model;

namespace XeroApi
{
    public class AttachmentRepository
    {
        private readonly IIntegrationProxy _integrationProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        internal AttachmentRepository(IIntegrationProxy integrationProxy)
        {
            _integrationProxy = integrationProxy;    
        }


        // POST

        public Attachment UpdateOrCreate<TModel>(TModel model, FileInfo fileInfo)
            where TModel : ModelBase, IAttachmentParent
        {
            string xml = _integrationProxy.UpdateOrCreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model), 
                new Attachment(fileInfo));

            Response response = ModelSerializer.DeserializeTo<Response>(xml);

            return response.Attachments.First();
        }


        // PUT

        public Attachment Create<TModel>(TModel model, FileInfo fileInfo)
            where TModel : ModelBase, IAttachmentParent
        {
            string xml = _integrationProxy.CreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                new Attachment(fileInfo));

            return ModelSerializer.DeserializeTo<Response>(xml).Attachments.First();
        }


        // GET (one)

        public Attachment GetAttachmentFor<TModel>(TModel model)
            where TModel : ModelBase, IAttachmentParent
        {
            // List the attachments against this model.
            string xml = _integrationProxy.FindAttachments(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model));

            Attachments attachments = ModelSerializer.DeserializeTo<Response>(xml).Attachments;

            if (attachments == null || attachments.Count == 0)
            {
                return null;
            }

            // Get the attachment content
            Stream content = _integrationProxy.FindOneAttachment(
                typeof (TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                attachments.First().AttachmentID.ToString());

            return attachments[0].WithContent(content);
        }
    }
}
