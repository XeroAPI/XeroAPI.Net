using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Logging;
using XeroApi.Model.Serialize;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PrivateApplicationRunner : ApiApplicationRunner, IAuthenticate
    {
        private const string ConsumerKey = "NIG0NDLQSASBBGLIWUBSMUJLMTBZTQ";
        private const string UserAgentString = "Xero.API.Console.CS (Private App Testing)";

        private static readonly IModelSerializer _serializer = new XmlModelSerializer();

        public static CoreRepository CreateRepository()
        {
            return CreateRepository(new PrivateApplicationRunner(), _serializer);
        }

        public static PayrollRepository CreatePayrollRepository()
        {
            return CreatePayrollRepository(new PrivateApplicationRunner(), _serializer);
        }

        public IOAuthSession Authenticate()
        {
            // Load the private certificate from disk using the password used to create it
            var privateCertificate = new X509Certificate2(@"D:\Stevie-Cert.pfx", "xero");
            IOAuthSession consumerSession = new XeroApiPrivateSession(UserAgentString, ConsumerKey, privateCertificate);

            consumerSession.MessageLogger = new DebugMessageLogger();

            return consumerSession;
        }

        public IOAuthSession AuthenticateForPayroll()
        {
            throw new System.NotImplementedException();
        }
    }
}
