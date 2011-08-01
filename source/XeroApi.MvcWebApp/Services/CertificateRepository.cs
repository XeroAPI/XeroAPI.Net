using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using DevDefined.OAuth.Consumer;

namespace Xero.ScreencastWeb.Services
{
    public static class CertificateRepository
    {

        public static X509Certificate2 GetOAuthSigningCertificate()
        {
            string oauthCertificateFindType = ConfigurationManager.AppSettings["XeroApiOAuthCertificateFindType"];
            string oauthCertificateFindValue = ConfigurationManager.AppSettings["XeroApiOAuthCertificateFindValue"];

            if (string.IsNullOrEmpty(oauthCertificateFindType) || string.IsNullOrEmpty(oauthCertificateFindValue))
            {
                return null;
            }

            X509FindType x509FindType = (X509FindType)Enum.Parse(typeof(X509FindType), oauthCertificateFindType);

            // Search the LocalMachine certificate store for matching X509 certificates.
            X509Store certStore = new X509Store("My", StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection certificateCollection = certStore.Certificates.Find(x509FindType, oauthCertificateFindValue, false);
            certStore.Close();

            if (certificateCollection.Count == 0)
            {
                throw new ApplicationException(string.Format("An OAuth certificate matching the X509FindType '{0}' and Value '{1}' cannot be found in the local certificate store.", oauthCertificateFindType, oauthCertificateFindValue));
            }

            return certificateCollection[0];
        }

        /// <summary>
        /// Gets the OAuth signing certificate from the local certificate store, if specfified in app.config.
        /// </summary>
        /// <returns></returns>
        public static AsymmetricAlgorithm GetOAuthSigningCertificatePrivateKey()
        {
            var oauthSigningCertificate = GetOAuthSigningCertificate();

            if (oauthSigningCertificate == null)
            {
                return null;
            }

            if (!oauthSigningCertificate.HasPrivateKey)
            {
                throw new ApplicationException("The specified OAuth Certificate find details matched a certificate, but there is not private key stored in the certificate");
            }

            return oauthSigningCertificate.PrivateKey;
        }

        public static X509Certificate2 GetClientSslCertificate()
        {
            ICertificateFactory clientSslCertificateFactory = GetClientSslCertificateFactory();
            return clientSslCertificateFactory.CreateCertificate();
        }

        /// <summary>
        /// Return a CertificateFactory that can read the Client SSL certificate from the local machine certificate store
        /// </summary>
        /// <returns></returns>
        public static ICertificateFactory GetClientSslCertificateFactory()
        {
            string oauthCertificateFindType = ConfigurationManager.AppSettings["XeroApiSslCertificateFindType"];
            string oauthCertificateFindValue = ConfigurationManager.AppSettings["XeroApiSslCertificateFindValue"];

            if (string.IsNullOrEmpty(oauthCertificateFindType) || string.IsNullOrEmpty(oauthCertificateFindValue))
            {
                return new NullCertificateFactory();
            }

            X509FindType x509FindType = (X509FindType)Enum.Parse(typeof(X509FindType), oauthCertificateFindType);
            ICertificateFactory certificateFactory = new LocalMachineCertificateFactory(oauthCertificateFindValue, x509FindType);

            if (certificateFactory.CreateCertificate() == null)
            {
                throw new ApplicationException(string.Format("A client SSL certificate matching the X509FindType '{0}' and value '{1}' cannot be found in the local certificate store.", oauthCertificateFindType, oauthCertificateFindValue));
            }

            return certificateFactory;
        }

    }
}
