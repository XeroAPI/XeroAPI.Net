using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class BalanceSheetReport : DynamicReportBase
    {
        private readonly DateTime? _date;


        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceSheetReport"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        public BalanceSheetReport(DateTime? date = null)
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
