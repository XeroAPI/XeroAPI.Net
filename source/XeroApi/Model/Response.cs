using System;
using System.Reflection;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    [XmlRoot(ElementName="Response", Namespace="")]
    [Serializable]
    public class Response
    {
        public DateTime DateTimeUTC { get; set; }
        public string Status { get; set; }
        public string ProviderName { get; set; }

        public Accounts Accounts { get; set; }
        public BankTransactions BankTransactions { get; set; }
        public BrandingThemes BrandingThemes { get; set; }
        public Contacts Contacts { get; set; }
        public CreditNotes CreditNotes { get; set; }
        public Currencies Currencies { get; set; }
        public Employees Employees { get; set; }
        public Invoices Invoices { get; set; }
        public Items Items { get; set; }
        public Journals Journals { get; set; }
        public ManualJournals ManualJournals { get; set; }
        public Organisations Organisations { get; set; }
        public Payments Payments { get; set; }
        public Reports Reports { get; set; }
        public TaxRates TaxRates { get; set; }
        public TrackingCategories TrackingCategories { get; set; }
        public Attachments Attachments { get; set; }
        public Users Users { get; set; }
        public Receipts Receipts { get; set; }
        public ExpenseClaims ExpenseClaims { get; set; }

        #region Type Helper Methods

        public IModelList GetTypedProperty(Type elementListType)
        {
            foreach (PropertyInfo property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType.IsAssignableFrom(elementListType))
                {
                    return (IModelList)property.GetValue(this, new object[] {});
                }
            }

            throw new ArgumentException("There are not properties that are of type " + elementListType.Name);
        }

        public IModelList<TModel> GetTypedProperty<TModel>()
            where TModel : ModelBase
        {
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                if (typeof(IModelList<TModel>).IsAssignableFrom(property.PropertyType))
                {
                    return (IModelList<TModel>)property.GetValue(this, new object[] { });
                }
            }

            throw new ArgumentException("There are no response elements that are a collection of type " + typeof(TModel).Name);
        }

        #endregion
    }
}
