using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public abstract class DynamicReportBase
    {

        internal abstract void GetQueryStringParams(NameValueCollection queryStringCollection);


        internal string ReportName
        {
            get
            {
                Type thisType = this.GetType();
                return thisType.Name.Replace("Report", string.Empty);
            }
        }
    }
}
