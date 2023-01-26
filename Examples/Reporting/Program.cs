using System;
using System.Threading.Tasks;
using GoApi.Core;

namespace Reporting
{
    internal class Program
    {
        // Main method for C# 7.1 and above
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        //// Main method for C# 7.0 and below. Be careful with Wait, you can get deadlocks. It is highly recommended to update to C# 7.1 for console applications.
        //// To change the C# version, open the project properties, go to Build, then click the Advanced button in the bottom right, and select your C# version.
        //// If you can't find an appropriate C# version you may need to update your Visual Studio.
        //public static void Main(string[] args)
        //{
        //    RunDemo().Wait();
        //}

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
