﻿using System;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Import;

namespace ImportDemo
{
    public class PayrollImport
    {
        /// <summary>
        ///     This is is very simple demo of how to create a payroll import.
        /// </summary>
        public static async Task PayrollImportDemo()
        {
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicInMemoryTokenStore(),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = await Go.CreateAsync(authorizationSettings);

            // Create the import object and upload it to the server
            var import = CreateAndSaveImport(api);

            // List un-posted imports
            await ImportList.ListUnpostedImports();

            // The user can post the journal manually from the PowerOffice GO user interface,
            // or the journal can be posted by code.
            PostImport(api, import);

            Console.WriteLine("Demo finished! Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates the and save import.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <returns>Import.</returns>
        private static Import CreateAndSaveImport(Go api)
        {
            // Set up a journal for import
            Console.WriteLine("Creating Payroll import");

            // Create the import "header"
            // The clue here is to set the Type to ImportType.Payroll
            var import = new Import
            {
                Description = "Sample Payroll Import",
                Type = ImportType.Payroll,
                Date = DateTime.Now
            };

            // Add dummy payroll lines
            Console.WriteLine("Adding payroll lines");

            import.ImportLines.Add(new ImportLine
            {
                AccountNumber = 1570,
                DocumentNumber = 1,
                DocumentDate = new DateTime(2015, 01, 31),
                PostingDate = new DateTime(2015, 01, 31),
                DepartmentCode = "",
                ProjectCode = "",
                Quantity = 0,
                Amount = 23576
            });

            import.ImportLines.Add(new ImportLine
            {
                AccountNumber = 2600,
                DocumentNumber = 1,
                DocumentDate = new DateTime(2015, 01, 31),
                PostingDate = new DateTime(2015, 01, 31),
                DepartmentCode = "",
                ProjectCode = "",
                Quantity = 0,
                Amount = -23576
            });

            // Save the journal to the server. When this call has finished, the payroll will
            // appear in the Journal Import list in PowerOffice Go.
            Console.WriteLine("Saving journal...");

            api.Import.Save(import);

            Console.WriteLine("Journal was saved and assign the Id: " + import.Id);
            return import;
        }

        /// <summary>
        /// Posts the import.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="import">The import.</param>
        private static void PostImport(Go api, Import import)
        {
            Console.WriteLine("Posting journal...");
            
            api.Import.Post(import);
            
            Console.WriteLine("Done...");
        }
    }
}
