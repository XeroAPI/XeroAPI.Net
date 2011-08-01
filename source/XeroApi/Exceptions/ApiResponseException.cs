using System;
using DevDefined.OAuth.Consumer;

namespace XeroApi.Exceptions
{
    public class ApiResponseException : Exception
    {

        public ApiResponseException()
        {
            
        }

        public ApiResponseException(string message) : base(message)
        {
           
        }

        public ApiResponseException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public ApiResponseException(IConsumerResponse consumerResponse) : base(string.Format("Xero API returned error http code: {0}", consumerResponse.ResponseCode))
        {
            ConsumerResponse = consumerResponse;
        }

        public IConsumerResponse ConsumerResponse { get; set; }

    }
}
