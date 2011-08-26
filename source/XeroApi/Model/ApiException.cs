using System;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    [XmlRoot("ApiException")]
    public class ApiExceptionDetails
    {
        public int ErrorNumber { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public string Elements { get; set; }
    }

    public class ApiException : Exception
    {
        public ApiException(ApiExceptionDetails details) 
            : base(string.Concat("The XeroAPI returned an ApiException response: ", details.Message))
        {
            Details = details;
        }

        public ApiExceptionDetails Details
        {
            get; 
            set;
        }
    }
}
