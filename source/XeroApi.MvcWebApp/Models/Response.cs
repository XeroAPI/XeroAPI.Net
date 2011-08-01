using System;

namespace Xero.ScreencastWeb.Models
{
    public class Response
    {
        public Guid? Id;

        public string Status;

        public string ProviderName;

        public DateTime DateTimeUTC;

        public Organisations Organisations;

        public Invoices Invoices;

        public Contacts Contacts;
    }
}