using DevDefined.OAuth.Framework;

namespace DevDefined.OAuth.Consumer
{
  public interface IConsumerRequestFactory
  {
    IConsumerRequest CreateConsumerRequest(IOAuthSession session, IOAuthContext context, IOAuthConsumerContext consumerContext);
  }
}