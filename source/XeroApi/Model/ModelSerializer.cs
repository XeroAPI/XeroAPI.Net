using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    public class ModelSerializer
    {
        [Obsolete("This uses an incorrect serializer implemetation", true)]
        internal static IModelList<TModel> Deserialize<TModel>(string xml, Type modelListType)
            where TModel : ModelBase
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var serializer = new System.Runtime.Serialization.DataContractSerializer(modelListType);

            using (TextReader tr = new StringReader(xml))
            using (XmlReader xr = new XmlTextReader(tr))
            {
                return (IModelList<TModel>)serializer.ReadObject(xr);
            }
        }

        internal static T DeserializeTo<T>(string xml)
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

        public static string Serialize<TModel>(ICollection<TModel> itemsToSerialise)
            where TModel : ModelBase
        {
            // Specify the namespaces to be used for the serializer - rather than using the default ones.
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");

            var serializer = new XmlSerializer(itemsToSerialise.GetType());

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, itemsToSerialise);
                sw.Flush();
            }

            return CleanXml(sb.ToString());
        }

        public static string Serialize<TModel>(TModel itemToSerialise)
            where TModel : ModelBase
        {
            // Specify the namespaces to be used for the serializer - rather than using the default ones.
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", "");

            var serializer = new XmlSerializer(typeof(TModel));

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, itemToSerialise, xmlnsEmpty);
                sw.Flush();
            }

            return CleanXml(sb.ToString());
        }

        public static string Serialize2<TModel>(TModel itemsToSerialise)
            where TModel : ModelBase
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };

            using (StringWriter sw = new StringWriter(sb))
            using (XmlWriter xs = XmlWriter.Create(sw, xmlWriterSettings))
            using (ModelTreeNavigator navigator = new ModelTreeNavigator(xs))
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

            return xElement.ToString();
        }
    }
}

