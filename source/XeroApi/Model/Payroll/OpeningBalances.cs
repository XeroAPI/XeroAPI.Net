using System;

namespace XeroApi.Model.Payroll
{
    public class OpeningBalances : HasLines
    {
        public DateTime OpeningBalanceDate { get; set; }
        public decimal Tax { get; set; }        
    }
}