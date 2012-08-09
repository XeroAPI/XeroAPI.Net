using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XeroApi.Model
{
    public abstract class TradingTransaction : EndpointModelBase
    {
        [ItemUpdatedDate]
        public DateTime? UpdatedDateUTC { get; set; }

        public string Type { get; set; }

        public string Reference { get; set; }

        [ReadOnly]
        public bool? SentToContact { get; set; }

        public decimal? CurrencyRate { get; set; }

        public Contact Contact { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DueDate { get; set; }

        public virtual Guid? BrandingThemeID { get; set; }

        public virtual string Status { get; set; }

        public LineAmountType LineAmountTypes { get; set; }

        public virtual LineItems LineItems { get; set; }

        public virtual decimal? SubTotal { get; set; }

        public virtual decimal? TotalTax { get; set; }

        public virtual decimal? Total { get; set; }

        public virtual string CurrencyCode { get; set; }

        [ReadOnly]
        public DateTime? FullyPaidOnDate { get; set; }
    }
}
