using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class AgedPayablesByContactReport : DynamicReportBase
    {
        private readonly Guid _contactId;
        private readonly DateTime? _date;
        private readonly DateTime? _fromDate; 
        private readonly DateTime? _toDate;


        /// <summary>
        /// Initializes a new instance of the <see cref="AgedPayablesByContactReport"/> class.
        /// </summary>
        /// <param name="contactId">The ContactID of the contact to filter on.</param>
        /// <param name="date">Shows payments up to this date. Defaults to end of the current month</param>
        /// <param name="fromDate">Show all payable invoices from this date for contact</param>
        /// <param name="toDate">Show all payable invoices to this date for the contact</param>
        public AgedPayablesByContactReport(Guid contactId, DateTime? date = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            _contactId = contactId;
            _date = date;
            _toDate = toDate;
            _fromDate = fromDate;
        }
        
        /// <summary>
        /// Generates the querystring params.
        /// </summary>
        /// <param name="queryStringParams">The query string params.</param>
        internal override void GenerateQuerystringParams(NameValueCollection queryStringParams)
        {
            queryStringParams.Add("contactID", _contactId.ToString());

            if (_fromDate.HasValue)
                queryStringParams.Add("fromDate", _fromDate.Value.ToString(ReportDateFormatString));

            if (_toDate.HasValue)
                queryStringParams.Add("toDate", _toDate.Value.ToString(ReportDateFormatString));

            if (_date.HasValue)
                queryStringParams.Add("date", _date.Value.ToString(ReportDateFormatString));
        }
    }
}
