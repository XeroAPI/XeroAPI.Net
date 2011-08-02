using System;

namespace XeroApi.Model
{
    public class ManualJournal : ModelBase
    {
        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        [ItemId]
        public Guid ManualJournalID { get; set; }

        public DateTime? Date { get; set; }
        
        public string Status { get; set; }

        public LineAmountType LineAmountTypes { get; set; }
        
        public string Narration { get; set; }
        
        public ManualJournalLineItems JournalLines { get; set; }
    }
    
    public class ManualJournals : ModelList<ManualJournal>
    {
    }
}