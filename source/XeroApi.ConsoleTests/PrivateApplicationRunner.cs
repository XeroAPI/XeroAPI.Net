using System.Security.Cryptography.X509Certificates;

using Booyami.DevDefined.OAuth.Consumer;
using Booyami.DevDefined.OAuth.Logging;

using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PrivateApplicationRunner
    {
        private const string ConsumerKey = "NIG0NDLQSASBBGLIWUBSMUJLMTBZTQ";
        private const string UserAgentString = "Xero.API.ScreenCast v1.0 (Private App Testing)";
        
        
        public static Repository CreateRepository()
        {
            // Load the private certificate from disk using the password used to create it
            X509Certificate2 privateCertificate = new X509Certificate2(@"D:\Stevie-Cert.pfx", "xero");
            IOAuthSession consumerSession = new XeroApiPrivateSession(UserAgentString, ConsumerKey, privateCertificate);

            consumerSession.MessageLogger = new DebugMessageLogger();

            // Wrap the authenticated consumerSession in the repository...
            return new Repository(consumerSession);
        }
    }
}
