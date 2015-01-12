using System;
using AuthorizationDemo;
using GoApi.Import;

namespace ImportDemo
{
    class PayrollImport
    {
        public static void TestImport()
        {
            // Set up a journal for import
            Console.WriteLine("Creating import");

            var import = new Import
            {
                Description = "Sample Payroll Import",
                Type = ImportType.Payroll,
                Date = DateTime.Now
            };

            // Add payroll lines
            Console.WriteLine("Adding payroll lines");

            import.ImportLines.Add(new ImportLine
            {
                AccountNumber = 1234,
                DocumentDate = DateTime.Now,
                DepartmentNumber = 10,
                ProjectNumber = 1000,
                Quantity = 10,
                Rate = 200,
                Amount = 2000
            });

            import.ImportLines.Add(new ImportLine
            {
                AccountNumber = 4321,
                DocumentDate = DateTime.Now,
                DepartmentNumber = 0,
                ProjectNumber = 0,
                Quantity = 10,
                Rate = -200,
                Amount = -2000
            });

            // Create an instance of the ImportService
            var importService = new ImportService(Authorize.TestClientAuthorization());

            // Save the journal to the server. When this call has finished, the payroll will
            // appear in the Journal Import list in PowerOffice GO.
            Console.WriteLine("Saving journal...");

            importService.Save(import);

            Console.WriteLine("Journal was saved and assign the Id: " + import.Id);

            // The user can post the journal manually from the PowerOffice GO user interface,
            // or the journal can be posted by code.
            importService.Post(import);
        }
    }
}
