using System;

namespace XeroApi.Model
{
    public class TrackingCategory : ModelBase
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public string Option { get; set; }

        public Guid TrackingCategoryID { get; set; }

        public Options Options { get; set; }
    }

    
    public class Option : ModelBase
    {
        public Guid? TrackingOptionID { get; set; }

        public string Name { get; set; }
    }

    
    public class Options : ModelList<Option>
    {
    }

    
    public class TrackingCategories : ModelList<TrackingCategory>
    {
    }
}