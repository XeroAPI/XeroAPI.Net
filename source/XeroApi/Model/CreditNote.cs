using System;

namespace XeroApi.Model
{
    public class CreditNote : TradingTransaction
    {
        [ItemId]
        public Guid CreditNoteID { get; set; }

        [ItemNumber]
        public string CreditNoteNumber { get; set; }

        public decimal? AppliedAmount { get; set; }

    }
    
    public class CreditNotes : ModelList<CreditNote>
    {
    }
}