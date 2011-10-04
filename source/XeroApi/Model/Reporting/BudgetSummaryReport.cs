using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class BudgetSummaryReport : DynamicReportBase
    {
        private readonly int? _periods;
        private readonly int? _timeframe;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetSummaryReport"/> class.
        /// </summary>
        /// <param name="periods">The number of periods to compare (integer between 1 and 12)</param>
        /// <param name="timeframe">The period size to compare to (1=month, 3=quarter, 12=year)</param>
        public BudgetSummaryReport(int? periods = null, int? timeframe = null)
        {
            _periods = periods;
            _timeframe = timeframe;
        }

        /// <summary>
        /// Generates the querystring params.
        /// </summary>
        /// <param name="queryStringParams">The query string params.</param>
        internal override void GenerateQuerystringParams(NameValueCollection queryStringParams)
        {
            if (_periods.HasValue)
                queryStringParams.Add("periods", _periods.Value.ToString());

            if (_timeframe.HasValue)
                queryStringParams.Add("timeframe", _timeframe.Value.ToString());
        }
    }
}
