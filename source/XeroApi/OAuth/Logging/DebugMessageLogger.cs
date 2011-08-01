using DevDefined.OAuth.Consumer;

namespace DevDefined.OAuth.Logging
{
    public class DebugMessageLogger : IMessageLogger
    {
        
        public void LogMessage(IConsumerRequest request, IConsumerResponse response)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", request.Context.RequestMethod, request.Context.GenerateUrl()));
            System.Diagnostics.Debug.WriteLine(string.Format("HTTP {0}\r\n{1}", response.ResponseCode, response.Content));
        }
        
    }
}
