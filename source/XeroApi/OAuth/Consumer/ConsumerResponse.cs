using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using DevDefined.OAuth.Utility;

namespace DevDefined.OAuth.Consumer
{
    public interface IConsumerResponse
    {
        HttpStatusCode ResponseCode { get; }
        WebHeaderCollection Headers { get; }

        bool IsServerError { get; }
        bool IsClientError { get; }
        bool IsGoodResponse { get; }

        string Content { get; }

        XDocument ToXDocument();
        T DeSerialiseTo<T>();
        NameValueCollection ToBodyParameters();
    }

    public class ConsumerResponse : IConsumerResponse
    {

        public ConsumerResponse(HttpWebResponse webResponse)
        {
            Content = webResponse.GetResponseStream().ReadToEnd();
            ResponseCode = webResponse.StatusCode;
            Headers = webResponse.Headers;
        }

        public string Content
        {
            get; 
            set;
        }

        public XDocument ToXDocument()
        {
            return XDocument.Parse(Content);
        }

        public T DeSerialiseTo<T>()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (TextReader tr = new StringReader(Content))
            using (XmlReader xr = new XmlTextReader(tr))
            {
                return (T)serializer.Deserialize(xr);
            }
        }

        public NameValueCollection ToBodyParameters()
        {
            return HttpUtility.ParseQueryString(Content);
        }

        public HttpStatusCode ResponseCode
        {
            get; 
            set;
        }

        public WebHeaderCollection Headers
        {
            get; 
            set;
        }

        public bool IsServerError
        {
            get { return (int) ResponseCode >= 500; }
        }

        public bool IsClientError
        {
            get { return (int) ResponseCode >= 400 && (int) ResponseCode <= 499; }
        }

        public bool IsGoodResponse
        {
            get { return (int)ResponseCode >= 200 && (int)ResponseCode <= 299; }
        }
    }
}
