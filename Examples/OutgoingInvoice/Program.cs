using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Common;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Invoices;

namespace OutgoingInvoice
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
        ///     The purpose of this demo is to show how to list out invoices and creating invoices ready to be sent from PowerOffice Go
        /// </summary>
        private static async Task RunDemo()
        {
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicInMemoryTokenStore(),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = await Go.CreateAsync(authorizationSettings);

            //Query all invoices with order dat in the last 10 days
            var dateTime10DaysAgo = DateTime.Now.AddDays(-10).Date;
            var invoices = api.OutgoingInvoice.List().Where(i => i.OrderDate > dateTime10DaysAgo).ToArray();

            foreach (var outgoingInvoiceListItem in invoices)
                Console.WriteLine(outgoingInvoiceListItem.CustomerCode + " - " + outgoingInvoiceListItem.TotalAmount);

            //Create a new invoice
            var newInvoice = new GoApi.Invoices.OutgoingInvoice
            {
                CustomerCode = 10000,
                ContractNo = "12345",
                CurrencyCode = "NOK",
                CustomerReference = "Reference",
                DepartmentCode = "1",
                ProjectCode = "1",
                OrderDate = DateTime.Now,
                PurchaseOrderNo = "123",
                ImportedOrderNo = 5555,
                DeliveryDate = DateTime.Now.AddDays(-10)
            };

            newInvoice.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
            {
                LineType = VoucherLineType.Normal,
                ProductCode = "1",
                Description = "Test",
                Quantity = 5,
                UnitOfMeasureCode = UnitOfMeasureCode.EA,
                UnitPrice = new decimal(5000.0),
                ExemptVat = false,
                DiscountPercent = new decimal(0.1),
                SortOrder = 0
            });

            newInvoice.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
            {
                LineType = VoucherLineType.Text,
                Description = "Description line",
                SortOrder = 1
            });

            //Save the invoices, store it's ID that can be used later to edit the invoice
            var newInvoiceId = api.OutgoingInvoice.Save(newInvoice).Id.Value;

            //Get the saved invoice
            newInvoice = api.OutgoingInvoice.Get(newInvoiceId);

            //Add another line to the invoice
            newInvoice.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
            {
                Description = "Test",
                DiscountPercent = new decimal(0.5),
                ExemptVat = false,
                LineType = VoucherLineType.Normal,
                Quantity = 5,
                ProductCode = "1",
                SortOrder = 2
            });

            //Set the delivery address on the invoice (this id can be queried from Customer service)
            newInvoice.DeliveryAddressId = 17506;

            //Save the invoice
            api.OutgoingInvoice.Save(newInvoice);

            //Delete the invoice
            api.OutgoingInvoice.Delete(newInvoice);

            Console.ReadKey();
        }
    }
}