using System;
using System.Linq;
using GoApi;
using GoApi.Core;
using GoApi.Payroll;

namespace PayrollDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore")
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = new Go(authorizationSettings);

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