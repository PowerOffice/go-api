using System;
using GoApi.Core;

namespace Reporting
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Run the payroll import demo
                TrialBalance.TrialBalanceDemo();
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            // Wait for user input
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
