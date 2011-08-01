using System;

namespace XeroApi.Model
{
    public class Contact : ModelBase
    {

        public string Name { get; set; }

        public Guid ContactID { get; set; }

        public string ContactNumber { get; set; }

        public DateTime UpdatedDateUTC { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }
    }
}
