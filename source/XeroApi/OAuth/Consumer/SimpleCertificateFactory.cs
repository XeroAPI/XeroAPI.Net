using System.Security.Cryptography.X509Certificates;

namespace DevDefined.OAuth.Consumer
{
    /// <summary>
    /// Simple Certificate Factory
    /// </summary>
    public class SimpleCertificateFactory : ICertificateFactory
    {
        private readonly X509Certificate2 _certificate;

        public SimpleCertificateFactory(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public X509Certificate2 CreateCertificate()
        {
            return _certificate;
        }

        public int GetMatchingCertificateCount()
        {
            return (_certificate == null) ? 0 : 1;
        }
    }
}
