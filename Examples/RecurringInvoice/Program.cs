using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Common;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Invoices;


namespace RecurringInvoice
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        /// <summary>
        ///     The purpose of this demo is to show how recurring (repeated) invoices can be created through the API.
        /// </summary>
        private static async Task RunDemo()
        {
            try
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

                //Get all recurring invoices as a list
                Console.WriteLine("Recurring Invoices:");
                var allRecurringInvoices = api.RecurringInvoice.List().ToArray();
                foreach (var recurringInvoiceListItem in allRecurringInvoices)
                {
                    Console.WriteLine($"Active:{recurringInvoiceListItem.IsActive} Cust:{recurringInvoiceListItem.CustomerCode} Net:{recurringInvoiceListItem.NetAmount}, Tot:{recurringInvoiceListItem.TotalAmount}.");
                    Console.WriteLine($"NextDate:{recurringInvoiceListItem.NextInvoiceDate} DaysInAdvance:{recurringInvoiceListItem.DaysInAdvance} Repeat:{recurringInvoiceListItem.SendFrequencyUnit} SendMethod:{recurringInvoiceListItem.SendMethod}");
                }

                Console.WriteLine();

                //Create recurring invoice
                var newRecurringInvoice = new GoApi.Invoices.RecurringInvoice
                {
                    IsActive = false,
                    NextInvoiceDate = new DateTime(2017, 10, 01),
                    DaysInAdvance = 1,
                    RepeatTimes = 12,
                    SendFrequency = 1,
                    SendFrequencyUnit = RecurringSendFrequencyUnit.Monthly,
                    SendMethod = RecurringSendMethod.Confirm,
                    CustomerCode = 100000,
                    ProjectCode = "1",
                    DepartmentCode = "1",
                    OurReferenceEmployeeCode = 1,
                    ContractNo = "Test",
                    PurchaseOrderNo = "Test",
                    CurrencyCode = "NOK"
                };

                newRecurringInvoice.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
                {
                    ProductCode = "1",
                    Description = "Overridden product description",
                    Quantity = 15,
                    UnitOfMeasureCode = UnitOfMeasureCode.EA,
                    UnitPrice = 1250,
                    LineType = VoucherLineType.Normal,
                    SortOrder = 0
                });


                //Save recurring invoice
                newRecurringInvoice = api.RecurringInvoice.Save(newRecurringInvoice);

                //Get recurring invoice by id
                var recurringInvoiceSaved = api.RecurringInvoice.Get(newRecurringInvoice.Id.Value);
                Console.WriteLine("First recurring invoice:");
                Console.WriteLine($"{recurringInvoiceSaved.CustomerCode} Net:{recurringInvoiceSaved.NetAmount}, Tot:{recurringInvoiceSaved.TotalAmount}.");

                //Edit recurring invoice by adding another line, changing the send method and setting it to active
                recurringInvoiceSaved.SendMethod = RecurringSendMethod.Send;
                recurringInvoiceSaved.IsActive = true;
                recurringInvoiceSaved.OutgoingInvoiceLines.Add(new OutgoingInvoiceLine
                {
                    LineType = VoucherLineType.Text,
                    Description = "Additional description",
                    SortOrder = int.MaxValue //want this line to be the last line, therefore setting sort order to int max
                });

                //Post changes
                api.RecurringInvoice.Save(recurringInvoiceSaved);

                //Delete recurring invoice
                api.RecurringInvoice.Delete(recurringInvoiceSaved);
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
