using System;
using System.Threading.Tasks;
using GoApi.Core;

namespace Reporting
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        private static async Task RunDemo()
        {
            try
            {
                // Run the payroll import demo
                await TrialBalance.TrialBalanceDemo();
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            try
            {
                await AccountTransactions.AccountTransactionsDemo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);

            }

            // Wait for user input
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
