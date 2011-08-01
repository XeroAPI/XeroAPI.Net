using System.Xml.Serialization;

namespace Xero.ScreencastWeb.Models
{
    public enum ContactStatus
    {
        [XmlEnum(Name = "ACTIVE")]
        Active,

        [XmlEnum(Name = "DELETED")]
        Deleted
    }
}
