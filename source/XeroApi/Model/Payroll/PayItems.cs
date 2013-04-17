namespace XeroApi.Model.Payroll
{
    public class PayItems : EndpointModelBase
    {
        public EarningsRates EarningsRates { get; set; }
        public DeductionTypes DeductionTypes { get; set; }
        public LeaveTypes LeaveTypes { get; set; }
        public ReimbursementTypes ReimbursementTypes { get; set; }
    }
}
