using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XeroApi.Model.Serialize
{
    public class XmlModelSerializer : IXmlModelSerializer
    {
        public T DeserializeTo<T>(string xml)
            where T : class
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(T));

            using (TextReader tr = new StringReader(xml))
            using (XmlReader xr = new XmlTextReader(tr))
            {
                return (T)serializer.Deserialize(xr);
            }
        }

        public string Serialize<TModel>(ICollection<TModel> itemsToSerialise)
            where TModel : ModelBase
        {
            return SerializeToXml(itemsToSerialise, new XmlSerializer(itemsToSerialise.GetType()));
        }

        public string Serialize<TModel>(TModel itemToSerialise)
            where TModel : ModelBase
        {
            return SerializeToXml(itemToSerialise, new XmlSerializer(typeof(TModel)));
        }

        public string MimeType { get { return "text/xml"; } }

        public static string SerializeToXml<TModel>(TModel itemToSerialise, XmlSerializer serialiser)
        {
            // Specify the namespaces to be used for the serializer - rather than using the default ones.
            var xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add(string.Empty, string.Empty);

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                serialiser.Serialize(sw, itemToSerialise, xmlnsEmpty);
                sw.Flush();
            }

            return CleanXml(sb.ToString());
        }

        public string Serialize2<TModel>(TModel itemsToSerialise)
            where TModel : ModelBase
        {
            var sb = new StringBuilder();

            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };

            using (var sw = new StringWriter(sb))
            using (var xs = XmlWriter.Create(sw, xmlWriterSettings))
            using (var navigator = new ModelTreeNavigator(xs))
            {
                navigator.Navigate(itemsToSerialise);

                xs.Flush();
                sw.Flush();
            }

            return sb.ToString();
        }

        private static string CleanXml(string xml)
        {
            XElement xElement = XElement.Parse(xml);

            xElement.Descendants().Where(el => el.Name == "ValidationErrors" || el.Name == "Warnings").Remove();
            xElement.Descendants().Where(el => el.Attributes().Any(attribute => attribute.Name.LocalName == "nil")).Remove();            
            xElement.Descendants().Where(el => el.Value == Guid.Empty.ToString()).Remove();

            return xElement.ToString();
        }
    }
}
