using System;
using System.Text;

namespace XeroApi.Model
{
    public class ContactPerson : ModelBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public bool IncludeInEmails { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(FirstName))
                sb.Append(FirstName + Environment.NewLine);

            if (!string.IsNullOrEmpty(LastName))
                sb.Append(LastName + Environment.NewLine);

            if (!string.IsNullOrEmpty(EmailAddress))
                sb.Append(EmailAddress + Environment.NewLine);

            return sb.ToString().TrimEnd(' ');
        }
    }

    public class ContactPersons : ModelList<ContactPerson>
    {
    }
}
