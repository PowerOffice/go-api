using System;

namespace Import
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to demonstrate how to import various journal types
        /// </summary>
        private static void Main(string[] args)
        {
            // Run the payroll import demo
            PayrollImport.TestImport();

            // Wait for user input
            Console.ReadKey();
        }
    }
}
