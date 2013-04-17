using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Model.Payroll;

namespace XeroApi.ConsoleApp.Payroll
{
    public static class PayrollPrinter
    {
        public static Employee TestGetEmployees(IEnumerable<Employee> employees)
        {
            // This stops the query happening twice
            var employeeList = employees.ToList();

            Console.WriteLine("There are {0} payroll employees", employeeList.Count());
            foreach (var e in employeeList)
            {
                PrintEmployeeBasic(e);
            }

            return employeeList.FirstOrDefault();
        }

        public static void PrintEmployeeBasic(Employee e)
        {
            var name =
                    System.Text.RegularExpressions.Regex.Replace(
                        string.Format("{0} {1} {2} {3}", e.Title, e.FirstName, e.MiddleNames, e.LastName), @"\s+", " ");

            Console.WriteLine("<--- Employee --->");
            Console.WriteLine("{0} {1}.", e.EmployeeID, name.Trim());
            Console.WriteLine("Started: {0}. They are {1}.", e.StartDate, e.Gender);
            PrintIfNotNullOrEmpty("Phone: {0}", e.Phone);
            PrintIfNotNullOrEmpty("Mobile: {0}", e.Mobile);
            PrintIfNotNullOrEmpty("Email: {0}", e.Email);
            PrintIfNotNullOrEmpty("Twitter: {0}", e.TwitterUserName);
        }

        public static void PrintEmployeeFull(Employee e)
        {
            PrintEmployeeBasic(e);
            PrintEmployeeTheRest(e);
            PrintHomeAddress(e);
            PrintOpeningBalances(e.OpeningBalances);
            PrintBankAccounts(e.BankAccounts);
            PrintSuperfundMemberships(e.SuperMemberships);
            PrintPayTemplate(e.PayTemplate);
        }

        public static void PrintSuperfundMemberships(IEnumerable<SuperMembership> memberships)
        {
            Console.WriteLine("<--- Superfund Memberships --->");
            foreach (var s in memberships)
            {
                Console.WriteLine("<--- Superfund Membership --->");
                Console.WriteLine(s.SuperFundID);
                Console.WriteLine(s.SuperMembershipID);
                Console.WriteLine(s.EmployeeNumber);
            }
        }

        public static void PrintOpeningBalances(OpeningBalances openingBalances)
        {
            Console.WriteLine("<--- Opening Balances --->");
            Console.WriteLine("Opening Balance  Date: {0}", openingBalances.OpeningBalanceDate);
            Console.WriteLine("Tax: {0}", openingBalances.Tax);
            PrintHasLines(openingBalances);
        }

        public static void PrintHasLines(HasLines lines)
        {
            PrintEarningsLines(lines.EarningsLines);
            PrintDeductionLines(lines.DeductionLines);
            PrintLeaveLines(lines.LeaveLines);
            PrintReimbursementLines(lines.ReimbursementLines);
            PrintSuperLines(lines.SuperLines);
        }

        public static void PrintReimbursementLines(IEnumerable<ReimbursementLine> reimbursementLines)
        {
            Console.WriteLine("<--- Reimbursement Lines --->");
            foreach (var r in reimbursementLines)
            {
                Console.WriteLine("<--- Reimbursement Line --->");
                Console.WriteLine("ReimbursementTypeID: {0}", r.ReimbursementTypeID);
                Console.WriteLine("Amount: {0}", r.Amount);
                PrintIfNotNullOrEmpty("Description: {0}", r.Description);
                Console.WriteLine("ExpenseAccount: {0}", r.ExpenseAccount);
            }
        }

        public static void PrintSuperLines(IEnumerable<SuperLine> superLines)
        {
            Console.WriteLine("<--- Super Lines --->");
            foreach (var s in superLines)
            {
                Console.WriteLine("<--- Super Line --->");
                Console.WriteLine("MembershipID: {0}", s.SuperMembershipID);
                Console.WriteLine("Amount: {0}", s.Amount);
                Console.WriteLine("CalculationType: {0}", s.CalculationType);
                Console.WriteLine("ContributionType: {0}", s.ContributionType);
                Console.WriteLine("ExpenseAccountCode: {0}", s.ExpenseAccountCode);
                Console.WriteLine("LiabilityAccountCode: {0}", s.LiabilityAccountCode);
                Console.WriteLine("MinimumMonthlyEarnings: {0}", s.MinimumMonthlyEarnings);
                Console.WriteLine("Percentage: {0}", s.Percentage);
            }
        }

        public static void PrintHomeAddress(Employee employee)
        {
            Console.WriteLine("<--- Home Address --->");
            var a = employee.HomeAddress;

            if (a != null)
            {
                PrintIfNotNullOrEmpty("Street: {0}", a.AddressLine1);
                PrintIfNotNullOrEmpty("        {0}", a.AddressLine2);
                PrintIfNotNullOrEmpty("        {0}", a.AddressLine3);
                PrintIfNotNullOrEmpty("        {0}", a.AddressLine4);
                PrintIfNotNullOrEmpty("City: {0}", a.City);
                Console.WriteLine("Region: {0}", a.Region);
                Console.WriteLine("PostalCode: {0}", a.PostalCode);
                PrintIfNotNullOrEmpty("Country: {0}", a.Country);
            }
        }

        public static void PrintBankAccounts(IEnumerable<BankAccount> accounts)
        {
            Console.WriteLine("<--- Bank Accounts --->");
            foreach (var b in accounts)
            {
                Console.WriteLine("<--- Bank Account --->");
                PrintIfNotNullOrEmpty("Account Name: {0}", b.AccountName);
                Console.WriteLine("Account Number: {0}", b.AccountNumber);
                PrintIfNotNullOrEmpty("Statement: {0}", b.StatementText);
                Console.WriteLine("BSB: {0}", b.BSB);
                Console.WriteLine("Percentage: {0}", b.Percentage);
                Console.WriteLine("Remainder: {0}", b.Remainder);
                Console.WriteLine("Amount: {0}", b.Amount);
            }
        }

        public static void PrintEmployeeTheRest(Employee e)
        {
            Console.WriteLine(e.Classification);
            Console.WriteLine(e.EmployeeGroupName);
            Console.WriteLine(e.Status);
            if (e.TerminationDate.HasValue)
            {
                Console.WriteLine("Left: {0}", e.TerminationDate.Value);
            }
        }

        public static void PrintIfNotNullOrEmpty(string format, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    Console.WriteLine(format, value);
                }
            }
        }

        public static void PrintPayRuns(IEnumerable<PayRun> payRuns)
        {
            Console.WriteLine("<--- Pay Runs --->");
            foreach (var p in payRuns)
            {
                PrintPayRun(p);
            }
        }

        public static void PrintPayRun(PayRun p)
        {
            Console.WriteLine("<-- Pay Run -->");
            Console.WriteLine("PayRunID: {0}", p.PayRunID);
            Console.WriteLine("PayrollCalendarID: {0}", p.PayrollCalendarID);
            Console.WriteLine("PeriodStartDate: {0}", p.PayRunPeriodStartDate);
            Console.WriteLine("PeriodEndDate: {0}", p.PayRunPeriodEndDate);
            Console.WriteLine("Status: {0}", p.PayRunStatus);
            Console.WriteLine("PaymentDate: {0}", p.PaymentDate);
            PrintIfNotNullOrEmpty("Message: {0}", p.PayslipMessage);
            Console.WriteLine("Deductions {0}", p.Deductions);
            Console.WriteLine("Netpay {0}", p.NetPay);
            Console.WriteLine("Reimbursements: {0}", p.Reimbursement);
            Console.WriteLine("Super: {0}", p.Super);
            Console.WriteLine("Tax: {0}", p.Tax);
            Console.WriteLine("Wages: {0}", p.Wages);

            if (p.Payslips != null && p.Payslips.Any())
            {
                PrintPayslips(p.Payslips);
            }
        }

        public static void PrintPayslips(IEnumerable<Payslip> payslips)
        {
            Console.WriteLine("<--- Payslips --->");
            foreach (var p in payslips)
            {
                PrintPayslip(p);
            }
        }

        public static void PrintPayslip(Payslip p)
        {
            Console.WriteLine("<--- Payslip --->");
            Console.WriteLine("PayslipID: {0}", p.PayslipID);
            Console.WriteLine("EmployeeID: {0}", p.EmployeeID);
            Console.WriteLine("FirstName: {0}", p.FirstName);
            Console.WriteLine("LastName: {0}", p.LastName);
            Console.WriteLine("LastEdited: {0}", p.LastEdited);
            Console.WriteLine("Deductions: {0}", p.Deductions);
            Console.WriteLine("NetPay: {0}", p.NetPay);
            Console.WriteLine("Reimbursements: {0}", p.Reimbursements);
            Console.WriteLine("Super: {0}", p.Super);
            Console.WriteLine("Tax: {0}", p.Tax);
            Console.WriteLine("Wages: {0}", p.Wages);

            if (p.DeductionLines != null && p.DeductionLines.Any())
            {
                PrintDeductionLines(p.DeductionLines);
            }

            if (p.EarningsLines != null && p.EarningsLines.Any())
            {
                PrintEarningsLines(p.EarningsLines);
            }

            if (p.LeaveEarningsLines != null && p.LeaveEarningsLines.Any())
            {
                PrintLeaveEarningsLines(p.LeaveEarningsLines);
            }

            if (p.ReimbursementLines != null && p.ReimbursementLines.Any())
            {
                PrintReimbursementLines(p.ReimbursementLines);
            }

            if (p.SuperannuationLines != null && p.SuperannuationLines.Any())
            {
                PrintSuperannuationLines(p.SuperannuationLines);
            }

            if (p.TaxLines != null && p.TaxLines.Any())
            {
                PrintTaxLines(p.TaxLines);
            }

            if (p.TimesheetEarningsLines != null && p.TimesheetEarningsLines.Any())
            {
                PrintTimesheetEarningsLines(p.TimesheetEarningsLines);
            }
        }

        public static void PrintTimesheetEarningsLines(IEnumerable<TimesheetEarningsLine> timesheetEarningsLines)
        {
            Console.WriteLine("<--- Timesheet Earnings Lines --->");
            foreach (var t in timesheetEarningsLines)
            {
                Console.WriteLine("<--- Timesheet Earnings Line --->");
                Console.WriteLine("EarningsRateID: {0}", t.EarningsRateID);
                Console.WriteLine("Amount: {0}", t.Amount);
                Console.WriteLine("RatePerUnit: {0}", t.RatePerUnit);
            }
        }

        public static void PrintTaxLines(IEnumerable<TaxLine> taxLines)
        {
            Console.WriteLine("<--- Tax Lines --->");
            foreach (var t in taxLines)
            {
                Console.WriteLine("<--- Tax Line --->");
                Console.WriteLine("PayslipTaxLineID: {0}", t.PayslipTaxLineID);
                Console.WriteLine("Amount: {0}", t.Amount);
                PrintIfNotNullOrEmpty("Description: {0}", t.Description);
            }
        }

        public static void PrintSuperannuationLines(IEnumerable<SuperannuationLine> superannuationLines)
        {
            Console.WriteLine("<--- Superannuation Lines --->");
            foreach (var s in superannuationLines)
            {
                Console.WriteLine("<--- Superannuation Line --->");
                Console.WriteLine("SuperMembershipID: {0}", s.SuperMembershipID);
                Console.WriteLine("Amount: {0}", s.Amount);
                Console.WriteLine("CalculationType: {0}", s.CalculationType);
                Console.WriteLine("ContributionType: {0}", s.ContributionType);
                Console.WriteLine("ExpenseAccountCode: {0}", s.ExpenseAccountCode);
                Console.WriteLine("LiabilityAccountCode: {0}", s.LiabilityAccountCode);
                Console.WriteLine("PaymentDateForThisPeriod: {0}", s.PaymentDateForThisPeriod);
            }
        }

        public static void PrintLeaveEarningsLines(IEnumerable<LeaveEarningsLine> leaveEarningsLines)
        {
            Console.WriteLine("<--- Leave Earnings Lines --->");
            foreach (var l in leaveEarningsLines)
            {
                Console.WriteLine("<--- Leave Earnings Line --->");
                Console.WriteLine("EarningsRateID: {0}", l.EarningsRateID);
                Console.WriteLine("NumberOfUnits: {0}", l.NumberOfUnits);
                Console.WriteLine("RatePerUnit: {0}", l.RatePerUnit);
            }
        }

        public static void PrintPayTemplate(PayTemplate payTemplate)
        {
            Console.WriteLine("<--- Pay Template --->");
            PrintHasLines(payTemplate);
        }

        public static void PrintLeaveLines(IEnumerable<LeaveLine> leaveLines)
        {
            Console.WriteLine("<--- Leave Lines --->");
            foreach (var l in leaveLines)
            {
                Console.WriteLine("<--- Leave Line --->");
                Console.WriteLine("Annual Number of Units: {0}", l.AnnualNumberOfUnits);
                Console.WriteLine("CalculationType: {0}", l.CalculationType);
                Console.WriteLine("Number of Units: {0}", l.NumberOfUnits);
                Console.WriteLine("TypeID: {0}", l.LeaveTypeID);
                Console.WriteLine("FullTimeNumberOfUnitsPerPeriod: {0}", l.FullTimeNumberOfUnitsPerPeriod);
            }
        }

        public static void PrintDeductionLines(IEnumerable<DeductionLine> deductionLines)
        {
            Console.WriteLine("<--- Deduction Lines --->");
            foreach (var d in deductionLines)
            {
                Console.WriteLine("<--- Deduction Line --->");
                Console.WriteLine("Amount: {0}", d.Amount);
                Console.WriteLine("CalculationType: {0}", d.CalculationType);
                Console.WriteLine("TypeID: {0}", d.DeductionTypeID);
            }
        }

        public static void PrintEarningsLines(IEnumerable<EarningsLine> earningsLines)
        {
            Console.WriteLine("<--- Earning Lines --->");
            foreach (var e in earningsLines)
            {
                Console.WriteLine("<--- Earning Line --->");
                Console.WriteLine("EarningsRateID: {0}", e.EarningsRateID);
                Console.WriteLine("Amount: {0}", e.Amount);
                Console.WriteLine("AnnualSalary: {0}", e.AnnualSalary);
                Console.WriteLine("RatePerUnit: {0}", e.RatePerUnit);
                Console.WriteLine("NormalNumberOfUnits: {0}", e.NormalNumberOfUnits);
                Console.WriteLine("NumberOfUnitsPerWeek: {0}", e.NumberOfUnitsPerWeek);
                Console.WriteLine("NumberOfUnits: {0}", e.NumberOfUnits);
                Console.WriteLine("EarningsRateCalculation: {0}", e.EarningsRateCalculation);
            }
        }

        public static void PrintEarningsRates(IEnumerable<EarningsRate> earningsRate)
        {
            Console.WriteLine("<--- Earning Rates --->");
            foreach (var e in earningsRate)
            {
                Console.WriteLine("<--- Earning Rate --->");
                Console.WriteLine("EarningsRateID: {0}", e.EarningsRateID);
                Console.WriteLine("Name: {0}", e.Name);
                Console.WriteLine("AccountCode: {0}", e.AccountCode);
                Console.WriteLine("EarningsRate: {0}", e.EarningsType);
                Console.WriteLine("RateType: {0}", e.RateType);
                Console.WriteLine("TypeOfUnits: {0}", e.TypeOfUnits);
                Console.WriteLine("IsExemptFromSuper: {0}", e.IsExemptFromSuper);
                Console.WriteLine("IsExemptFromTax: {0}", e.IsExemptFromTax);
                Console.WriteLine("RatePerUnit: {0}", e.RatePerUnit);
                Console.WriteLine("RateMultiplier: {0}", e.Multiplier);
                Console.WriteLine("AccrueLeave: {0}", e.AccrueLeave);
                Console.WriteLine("Amount: {0}", e.Amount);
                Console.WriteLine("UpdatedDateUTC: {0}", e.UpdatedDateUTC);                
            }
        }

        public static void PrintReimbursementTypes(IEnumerable<ReimbursementType> reimbursementTypes)
        {
            Console.WriteLine("<--- Reimbursement Types --->");
            foreach (var r in reimbursementTypes)
            {
                Console.WriteLine("<--- Reimbursement Type --->");
                Console.WriteLine("ReimbursementTypeID: {0}", r.ReimbursementTypeID);
                Console.WriteLine("Name: {0}", r.Name);
                Console.WriteLine("AccountCode: {0}", r.AccountCode);
                Console.WriteLine("UpdatedDateUTC: {0}", r.UpdatedDateUTC);
            }
        }

        public static void PrintLeaveTypes(IEnumerable<LeaveType> leaveTypes)
        {
            Console.WriteLine("<--- Leave Types --->");
            foreach (var l in leaveTypes)
            {
                Console.WriteLine("<--- Leave Type --->");
                Console.WriteLine("LeaveTypeID: {0}", l.LeaveTypeID);
                Console.WriteLine("Name: {0}", l.Name);
                Console.WriteLine("IsPaidLeave: {0}", l.IsPaidLeave);
                Console.WriteLine("NormalEntitlement: {0}", l.IsPaidLeave);
                Console.WriteLine("ShowOnPayslip {0}", l.ShowOnPayslip);
                Console.WriteLine("TypeOfUnits: {0}", l.TypeOfUnits);
                Console.WriteLine("LeaveLoadingRate {0}", l.LeaveLoadingRate);
                Console.WriteLine("UpdatedDateUTC: {0}", l.UpdatedDateUTC);
            }
        }

        public static void PrintDeductionTypes(IEnumerable<DeductionType> deductionTypes)
        {
            Console.WriteLine("<--- Deduction Types --->");
            foreach (var d in deductionTypes)
            {
                Console.WriteLine("<--- Deduction Type --->");
                Console.WriteLine("DeductionTypeID: {0}", d.DeductionTypeID);
                Console.WriteLine("Name: {0}", d.Name);
                Console.WriteLine("AccountCode: {0}", d.AccountCode);
                Console.WriteLine("ReducesSuper: {0}", d.ReducesSuper);
                Console.WriteLine("ReducesTax: {0}", d.ReducesTax);
                Console.WriteLine("UpdatedDateUTC: {0}", d.UpdatedDateUTC);
            }
        }

        public static void PrintPayItems(PayItems payItems)
        {
            Console.WriteLine("<--- Pay Item --->");
            PrintEarningsRates(payItems.EarningsRates);
            PrintDeductionTypes(payItems.DeductionTypes);
            PrintLeaveTypes(payItems.LeaveTypes);
            PrintReimbursementTypes(payItems.ReimbursementTypes);
        }

        public static void PrintLeaveApplication(LeaveApplication application)
        {
            Console.WriteLine("<--- Leave Application --->");
            Console.WriteLine("LeaveApplicationID: {0}", application.LeaveApplicationID);
            Console.WriteLine("Title: {0}", application.Title);
            PrintIfNotNullOrEmpty("Description: {0}", application.Description);
            Console.WriteLine("EmployeeID: {0}", application.EmployeeID);
            Console.WriteLine("LeaveTypeID: {0}", application.LeaveTypeID);
            Console.WriteLine("StartDate: {0}", application.StartDate);
            Console.WriteLine("EndDate: {0}", application.EndDate);
            Console.WriteLine("UpdatedDateUTC: {0}", application.UpdatedDateUTC);

            Console.WriteLine("<--- Leave Periods --->");
            foreach (var l in application.LeavePeriods)
            {
                PrintLeavePeriod(l);
            }
        }

        private static void PrintLeavePeriod(LeavePeriod leavePeriod)
        {
            Console.WriteLine("<--- Leave Period --->");
            Console.WriteLine("LeavePeriodStatus: {0}", leavePeriod.LeavePeriodStatus);
            Console.WriteLine("NumberOfUnits: {0}", leavePeriod.NumberOfUnits);
            Console.WriteLine("PayPeriodStartDate: {0}", leavePeriod.PayPeriodStartDate);
            Console.WriteLine("PayPeriodEndDate: {0}", leavePeriod.PayPeriodEndDate);
        }

        public static void PrintPayrollCalendar(PayrollCalendar payrollCalendar)
        {
            Console.WriteLine("<--- Payroll Calendar --->");
            Console.WriteLine("PayrollCalendarID: {0}", payrollCalendar.PayrollCalendarID);
            Console.WriteLine("Name: {0}", payrollCalendar.Name);
            Console.WriteLine("CalendarType: {0}", payrollCalendar.CalendarType);
            Console.WriteLine("PaymentDate: {0}", payrollCalendar.PaymentDate);
            Console.WriteLine("StartDate: {0}", payrollCalendar.StartDate);
            Console.WriteLine("UpdatedDateUTC: {0}", payrollCalendar.UpdatedDateUTC);
        }
    }
}