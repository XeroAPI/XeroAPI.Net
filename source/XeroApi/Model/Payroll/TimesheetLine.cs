using System;

namespace XeroApi.Model.Payroll
{
    public class TimesheetLine : HasUpdatedDate
    {
        public Guid EarningsRateID { get; set; }
        public Guid TrackingItemID { get; set; }
        
        [System.Xml.Serialization.XmlArrayItem("NumberOfUnit")]
        public decimal[] NumberOfUnits { get; set; }        
    }

    public class TimesheetLines : ModelList<TimesheetLine>
    {
    }

    //public class NumberOfUnits : EndpointModelBase //: ModelList<NumberOfUnit>
    //{
    //    [System.Xml.Serialization.XmlArrayItem("NumberOfUnit")]
    //    public decimal[] NumberOfUnit { get; set; }
    //}


    //public class NumberOfUnit : EndpointModelBase
    //{
    //   // [System.Xml.Serialization.XmlRoot("NumberOfUnit")]
    //    //public decimal Value { get; set; }
    //}
}