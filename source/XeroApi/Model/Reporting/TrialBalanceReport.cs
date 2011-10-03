using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class TrialBalanceReport : DynamicReportBase
    {
        private DateTime? _fromDate;
        private DateTime? _toDate;


        // User-accesible constructor
        public TrialBalanceReport(DateTime? fromDate = null, DateTime? toDate = null)
        {
            _fromDate = fromDate;
            _toDate = toDate;
        }

        // De-serialising constructor
        internal TrialBalanceReport()
        {
            
        }

        internal override void GetQueryStringParams(NameValueCollection collection)
        {
            if (_fromDate.HasValue)
                collection.Add("fromDate", _fromDate.Value.ToString("u"));

            if (_toDate.HasValue)
                collection.Add("toDate", _toDate.Value.ToString("u"));
        }
    }
}
