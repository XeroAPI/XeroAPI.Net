using System;

namespace XeroApi.Model
{
    public class Contact : ModelBase
    {
        [ItemId]
        public Guid ContactID { get; set; }

        [ItemNumber]
        public string ContactNumber { get; set; }

        [ItemUpdatedDate]
        public DateTime UpdatedDateUTC { get; set; }
        
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
