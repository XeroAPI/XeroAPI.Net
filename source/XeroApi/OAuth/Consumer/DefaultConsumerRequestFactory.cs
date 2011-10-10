using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Consumer
{
    public class DefaultConsumerRequestFactory : IConsumerRequestFactory
    {
        static IConsumerRequestFactory _defaultInstance;

        public DefaultConsumerRequestFactory()
        {
        }

        public DefaultConsumerRequestFactory(ICertificateFactory clientSslCertificateFactory)
        {
            CertificateFactory = clientSslCertificateFactory;
        }

        public static IConsumerRequestFactory Instance
        {
            get { return _defaultInstance ?? (_defaultInstance = new DefaultConsumerRequestFactory()); }
            set { _defaultInstance = value; }
        }

        public ICertificateFactory CertificateFactory
        {
            get; 
            set;
        }

        public IConsumerRequest CreateConsumerRequest(IOAuthSession session, IOAuthContext context, IOAuthConsumerContext consumerContext)
        {
            return new ConsumerRequest(session, context, consumerContext, CertificateFactory);
        }
    }
}