using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Consumer
{
    public class DefaultConsumerRequestFactory : IConsumerRequestFactory
    {
        readonly ICertificateFactory _clientSslCertificateFactory;
        static IConsumerRequestFactory _defaultInstance;

        public DefaultConsumerRequestFactory()
        {
        }

        public DefaultConsumerRequestFactory(ICertificateFactory clientSslCertificateFactory)
        {
            _clientSslCertificateFactory = clientSslCertificateFactory;
        }

        public static IConsumerRequestFactory Instance
        {
            get { return _defaultInstance ?? (_defaultInstance = new DefaultConsumerRequestFactory()); }
            set { _defaultInstance = value; }
        }

        public IConsumerRequest CreateConsumerRequest(IOAuthSession session, IOAuthContext context, IOAuthConsumerContext consumerContext, IToken token)
        {
            return new ConsumerRequest(session, context, consumerContext, _clientSslCertificateFactory);
        }
    }
}