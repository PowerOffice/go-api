using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;
using GoApi.Payroll;

namespace PayrollDemo
{
    public class Program
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
        ///     The purpose of this demo is to show how salary lines that should be paid out over a payroll can be created.
        /// </summary>
        private static async Task RunDemo()
        {
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore"),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = await Go.CreateAsync(authorizationSettings);

            //Query all pay items existing in Go
            var payItems = api.Payroll.PayItem.Get().ToArray();
            Console.WriteLine("Number of pay items: " + payItems.Length);

            //Gets the pay item with code 120
            var hourlyWagePayItem = api.Payroll.PayItem.Get().First(p => p.Code == "120");
            Console.WriteLine(hourlyWagePayItem.Name + " - " + hourlyWagePayItem.Benefit + " - " +
                              hourlyWagePayItem.Description);

            //Creates a salary line for hourly wage
            var salaryLine = new SalaryLine
            {
                EmployeeCode = 64,
                PayItemCode = "120",
                Quantity = 100,
                ProjectCode = "1",
                DepartmentCode = "1",
                Comment = "This is a text that will appear on the payslip"
            };

            //Saves the created line to Go and prints the Id it's assigned
            salaryLine = api.Payroll.SalaryLine.Save(salaryLine);
            Console.WriteLine("ImportedSalaryLine id: " + salaryLine.Id);

            //Gets all the imported salary lines and prints them out
            var importedSalaryLines = api.Payroll.SalaryLine.Get();
            Console.WriteLine();
            Console.WriteLine("Imported salary lines:");
            foreach (var importedSalaryLine in importedSalaryLines)
                Console.WriteLine("Employee: " + importedSalaryLine.EmployeeCode + " - PayItem: " +
                                  importedSalaryLine.PayItemCode + " - Quantity: " + importedSalaryLine.Quantity);

            //Deletes the imported SalaryLine
            api.Payroll.SalaryLine.Delete(salaryLine);

            Console.ReadKey();
        }
    }
}