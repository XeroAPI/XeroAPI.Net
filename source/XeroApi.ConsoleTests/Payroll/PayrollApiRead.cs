using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Model.Payroll;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.ConsoleApp.Payroll
{
    public class PayrollApiRead
    {
        private static PayrollRepository _payrollRepository;
        private static Employee _employee;

        public void Exercise(PayrollRepository repository, Employee employee)
        {
            _payrollRepository = repository;
            _employee = employee;

            if (_payrollRepository != null)
            {
                if (_employee != null)
                {
                    TestGetEmployee(_employee);
                    TestGetLeaveApplications(_employee);
                    TestGetTaxDeclaration(_employee);
                }

                TestGetPayItems();
                TestGetPayrollCalendar();
                TestSuperfunds();
                TestTimesheets();
                TestPayRunAndSlips();
            }
        }

        private static void TestPayRunAndSlips()
        {
            var payRuns = TestGetPayRuns();
            if (payRuns != null)
            {
                foreach (var pr in payRuns)
                {
                    var run = TestGetPayRun(pr);

                    if (run != null)
                    {
                        foreach (var s in run.Payslips)
                        {
                            TestGetPayslip(s);
                        }
                    }
                }
            }
        }

        private static void TestGetPayslip(Payslip payslip)
        {
            Console.WriteLine("<--- Detailed Payslip --->");
            var s = _payrollRepository.GetPayslipDetails(payslip);
            if (s != null)
            {
                PayrollPrinter.PrintPayslip(s);
            }
        }

        private static IEnumerable<PayRun> TestGetPayRuns()
        {
            var payRuns = _payrollRepository.PayRuns;
            if (payRuns != null)
            {
                PayrollPrinter.PrintPayRuns(payRuns);
            }

            return payRuns;
        }

        private static PayRun TestGetPayRun(PayRun payRun)
        {
            var run = _payrollRepository.GetPayRunDetails(payRun);
            if (run != null)
            {
                PayrollPrinter.PrintPayRun(run);
            }

            return run;
        }

        private static void TestTimesheets()
        {
            Console.WriteLine("<--- Timesheets --->");
            var timesheets = _payrollRepository.Timesheets;
            if (timesheets != null)
            {
                foreach (var t in timesheets)
                {
                    Console.WriteLine("<--- Timesheet --->");
                    Console.WriteLine("TimesheetID: {0}", t.TimesheetID);
                    Console.WriteLine("EmployeeID: {0}", t.EmployeeID);
                    Console.WriteLine("StartDate: {0}", t.StartDate);
                    Console.WriteLine("EndDate: {0}", t.EndDate);
                    Console.WriteLine("Status: {0}", t.Status);
                    Console.WriteLine("Hours: {0}", t.Hours);

                    Console.WriteLine("<--- Timesheet Lines --->");
                    foreach (var line in t.TimesheetLines)
                    {
                        Console.WriteLine("<--- Timesheet Line --->");
                        if (line.TrackingItemID != Guid.Empty)
                        {
                            Console.WriteLine("TrackingItemID: {0}", line.TrackingItemID);
                        }

                        Console.WriteLine("EarningsRateID: {0}", line.EarningsRateID);
                        //Console.WriteLine("{0:0.00}\t{1:0.00}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t",
                        //    line.NumberOfUnits[0],
                        //    line.NumberOfUnits[1],
                        //    line.NumberOfUnits[2],
                        //    line.NumberOfUnits[3],
                        //    line.NumberOfUnits[4],
                        //    line.NumberOfUnits[5],
                        //    line.NumberOfUnits[6]);
                    }
                }
            }
        }

        private static void TestSuperfunds()
        {
            Console.WriteLine("<--- Superfunds --->");
            var funds = _payrollRepository.Superfunds;
            if (funds != null)
            {
                foreach (var f in funds)
                {
                    Console.WriteLine("<--- Superfund --->");
                    Console.WriteLine("SuperFundID: {0}", f.SuperFundID);
                    Console.WriteLine("EmployerNumber {0}", f.EmployerNumber);
                    Console.WriteLine("ABN: {0}", f.ABN);
                    Console.WriteLine("Type: {0}", f.Type);

                    if (f.Type == SuperfundType.SMSF)
                    {
                        Console.WriteLine("Name: {0}", f.Name);
                        Console.WriteLine("AccountName: {0}", f.AccountName);
                        Console.WriteLine("AccountNumber: {0}", f.AccountNumber);
                        Console.WriteLine("BSB: {0}", f.BSB);
                    }
                    else
                    {
                        Console.WriteLine("SPIN: {0}", f.SPIN);
                    }

                    Console.WriteLine("UpdatedDateUTC: {0}", f.UpdatedDateUTC);
                }
            }
        }

        private static void TestGetTaxDeclaration(Employee employee)
        {
            Console.WriteLine("<--- Tax Declaration --->");
            var d = _payrollRepository.GetTaxDeclaration(employee);
            if (d != null)
            {
                Console.WriteLine("EmployeeID: {0}", d.EmployeeID);
                Console.WriteLine("TaxFileNumber: {0}", d.TaxFileNumber);
                Console.WriteLine("EmploymentBasis: {0}", d.EmploymentBasis);
                Console.WriteLine("AustralianResidentForTaxPurposes: {0}", d.AustralianResidentForTaxPurposes);
                Console.WriteLine("EligibleToReceiveLeaveLoading: {0}", d.EligibleToReceiveLeaveLoading);
                Console.WriteLine("HasHELPDebt: {0}", d.HasHELPDebt);
                Console.WriteLine("HasSFSSDebt: {0}", d.HasSFSSDebt);
                Console.WriteLine("TFNPendingOrExemptionHeld: {0}", d.TFNPendingOrExemptionHeld);
                Console.WriteLine("TaxFreeThresholdClaimed: {0}", d.TaxFreeThresholdClaimed);
                Console.WriteLine("ApprovedWithholdingVariationPercentage {0}", d.ApprovedWithholdingVariationPercentage);
                Console.WriteLine("TaxOffsetEstimatedAmount {0}", d.TaxOffsetEstimatedAmount);
                Console.WriteLine("UpwardVariationTaxWithholdingAmount {0}", d.UpwardVariationTaxWithholdingAmount);
                Console.WriteLine("UpdatedDateUTC: {0}", d.UpdatedDateUTC);
            }
        }

        private static void TestGetEmployee(Employee employee)
        {
            var e = _payrollRepository.FindById<Employee>(employee.EmployeeID);
            if (e != null)
            {
                PayrollPrinter.PrintEmployeeFull(e);
            }
        }

        private static void TestGetPayItems()
        {
            PayItems payItems = _payrollRepository.PayItems;
            if (payItems != null)
            {
                PayrollPrinter.PrintPayItems(payItems);
            }
        }

        private static void TestGetPayrollCalendar()
        {
            Console.WriteLine("<--- Payroll Calendars --->");
            IQueryable<PayrollCalendar> calendars = _payrollRepository.PayrollCalendars;
            foreach (var c in calendars)
            {
                PayrollPrinter.PrintPayrollCalendar(c);
            }
        }

        private static void TestGetLeaveApplications(Employee employee)
        {
            var application = _payrollRepository.GetLeaveApplication(employee);
            if (application != null)
            {
                PayrollPrinter.PrintLeaveApplication(application);
            }
        }
    }
}