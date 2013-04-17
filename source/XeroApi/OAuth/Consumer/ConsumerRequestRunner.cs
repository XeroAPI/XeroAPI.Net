using System;
using System.Diagnostics;
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

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var httpWebResponse = webRequest.GetResponse() as HttpWebResponse;
                consumerResponse = new ConsumerResponse(httpWebResponse, GetElapsedTimespan(stopwatch));
            }
            catch (WebException webEx)
            {
                // I *think* it's safe to assume that the response will always be a HttpWebResponse...
                HttpWebResponse httpWebResponse = (HttpWebResponse)(webEx.Response);

                if (httpWebResponse == null)
                {
                    throw new ApplicationException("An HttpWebResponse could not be obtained from the WebException. Status was " + webEx.Status, webEx);
                }

                consumerResponse = new ConsumerResponse(httpWebResponse, webEx, GetElapsedTimespan(stopwatch));
            }

            return consumerResponse;
        }

        private TimeSpan GetElapsedTimespan(Stopwatch stopwatch)
        {
            if (stopwatch.IsRunning)
                stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }

    
}
