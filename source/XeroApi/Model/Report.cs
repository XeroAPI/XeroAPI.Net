using System;
using System.Xml.Serialization;

namespace XeroApi.Model
{
    // TODO: All these report classes need a little tidying up

    public class Report : ModelBase
    {
        [ItemId]
        public string ReportID { get; set; }

        public string ReportName { get; set; }

        public string ReportType { get; set; }

        [XmlArray("ReportTitles")]
        [XmlArrayItem("ReportTitle")]
        public string[] ReportTitles { get; set; }

        public string ReportDate { get; set; }

        [ItemUpdatedDate]
        public DateTime UpdatedDateUTC { get; set; }

        public ReportAttributes Attributes { get; set; }

        public ReportFields Fields { get; set; }

        [XmlArray("Rows")]
        [XmlArrayItem("Row")]
        public ReportRow[] Rows { get; set; }
    }

    public class ReportAttribute : ModelBase
    {
        public string Name;
        public string Description;
        public string Value;
    }

    public class ReportAttributes : ModelList<ReportAttribute>
    {
    }

    [XmlRoot(DataType = "Row", Namespace = "", ElementName = "Row")]
    public class ReportRow
    {
        public string RowType { get; set; }

        [XmlArray("Cells")]
        [XmlArrayItem("Cell")]
        public ReportCell[] Cells { get; set; }

        public string Title { get; set; }

        [XmlArray("Rows")]
        [XmlArrayItem("Row")]
        public ReportRow[] Rows { get; set; }
    }
    
    public class ReportCell : ModelBase
    {
        public string Value { get; set; }

        [XmlArray("Attributes")]
        [XmlArrayItem("Attribute")]
        public ReportCellAttributes Attributes { get; set; }
    }

    public class ReportCellAttribute : ModelBase
    {
        public string Value { get; set; }
        public string Id { get; set; }
    }

    public class ReportCellAttributes : ModelList<ReportCellAttribute>
    {
    }

    public class ReportField : ModelBase
    {
        public string FieldID { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Format { get; set; }
    }

    public class ReportFields : ModelList<ReportField>
    {
    }

    public class Reports : ModelList<Report>
    {
    }
}
