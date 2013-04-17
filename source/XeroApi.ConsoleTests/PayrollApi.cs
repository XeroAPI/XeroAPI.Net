using System.Linq;
using XeroApi.ConsoleApp.Payroll;
using XeroApi.Model.Payroll;

namespace XeroApi.ConsoleApp
{
    public class PayrollApi
    {
        public void Exercise(PayrollRepository repository)
        {
            if (repository == null)
            {
                return;
            }

            Employee employee = repository.Employees.FirstOrDefault();

            var read = new PayrollApiRead();
            read.Exercise(repository, employee);

            var write = new PayrollApiWrite();
            write.Exercise(repository, employee);
        }
    }
}
