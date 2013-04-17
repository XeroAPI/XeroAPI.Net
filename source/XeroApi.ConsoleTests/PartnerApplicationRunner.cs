using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using XeroApi.Model.Serialize;
using XeroApi.OAuth;

namespace XeroApi.ConsoleApp
{
    class PartnerApplicationRunner : ApiApplicationRunner, IAuthenticate
    {
        private const string UserAgent = "Xero.API.Console.CS (Partner App Testing)";
        private const string ConsumerKey = "ZWVJMWFMNZJMYJG0NDRIY2IYZGIWMZ";

        private static readonly X509Certificate2 OAuthCertificate = new X509Certificate2(@"D:\Stevie-Cert.pfx", "xero");
        private static readonly X509Certificate2 ClientSslCertificate = new X509Certificate2(@"D:\EnTrust-D4-Mk2.p12", "xero");

        private static readonly IModelSerializer _serializer = new XmlModelSerializer();

        public static CoreRepository CreateRepository()
        {
            return CreateRepository(new PartnerApplicationRunner(), _serializer);
        }

        public static PayrollRepository CreatePayrollRepository()
        {
            return CreatePayrollRepository(new PartnerApplicationRunner(), _serializer);
        }

        public IOAuthSession Authenticate()
        {
            return AuthenticateInternal();
        }

        public IOAuthSession AuthenticateForPayroll()
        {
            return AuthenticateInternal(true);
        }

        private IOAuthSession AuthenticateInternal(bool addPayroll = false)
        {
            var consumerSession = new XeroApiPartnerSession(
                UserAgent,
                ConsumerKey,
                OAuthCertificate, // OAuth signing certificate
                ClientSslCertificate, // Client SSL Certificate
                new InMemoryTokenRepository())
            {
                MessageLogger = new DebugMessageLogger()
            };

            // 1. Get a request token
            IToken requestToken = consumerSession.GetRequestToken();

            Console.WriteLine("Request Token Key: {0}", requestToken.Token);
            Console.WriteLine("Request Token Secret: {0}", requestToken.TokenSecret);

            // 2. Get the user to log into Xero using the request token in the querystring
            string authorisationUrl = consumerSession.GetUserAuthorizationUrl();

            if (addPayroll)
            {
                authorisationUrl += "&scope=payroll.employees,payroll.payitems,payroll.payrollcalendars,payroll.payruns,payroll.payslip,payroll.superfunds,payroll.taxdeclaration,payroll.timesheets,payroll.leaveapplications";
            }

            Process.Start(authorisationUrl);

            // 3. Get the use to enter the authorisation code from Xero (4-7 digit number)
            Console.WriteLine("Please input the code you were given in Xero:");
            var verificationCode = Console.ReadLine();

            if (string.IsNullOrEmpty(verificationCode))
            {
                Console.WriteLine("You didn't type a verification code!");
                return null;
            }

            // 4. Use the request token and verification code to get an access token
            AccessToken accessToken;

            try
            {
                accessToken = consumerSession.ExchangeRequestTokenForAccessToken(verificationCode.Trim());
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return null;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);
            Console.WriteLine("Access Token Lasts for: {0}", accessToken.TokenTimespan);
            Console.WriteLine("Session Lasts for: {0}", accessToken.SessionTimespan);
            
            // 5. Wait a second and try and renew the access token..
            Console.WriteLine("Renewing the access token...");
            System.Threading.Thread.Sleep(1000);

            try
            {
                accessToken = consumerSession.RenewAccessToken();
            }
            catch (OAuthException ex)
            {
                Console.WriteLine("An OAuthException was caught:");
                Console.WriteLine(ex.Report);
                return null;
            }

            Console.WriteLine("Access Token Key: {0}", accessToken.Token);
            Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret);
            Console.WriteLine("Access Token Lasts for: {0}", accessToken.TokenTimespan);
            Console.WriteLine("Session Lasts for: {0}", accessToken.SessionTimespan);

            return consumerSession;
        }
    }
}
