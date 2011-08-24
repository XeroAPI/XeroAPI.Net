using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace XeroApi.Model
{
    internal class ModelSerializer
    {

        /// <summary>
        /// Deserializes the specified items.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        internal static string Deserialize<TModel>(ModelList<TModel> items)
            where TModel : ModelBase
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb)) 
            {

                using (XmlWriter xw = new XmlTextWriter(sw))
                {
                    var serializer = new System.Runtime.Serialization.DataContractSerializer(items.GetType());
                    serializer.WriteObject(xw, items);

                    xw.Flush();
                }

                sw.Flush();
            }

            return sb.ToString();
        }

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
                return (IModelList <TModel>)serializer.ReadObject(xr);
            }
        }

        internal static Response DeserializeResponse(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Response));

            using (TextReader tr = new StringReader(xml))
            using (XmlReader xr = new XmlTextReader(tr))
            {
                return (Response)serializer.Deserialize(xr);
            }
        }

        public static string Serialize<TModel>(ICollection<TModel> itemsToSerialise)
            where TModel : ModelBase
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(itemsToSerialise.GetType());

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, itemsToSerialise);
                sw.Flush();
            }

            return sb.ToString();
        }

        public static string Serialize<TModel>(TModel itemsToSerialise)
            where TModel : ModelBase
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TModel));

            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, itemsToSerialise);
                sw.Flush();
            }

            return sb.ToString();
        }
    }
}
