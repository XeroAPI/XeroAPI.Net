namespace XeroApi.Model.Constants
{
    public class InvoiceStatusCodes
    {
        public static string Draft
        {
            get { return "DRAFT"; }
        }
        public static string Submitted
        {
            get { return "SUBMITTED"; }
        }
        public static string Deleted
        {
            get { return "DELETED"; }
        }
        public static string Authorised
        {
            get { return "AUTHORISED"; }
        }
        public static string Paid
        {
            get { return "PAID"; }
        }
        public static string Voided
        {
            get { return "VOIDED"; }
        }
    }
}
