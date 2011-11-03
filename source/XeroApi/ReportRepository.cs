using System;
using System.Linq;
using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;
using XeroApi.Model.Reporting;

namespace XeroApi
{
    public class ReportRepository
    {
        private readonly IIntegrationProxy _integrationProxy;
        private readonly QueryProvider _queryProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReportRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="queryProvider">The query provider.</param>
        internal ReportRepository(IIntegrationProxy integrationProxy, QueryProvider queryProvider)
        {
            _integrationProxy = integrationProxy;
            _queryProvider = queryProvider;
        }


        /// <summary>
        /// Runs a dynamic report.
        /// </summary>
        /// <typeparam name="TReport">The type of the report.</typeparam>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        public Report RunDynamicReport<TReport>(TReport report)
            where TReport : DynamicReportBase
        {
            var queryDescription = new ReportQueryDescription
            {
                ElementName = "Report",
                ElementId = report.ReportName,
                QueryStringParams = report.GetQueryStringParamCollection()
            };

            string xml = _integrationProxy.FindElements(queryDescription);

            Response response = ModelSerializer.DeserializeTo<Response>(xml);

            return response.Reports[0];
        }


        /// <summary>
        /// Gets the published report.
        /// </summary>
        /// <param name="reportId">The report id.</param>
        /// <returns></returns>
        public Report GetPublishedReport(Guid reportId)
        {
            var queryDescription = new ReportQueryDescription
            {
                ElementName = "Report",
                ElementId = reportId.ToString(),
            };

            string xml = _integrationProxy.FindElements(queryDescription);

            Response response = ModelSerializer.DeserializeTo<Response>(xml);

            return response.Reports[0];
        }

        /// <summary>
        /// Gets the get all published reports.
        /// </summary>
        /// <value>The get all published reports.</value>
        public IQueryable<Report> ListAllPublishedReports
        {
            get { return new ApiQuery<Report>(_queryProvider); }
        }

    }
}
