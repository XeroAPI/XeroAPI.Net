namespace XeroApi.Model.Payroll
{
    public class BankAccount : EndpointModelBase
    {
        public string StatementText { get; set; }
        public string AccountName { get; set; }
        public int BSB { get; set; }
        public int AccountNumber { get; set; }
        public bool Remainder { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
    }

    public class BankAccounts : ModelList<BankAccount>
    {
    }    
}