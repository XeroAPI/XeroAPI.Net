using System;
using System.Linq;
using XeroApi.Model.Payroll;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.ConsoleApp.Payroll
{
    public class PayrollApiWrite
    {
        public void Exercise(PayrollRepository payrollRepository, Employee employee)
        {
            var calendar = payrollRepository.PayrollCalendars.FirstOrDefault(p => p.CalendarType == CalendarType.WEEKLY);
            var earnings = payrollRepository.PayItems.EarningsRates.FirstOrDefault(p => p.EarningsType == EarningsType.ORDINARYTIMEEARNINGS);
            
            var newFullEmployee = CreateFullNewEmployee(payrollRepository, calendar.PayrollCalendarID, earnings.EarningsRateID);
            
            //CreateNewTimesheet(payrollRepository, newFullEmployee);
        }

        private void CreateNewTimesheet(PayrollRepository payrollRepository, Employee newEmployee)
        {
            var timesheet = new Timesheet
            {
                EmployeeID = newEmployee.EmployeeID,
                Hours = 7.5M,
                StartDate = DateTime.UtcNow.AddDays(7),
                EndDate = DateTime.UtcNow,
            };

            payrollRepository.UpdateOrCreate(timesheet);
        }

        private Employee CreateSimpleNewEmployee(PayrollRepository payrollRepository)
        {
            if (payrollRepository == null)
            {
                throw new ArgumentNullException("payrollRepository");
            }

            var employee = new Employee
            {
                FirstName = "Zowie",
                LastName = "Bowie"
            };

            return payrollRepository.UpdateOrCreate(employee);
        }

        private Employee CreateFullNewEmployee(PayrollRepository payrollRepository, Guid payrollCalanderId, Guid ordinaryEarningRateId)
        {
            if (payrollRepository == null)
            {
                throw new ArgumentNullException("payrollRepository");
            }

            Console.WriteLine("<-- Create Employee -->");

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Silver",
                Title = "Long",
                Classification = "Manager",
                DateOfBirth = new DateTime(1970, 3, 28),
                Email = "johnsilver@treasure.island.com",
                Gender = "M",
                Mobile = "022 222 2222",
                Phone = "04 444 4444",
                TwitterUserName = "@longjohnsilver",
                StartDate = new DateTime(2012, 1, 23),
                PayrollCalendarID = payrollCalanderId,
                OrdinaryEarningsRateID = ordinaryEarningRateId,
                
                BankAccounts = new BankAccounts
                {
                    new BankAccount
                    {
                        AccountName = "My Money",
                        AccountNumber = 111111111,
                        BSB = 123456,
                        StatementText = "Salary",
                        Remainder = true
                    }
                },

                HomeAddress = new HomeAddress
                {
                    AddressLine1 = "12 Main Street",
                    AddressLine2 = "A leafy suburb",
                    City = "Boom Town",
                    Region = State.QLD,
                    PostalCode = 1234
                }
            };
            
            Employee newEmployee = payrollRepository.UpdateOrCreate(employee);

            Console.WriteLine("<-- Created Employee -->");
            PayrollPrinter.PrintEmployeeBasic(newEmployee);

            return newEmployee;
        }
    }
}
