using DevDefined.OAuth.Consumer;
using XeroApi.Model.Serialize;

namespace XeroApi.Integration
{
    public class PayrollIntegrationProxy : IntegrationProxy
    {
        public PayrollIntegrationProxy(IOAuthSession oauthSession, string mimeType) :
            base(oauthSession, "payroll.xro/1.0/", mimeType)
        {
        }

        protected override string GetEndpointName(string endpointName)
        {
            if (endpointName != "TaxDeclaration" && endpointName != "Payslip")
                return base.GetEndpointName(endpointName);

            return endpointName;
        }
    }
}