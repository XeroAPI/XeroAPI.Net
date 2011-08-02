using System;

namespace XeroApi.Model
{
    public class Attachment : ModelBase
    {
        public Guid? AttachmentID { get; set; }

        public string Filename { get; set; }

        public string MimeType { get; set; }

        public int? ContentLength { get; set; }

        public byte[] Content { get; set; }
    }


    public class Attachments : ModelList<Attachment>
    {
    }
}
