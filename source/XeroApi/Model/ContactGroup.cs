using System;

namespace XeroApi.Model
{
    public class ContactGroup : ModelBase
    {
        public Guid ContactGroupID { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public Contacts Contacts { get; set; }
    }

    public class ContactGroups : ModelList<ContactGroup>
    {
    }
}