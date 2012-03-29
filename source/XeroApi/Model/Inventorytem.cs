using System;

namespace XeroApi.Model
{
    public class Item : ModelBase
    {
        [ItemId]
        public Guid ItemID { get; set; }

        [ItemNumber]
        public string Code { get; set; }

        public string Description { get; set; }

        [ItemUpdatedDate]
        public DateTime UpdatedDateUTC { get; set; }

        public ItemPrice PurchaseDetails { get; set; }

        public ItemPrice SalesDetails { get; set; }

        public override string ToString()
        {
            return string.Format("Item:{0}", Code);
        }
    }


    public class Items : ModelList<Item>
    {
    }
}
