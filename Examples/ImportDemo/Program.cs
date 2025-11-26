using System;
using System.Threading.Tasks;
using GoApi.Core;

namespace ImportDemo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        /// <summary>
        ///     The purpose of this demo is to demonstrate how to import various journal types
        ///     See each import class for details regarding the various import types.         
        /// </summary>
        /// <remarks>
        ///     FOR VERSION 2.6.0 AND ABOVE:
        ///     The newly created Voucher endpoint is a prefered way of creating vouchers in PowerOffice Go. 
        ///     Take a look at VoucherDemo before thinking about using this demo.
        /// </remarks>
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
