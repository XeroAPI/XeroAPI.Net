using System;

namespace XeroApi.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to run as a public or private application?");
            Console.WriteLine(" Press 1 for a public application");
            Console.WriteLine(" Press 2 for a private application");
            Console.WriteLine(" Press 3 for a partner application");
            Console.WriteLine(" Press 4 for a public application for payroll (AU only)");
            Console.WriteLine(" Press 5 for a partner application for payroll (AU only)");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();

            if (keyInfo.KeyChar == '1')
            {
                Console.WriteLine("Running as a public application...");
                new CoreApi().Exercise(PublicApplicationRunner.CreateRepository());
            }
            if (keyInfo.KeyChar == '2')
            {
                Console.WriteLine("Running as a private application...");
                new CoreApi().Exercise(PrivateApplicationRunner.CreateRepository());
            }
            if (keyInfo.KeyChar == '3')
            {
                Console.WriteLine("Running as a partner application...");
                new CoreApi().Exercise(PartnerApplicationRunner.CreateRepository());
            }
            if (keyInfo.KeyChar == '4')
            {
                Console.WriteLine("Running as a public application for payroll...");
                new PayrollApi().Exercise(PublicApplicationRunner.CreatePayrollRepository());
            }
            if (keyInfo.KeyChar == '5')
            {
                Console.WriteLine("Running as a partner application for payroll...");
                new PayrollApi().Exercise(PartnerApplicationRunner.CreatePayrollRepository());
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine(" Press Enter to Exit");
            Console.ReadLine();
        }
    }
}
