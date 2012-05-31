using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Utility;

namespace DevDefined.OAuth.Consumer
{
    public interface IConsumerResponse
    {
        HttpStatusCode ResponseCode { get; }
        WebHeaderCollection Headers { get; }
        WebException WebException { get; }

        bool IsServerError { get; }
        bool IsClientError { get; }
        bool IsGoodResponse { get; }
        bool IsTokenExpiredResponse { get; }
        bool IsOAuthProblemResponse { get; }
        bool IsForbiddenResponse { get; }

        TimeSpan TimeTaken { get; }

        Stream Stream { get; }
        byte[] ByteArray { get; }
        string Content { get; }
        string ContentType { get; }
        int ContentLength { get; }
        
        OAuthProblemReport ToProblemReport();
        XDocument ToXDocument();
        T DeSerialiseTo<T>();
        NameValueCollection ToBodyParameters();
    }

    public class ConsumerResponse : IConsumerResponse
    {
        private readonly MemoryStream _responseContentStream = new MemoryStream();
        private readonly OAuthProblemReport _problemReport = null;
        private readonly WebException _webException = null;


        public ConsumerResponse(HttpWebResponse webResponse, WebException webException, TimeSpan timeTaken = default(TimeSpan))
            : this(webResponse, timeTaken)
        {
            _webException = webException;
        }

        public ConsumerResponse(HttpWebResponse webResponse, TimeSpan timeTaken = default(TimeSpan))
        {
            webResponse.GetResponseStream().CopyTo(_responseContentStream);

            if (webResponse.Headers["Content-Type"] != string.Empty)
                ContentType = webResponse.Headers["Content-Type"];

            if (webResponse.Headers["Content-Length"] != string.Empty)
                ContentLength = int.Parse(webResponse.Headers["Content-Length"]);

            TimeTaken = timeTaken;
            ContentEncoding = webResponse.ContentEncoding;
            ResponseCode = webResponse.StatusCode;
            Headers = webResponse.Headers;

            // Look for 'oauth_problem' in the message response for http 401 and 400 responses
            if (ResponseCode == HttpStatusCode.Unauthorized || ResponseCode == HttpStatusCode.BadRequest)
            {
                _problemReport = Content.Contains(Parameters.OAuth_Problem) ? new OAuthProblemReport(Content) : null;
            }
        }

        public Byte[] ByteArray
        {
            get
            {
                return _responseContentStream.ToArray();
            }
        }

        public TimeSpan TimeTaken
        {
            get; 
            private set;
        }

        public Stream Stream
        {
            get
            {
                return _responseContentStream;
            }
        }

        public string Content
        {
            get
            {
                return Stream.ReadToEnd();
            }
        }

        public string ContentEncoding
        {
            get; 
            private set;
        }

        public string ContentType
        {
            get; 
            private set;
        }

        public int ContentLength
        { 
            get; 
            private set;
        }

        public WebException WebException
        {
            get { return _webException; }
        }

        public XDocument ToXDocument()
        {
            return XDocument.Parse(Content);
        }

        public T DeSerialiseTo<T>()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(Stream))
            using (XmlReader xr = new XmlTextReader(sr))
            {
                return (T)serializer.Deserialize(xr);
            }
        }

        public NameValueCollection ToBodyParameters()
        {
            return UriUtility.ParseQueryString(Content);
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

        public bool IsOAuthProblemResponse
        {
            get { return _problemReport != null; }
        }

        public bool IsForbiddenResponse
        {
            get { return (int) ResponseCode == 403; }
        }

        public bool IsTokenExpiredResponse
        {
            get
            {
                return (_problemReport != null) 
                    && (string.Compare(_problemReport.Problem, OAuthProblems.TokenExpired, true) == 0)
                    && ( _problemReport.ProblemAdvice.Contains("expired"));
            }
        }
        
        public OAuthProblemReport ToProblemReport()
        {
            return _problemReport;
        }
    }
}
