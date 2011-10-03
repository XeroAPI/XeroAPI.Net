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
        private readonly ApiQueryProvider _queryProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReportRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="queryProvider">The query provider.</param>
        internal ReportRepository(IIntegrationProxy integrationProxy, ApiQueryProvider queryProvider)
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
        public TReport RunDynamicReport<TReport>(TReport report)
            where TReport : DynamicReportBase
        {
            return null;

            /*ApiQueryDescription queryDescription = new ReportQueryDescription
                                                       {
                                                           ElementName = "Reports",
                                                           ElementId = report.ReportName,
                                                           
                                                       };

            _integrationProxy.FindElements()*/
            
        }


        /// <summary>
        /// Gets the published report.
        /// </summary>
        /// <typeparam name="TReport">The type of the report.</typeparam>
        /// <param name="reportId">The report id.</param>
        /// <returns></returns>
        public TReport GetPublishedReport<TReport>(Guid reportId)
            where TReport : PublishedReportBase
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the get all published reports.
        /// </summary>
        /// <value>The get all published reports.</value>
        public IQueryable<Report> GetAllPublishedReports
        {
            get { return new ApiQuery<Report>(_queryProvider); }
        }

    }
}
