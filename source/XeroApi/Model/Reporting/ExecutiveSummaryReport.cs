using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class ExecutiveSummaryReport : DynamicReportBase
    {
        private DateTime? _date;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutiveSummaryReport"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public ExecutiveSummaryReport(DateTime? date = null)
        {
            _date = date;
        }

        /// <summary>
        /// Generates the querystring params.
        /// </summary>
        /// <param name="queryStringParams">The query string params.</param>
        internal override void GenerateQuerystringParams(NameValueCollection queryStringParams)
        {
            if (_date.HasValue)
                queryStringParams.Add("date", _date.Value.ToString(ReportDateFormatString));
        }
        
    }
}
