using System;
using System.Net;

namespace DevDefined.OAuth.Consumer
{

    public interface IConsumerRequestRunner
    {
        IConsumerResponse Run(IConsumerRequest consumerRequest);
    }

    public class DefaultConsumerRequestRunner : IConsumerRequestRunner
    {
        
        public IConsumerResponse Run(IConsumerRequest consumerRequest)
        {
            HttpWebRequest webRequest = consumerRequest.ToWebRequest();
            IConsumerResponse consumerResponse = null;

            try
            {
                consumerResponse = new ConsumerResponse(webRequest.GetResponse() as HttpWebResponse);
            }
            catch (WebException webEx)
            {
                // I *think* it's safe to assume that the response will always be a HttpWebResponse...
                HttpWebResponse httpWebResponse = (HttpWebResponse)(webEx.Response);

                if (httpWebResponse == null)
                {
                    throw new ApplicationException("An HttpWebResponse could not be obtained from the WebException. Status was " + webEx.Status, webEx);
                }

                consumerResponse = new ConsumerResponse(httpWebResponse, webEx);
            }

            return consumerResponse;
        }

        
    }
}
