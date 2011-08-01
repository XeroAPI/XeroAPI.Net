using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Xero.ScreencastWeb.Models
{
    public static class ModelSerializer
    {

        public static string Serialize<TModel>(TModel item)
        {
            StringBuilder sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TModel));
                xmlSerializer.Serialize(sw, item);
                sw.Flush();
            }

            return sb.ToString();
        }

        public static TModel DeSerializer<TModel>(string input)
        {
            using (TextReader textReader = new StringReader(input))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, new XmlReaderSettings()))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof (TModel));
                    return (TModel)xmlSerializer.Deserialize(xmlReader);
                }
            }
        }

    }
}
