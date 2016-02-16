using System;
using GoApi.Core;

namespace ImportDemo
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to demonstrate how to import various journal types
        ///     See each import class for details regarding the various import types.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                // Run the payroll import demo
                //PayrollImport.PayrollImportDemo();

                //CustomerInvoicesImport.CustomerInvoicesImportDemo();
                //ImportList.ListUnpostedImports();
                SalesOrderImport.SalesOrderImportDemo();
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
