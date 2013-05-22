using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
                    //Generate a more helpful error message, hopefully this will save someone else hours in "the abyss"
                    if (webEx.Message.ToLower().Contains("could not create ssl/tls secure channel"))
                    {
                        //Find out what certificates were in the web request
                        var clientCerts = webRequest.ClientCertificates;
                        var certString = "";
                        foreach (var clientCert in clientCerts)
                        {
                            var thumbprint = "unknown";
                            var name = "unknown";

                            var x509Certificate2 = clientCert as X509Certificate2;
                            if (x509Certificate2 != null)
                            {
                                thumbprint = x509Certificate2.Thumbprint;
                                name = x509Certificate2.GetNameInfo(X509NameType.SimpleName, false);
                            }

                            certString += String.Format("[name: \"{0}\", serial: \"{1}\", thumbprint: \"{2}\"] ",
                                                        name,
                                                        clientCert.GetSerialNumberString(),
                                                        thumbprint);
                        }

                        //Try get the username that is running the current process (so we know who to give permissions to)
                        var processUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                        var userId = "your current process";
                        if (processUser != null)
                        {
                            userId = String.Format("\"{0}\"", processUser.Name);
                        }

                        throw new WebException(String.Format("{0} Check that {1} has permission to read the following certificates: {2}", webEx.Message, userId, certString));
                    }

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
