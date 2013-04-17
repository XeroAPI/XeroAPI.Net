using System;
using XeroApi.Model.Payroll.Enums;

namespace XeroApi.Model.Payroll
{
    public class PayrollCalendar : HasUpdatedDate
    {
        public Guid PayrollCalendarID { get; set; }
        public string Name { get; set; }
        public CalendarType CalendarType { get; set; }
        public DateTime? PaymentDate { get; set; }        
        public DateTime? StartDate { get; set; }        
    }

    public class PayrollCalendars : ModelList<PayrollCalendar>
    {
    }
}
