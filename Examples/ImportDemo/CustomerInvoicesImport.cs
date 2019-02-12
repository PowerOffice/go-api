using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;
using GoApi.Import;

namespace ImportDemo
{
    public class CustomerInvoicesImport
    {
        /// <summary>
        ///     This is is very simple demo of how to create a customer invoices import.
        /// </summary>
        public static async Task CustomerInvoicesImportDemo()
        {
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my1.tokenstore"),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = await Go.CreateAsync(authorizationSettings);

            // Create the import object and upload it to the server
            var import = CreateAndSaveImport(api);

            // List un-posted imports
            ListUnpostedImports(api);

            // The user can post the journal manually from the PowerOffice GO user interface,
            // or the journal can be posted by code.
            PostImport(api, import);

            Console.WriteLine("Demo finished! Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Lists the unposted imports.
        /// </summary>
        /// <param name="api">The API.</param>
        private static void ListUnpostedImports(Go api)
        {
            // Get list of imports
            Console.WriteLine("List of unposted imports:");
            var imports = api.Import.Get().Where(i => i.IsPosted == false);
            foreach (var import in imports)
                Console.WriteLine("{0}: {1} {2} {3}", import.Id, import.Type, import.Description, import.IsPosted ? "[POSTED]" : "");
            Console.WriteLine();
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

        /// <summary>
        /// Creates and save import.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <returns>Import.</returns>
        private static Import CreateAndSaveImport(Go api)
        {
            // Set up a journal for import
            Console.WriteLine("Creating Customer Invoice import");

            // Create the import "header"
            // The clue here is to set the Type to ImportType.OutgoingVoucher
            var import = new Import
            {
                Description = "Sample Customer Invoice Import",
                Type = ImportType.OutgoingVoucher,
                Date = DateTime.Now
            };

            // Add customer invoice lines
            Console.WriteLine("Adding customer invoice lines");

            //First we add a header line for the outgoing invoice
            //Note: Before importing Customer(s) and Product(s) on the import should be created through Api.Customer / Api.Product
            //All lines with the same DocumentNumber will be attached to one invoice/creditnote, there must be one and only one line with CustomerCode (this is identifying that this line is the header)
            var importLine1 = new ImportLine();
            importLine1.Amount = 100.0m;
            importLine1.CurrencyAmount = importLine1.Amount;
            importLine1.CurrencyCode = "NOK";
            importLine1.Description = "test1";
            importLine1.DocumentDate = DateTime.Now;
            importLine1.DocumentNumber = 1;
            importLine1.PostingDate = DateTime.Now;

            //Add important Invoice head information
            importLine1.CustomerCode = 12345;
            importLine1.DueDate = DateTime.Now.AddDays(14);
            importLine1.Cid = "7123451002";
            importLine1.Reference = "Reference here";
            importLine1.InvoiceNo = "100";

            import.ImportLines.Add(importLine1);

            //Then we add a salesline on this customer invoice
            var importLine2 = new ImportLine();
            importLine2.AccountNumber = 3000;
            importLine2.Amount = -100.0m;
            importLine2.VatCode = "3";
            importLine2.CurrencyAmount = importLine2.Amount;
            importLine2.Quantity = 1;
            importLine2.CurrencyCode = "NOK";
            importLine2.Description = "test2";
            importLine2.DocumentDate = DateTime.Now;
            importLine2.DocumentNumber = 1;
            importLine2.PostingDate = DateTime.Now;
            importLine2.ProductCode = "1";

            import.ImportLines.Add(importLine2);

            // Save the journal to the server. When this call has finished, the customer invoice import will
            // appear in the Journal Import list in PowerOffice Go.
            Console.WriteLine("Saving journal...");

            api.Import.Save(import);

            Console.WriteLine("Journal was saved and assign the Id: " + import.Id);
            return import;
        }




    }
}
