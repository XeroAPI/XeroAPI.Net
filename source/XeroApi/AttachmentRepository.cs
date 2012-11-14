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
            return UpdateOrCreate(model, new Attachment(fileInfo));
        }

        public Attachment UpdateOrCreate<TModel>(TModel model, Attachment attachment)
            where TModel : ModelBase, IAttachmentParent
        {
            string xml = _integrationProxy.UpdateOrCreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                attachment);

            Response response = ModelSerializer.DeserializeTo<Response>(xml);

            return response.Attachments.First();
        }


        // PUT

        public Attachment Create<TModel>(TModel model, FileInfo fileInfo)
            where TModel : ModelBase, IAttachmentParent
        {
            return Create(model, new Attachment(fileInfo));
        }

        public Attachment Create<TModel>(TModel model, Attachment attachment)
            where TModel : ModelBase, IAttachmentParent
        {
            string xml = _integrationProxy.CreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                attachment);

            return ModelSerializer.DeserializeTo<Response>(xml).Attachments.First();
        }


        // GET (one)

        public Attachment GetAttachmentFor<TModel>(TModel model)
            where TModel : ModelBase, IAttachmentParent
        {
            // List the attachments against this model.
            var modelItemId = ModelTypeHelper.GetModelItemId(model);

            var allAttachmentsXml = _integrationProxy.FindAttachments(typeof(TModel).Name, modelItemId);

            var allAttachments = ModelSerializer.DeserializeTo<Response>(allAttachmentsXml).Attachments;

            if (allAttachments == null || allAttachments.Count == 0)
            {
                return null;
            }

            var theFirstAttachment = allAttachments.First();

            // Get the attachment content
            var content = _integrationProxy.FindOneAttachment(
                typeof (TModel).Name,
                modelItemId,
                theFirstAttachment.AttachmentID.ToString());

            return theFirstAttachment.WithContent(content);
        }
    }
}
