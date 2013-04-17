
namespace XeroApi.Model.Payroll
{
    public abstract class HasLines : EndpointModelBase
    {
        public EarningsLines EarningsLines { get; set; }
        public DeductionLines DeductionLines { get; set; }
        public SuperLines SuperLines { get; set; }
        public ReimbursementLines ReimbursementLines { get; set; }
        public LeaveLines LeaveLines { get; set; }        
    }
}
