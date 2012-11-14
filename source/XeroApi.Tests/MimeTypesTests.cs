using System.IO;
using NUnit.Framework;

namespace XeroApi.Tests
{
    public class MimeTypesTests
    {
        [Test]
        public void it_can_return_the_mime_type_for_a_filename()
        {
            string mimeType = MimeTypes.GetMimeType("anyFilename.png");

            Assert.AreEqual("image/png", mimeType);
        }

        [Test]
        public void it_can_return_the_mime_type_for_a_filepath()
        {
            string mimeType = MimeTypes.GetMimeType(@"c:\any\dir\anyFilename.jpeg");

            Assert.AreEqual("image/jpeg", mimeType);
        }

        [Test]
        public void it_can_return_the_mime_type_for_a_fileinfo()
        {
            var mimeType = MimeTypes.GetMimeType(new FileInfo("anyFilename.txt"));

            Assert.AreEqual("text/plain", mimeType);
        }
    }
}
