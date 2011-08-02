using System;

namespace XeroApi.Model
{
    public class ItemPrice : ModelBase
    {
        public decimal? UnitPrice { get; set; }

        public string AccountCode { get; set; }

        public string TaxType { get; set; }
    }


    public class ItemPrices : ModelList<ItemPrice>
    {
    }
}
