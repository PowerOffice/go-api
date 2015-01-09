using System;
using ApiAuthorization;
using GoApi.Import;

namespace Import
{
    class PayrollImport
    {
        public static void TestImport()
        {
            // Set up a journal for import
            Console.WriteLine("Creating journal");

            var journal = new Journal
            {
                Description = "Sample Payroll Import",
                Type = JournalType.Payroll,
                Date = DateTime.Now
            };

            // Add journal lines
            Console.WriteLine("Adding journal lines");

            journal.Lines.Add(new JournalLine
            {
                AccountNumber = 1234,
                DocumentDate = DateTime.Now,
                DeparmentNumber = 10,
                ProjectNumber = 1000,
                Quantity = 10,
                Rate = 200,
                Amount = 2000
            });

            journal.Lines.Add(new JournalLine
            {
                AccountNumber = 4321,
                DocumentDate = DateTime.Now,
                DeparmentNumber = 0,
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

            importService.Save(journal);

            Console.WriteLine("Journal was saved and assign the Id: " + journal.Id);

            // The user can post the journal manually from the PowerOffice GO user interface,
            // or the journal can be posted by code.
            importService.Post(journal);
        }
    }
}
