using System;

namespace XeroApi.Model
{
    public class Journal : ModelBase
    {

        public Guid JournalID { get; set; }

        public DateTime JournalDate { get; set; }

        public long JournalNumber { get; set; }

        public DateTime CreatedDateUTC { get; set; }

        public string Reference { get; set; }

        public JournalLines JournalLines { get; set; }
    }
    

    public class Journals : ModelList<Journal>
    {
    }
}
