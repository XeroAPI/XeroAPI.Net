using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using XeroApi.Integration;

namespace XeroApi.Model.Reporting
{
    public class ReportQueryDescription : IApiQueryDescription
    {

        public string ElementName
        {
            get;
            set;
        }

        public string ElementId
        {
            get;
            set;
        }

        public DateTime? UpdatedSinceDate
        {
            get;
            set;
        }

        public NameValueCollection QueryStringParams
        {
            get;
            set;
        }
        
    }
}
