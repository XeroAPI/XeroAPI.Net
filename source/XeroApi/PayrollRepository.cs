using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Exceptions;
using XeroApi.Integration;
using XeroApi.Linq;
using XeroApi.Model;
using XeroApi.Model.Payroll;
using XeroApi.Model.Serialize;
using Employee = XeroApi.Model.Payroll.Employee;
using Response = XeroApi.Model.Payroll.Response;

namespace XeroApi
{
    public class PayrollRepository : GenericRepository<Response>, IRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreRepository"/> class.
        /// </summary>
        /// <param name="integrationProxy">The integration proxy.</param>
        /// <param name="serializer">The serializer to be used</param>
        public PayrollRepository(IIntegrationProxy integrationProxy, IModelSerializer serializer)
            : base(integrationProxy, serializer)
        {
        }

        public new TModel Create<TModel>(TModel itemsToCreate)
            where TModel : EndpointModelBase
        {
            throw new NotImplementedException("Payroll does not support PUT");
        }

        public new IEnumerable<TModel> Create<TModel>(ICollection<TModel> itemsToCreate)
            where TModel : EndpointModelBase
        {
            throw new NotImplementedException("Payroll does not support PUT");
        }

        private IQueryable<T> CallApi<T>()
        {
            try
            {
                return new ApiQuery<T>(Provider);
            }
            catch (Exception e)
            {
                throw new ApiResponseException(e.Message);
            }
        }

        public IQueryable<Employee> Employees
        {
            get { return CallApi<Employee>(); }
        }
        
        public IQueryable<LeaveApplication> LeaveApplications
        {
            get { return CallApi<LeaveApplication>(); }
        }

        public PayItems PayItems
        {
            get { return CallApi<PayItems>().FirstOrDefault(); }
        }

        public IQueryable<PayrollCalendar> PayrollCalendars
        {
            get { return CallApi<PayrollCalendar>(); }
        }

        public IQueryable<PayRun> PayRuns
        {
            get { return CallApi<PayRun>(); }
        }

        public IQueryable<SuperFund> Superfunds
        {
            get { return CallApi<SuperFund>(); }
        }

        public IQueryable<Timesheet> Timesheets
        {
            get { return CallApi<Timesheet>(); }
        }

        public Payslip GetPayslipDetails(Payslip payslip)
        {
            return FindById<Payslip>(payslip.PayslipID, true);
        }

        public PayRun GetPayRunDetails(PayRun payRun)
        {
            return FindById<PayRun>(payRun.PayRunID);
        }

        public LeaveApplication GetLeaveApplication(Employee employee)
        {
            return FindById<LeaveApplication>(employee.EmployeeID);
        }

        public TaxDeclaration GetTaxDeclaration(Employee e)
        {
            return FindById<TaxDeclaration>(e.EmployeeID);
        }
    }
}
