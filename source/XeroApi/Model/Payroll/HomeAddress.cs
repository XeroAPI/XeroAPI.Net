using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class HomeAddress : EndpointModelBase
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string City { get; set; }
        public State Region { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
    }
}