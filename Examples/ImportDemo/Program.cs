using System;
using System.Threading.Tasks;
using GoApi.Core;

namespace ImportDemo
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


        /// <summary>
        ///     The purpose of this demo is to demonstrate how to import various journal types
        ///     See each import class for details regarding the various import types.
        /// </summary>
        private static async Task RunDemo()
        {
            try
            {
                // Run the payroll import demo
                await PayrollImport.PayrollImportDemo();
                await CustomerInvoicesImport.CustomerInvoicesImportDemo();
                await ImportList.ListUnpostedImports();
                await SalesOrderImport.SalesOrderImportDemo();
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
