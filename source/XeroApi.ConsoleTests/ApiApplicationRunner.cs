using DevDefined.OAuth.Consumer;
using XeroApi.Integration;
using XeroApi.Model.Serialize;

namespace XeroApi.ConsoleApp
{
    public abstract class ApiApplicationRunner
    {
        private static IOAuthSession ConsumerSession { get; set; }
        private static IModelSerializer _serializer;

        public static CoreRepository CreateRepository(IAuthenticate authenticate, IModelSerializer serializer)
        {
            _serializer = serializer;
            if (ConsumerSession == null)
            {
                ConsumerSession = authenticate.Authenticate();
            }

            // Wrap the authenticated consumerSession in the repository...            
            return ConsumerSession == null ? null : new CoreRepository(new CoreIntegrationProxy(ConsumerSession, _serializer.MimeType), _serializer);
        }

        public static PayrollRepository CreatePayrollRepository(IAuthenticate authenticate, IModelSerializer serializer)
        {
            _serializer = serializer;
            if (ConsumerSession == null)
            {
                ConsumerSession = authenticate.AuthenticateForPayroll();
            }

            // Wrap the authenticated consumerSession in the repository...            
            return ConsumerSession == null ? null : new PayrollRepository(new PayrollIntegrationProxy(ConsumerSession, _serializer.MimeType), _serializer);
        }
    }

    public interface IAuthenticate
    {
        IOAuthSession Authenticate();
        IOAuthSession AuthenticateForPayroll();
    }
}
