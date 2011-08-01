using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();

            if (keyInfo.KeyChar == '1')
            {
                Console.WriteLine("Running as a public application...");
                PublicApplicationRunner.Run();
            }
            if (keyInfo.KeyChar == '2')
            {
                Console.WriteLine("Running as a private application...");
                PrivateApplicationRunner.Run();
            }
            if (keyInfo.KeyChar == '3')
            {
                Console.WriteLine("Running as a partner application...");
                PartnerApplicationRunner.Run();
            }

            Console.WriteLine("");
            Console.WriteLine(" Press Enter to Exit");
            Console.ReadLine();
        }
    }
}
