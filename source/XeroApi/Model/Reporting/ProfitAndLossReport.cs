using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class ProfitAndLossReport : DynamicReportBase
    {
        private DateTime? _fromDate;
        private DateTime? _toDate;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfitAndLossReport"/> class.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public ProfitAndLossReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            _fromDate = fromDate;
            _toDate = toDate;
        }

        /// <summary>
        /// Generates the querystring params.
        /// </summary>
        /// <param name="queryStringParams">The query string params.</param>
        internal override void GenerateQuerystringParams(NameValueCollection queryStringParams)
        {
            if (_fromDate.HasValue)
                queryStringParams.Add("fromDate", _fromDate.Value.ToString(ReportDateFormatString));

            if (_toDate.HasValue)
                queryStringParams.Add("toDate", _toDate.Value.ToString(ReportDateFormatString));
        }
    }
}
