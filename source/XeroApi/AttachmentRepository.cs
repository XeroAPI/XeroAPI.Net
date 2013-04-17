using System.IO;
using System.Linq;
using XeroApi.Integration;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi
{
    public class AttachmentRepository
    {
        private readonly IIntegrationProxy _integrationProxy;
        private readonly IModelSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="serializer"></param>
        internal AttachmentRepository(IIntegrationProxy integrationProxy, IModelSerializer serializer)
        {
            _integrationProxy = integrationProxy;
            _serializer = serializer;
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
            string data = _integrationProxy.UpdateOrCreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                attachment);

            var response = _serializer.DeserializeTo<Response>(data);

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
            string data = _integrationProxy.CreateAttachment(
                typeof(TModel).Name,
                ModelTypeHelper.GetModelItemId(model),
                attachment);

            return _serializer.DeserializeTo<Response>(data).Attachments.First();
        }


        // GET (one)

        public Attachment GetAttachmentFor<TModel>(TModel model)
            where TModel : ModelBase, IAttachmentParent
        {
            // List the attachments against this model.
            var modelItemId = ModelTypeHelper.GetModelItemId(model);

            var allAttachmentsXml = _integrationProxy.FindAttachments(typeof(TModel).Name, modelItemId);

            var allAttachments = _serializer.DeserializeTo<Response>(allAttachmentsXml).Attachments;

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
