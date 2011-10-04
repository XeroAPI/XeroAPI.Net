using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XeroApi.Model.Reporting
{
    public class BankStatementReport : DynamicReportBase
    {
        private readonly Guid _bankAccountId;
        private DateTime? _fromDate;
        private DateTime? _toDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankStatementReport"/> class.
        /// </summary>
        /// <param name="bankAccountId">The bank account id.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public BankStatementReport(Guid bankAccountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            _bankAccountId = bankAccountId;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BankStatementReport"/> class.
        /// </summary>
        /// <param name="bankAccount">The bank account.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public BankStatementReport(Account bankAccount, DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (bankAccount == null || bankAccount.Type != "BANK")
            {
                throw new ArgumentException("The parameter 'bankAccount' is null or not of the correct type", "bankAccount");
            }

            _bankAccountId = bankAccount.AccountID;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        /// <summary>
        /// Generates the querystring params.
        /// </summary>
        /// <param name="queryStringParams">The query string params.</param>
        internal override void GenerateQuerystringParams(NameValueCollection queryStringParams)
        {
            queryStringParams.Add("bankAccountID", _bankAccountId.ToString());

            if (_fromDate.HasValue)
                queryStringParams.Add("fromDate", _fromDate.Value.ToString(ReportDateFormatString));

            if (_toDate.HasValue)
                queryStringParams.Add("toDate", _toDate.Value.ToString(ReportDateFormatString));
        }
    }
}
