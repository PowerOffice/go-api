using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;
using GoApi.JournalEntry;

namespace JournalEntryDemo
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
        /// The purpose of this demo is to show how Journal Entry Vouchers can be created with suggested voucher lines.
        /// Journal Entry Vouchers are vouchers that is unposted and will appear in Journal Entry. 
        /// Users in PowerOffice Go will then verify the suggested voucherlines, perhaps edit it some and then the voucher.
        /// </summary>
        static async Task RunDemo()
        {
            try
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


                //Queries and prints out all previously created Journal Entry Vouchers
                var vouchers = api.JournalEntryVoucher.Get().ToList();
                foreach (var voucher in vouchers)
                {
                    Console.WriteLine($"{voucher.VoucherDate} {voucher.SupplierCode} {voucher.TotalAmount}");

                    foreach (var journalEntryVoucherLine in voucher.VoucherLines)
                    {
                        Console.WriteLine($"{journalEntryVoucherLine.Date} {journalEntryVoucherLine.Description} {journalEntryVoucherLine.DebitAccountCode} {journalEntryVoucherLine.CreditAccountCode} {journalEntryVoucherLine.Amount}");
                    }

                    Console.WriteLine();
                }

                //Reads a file from disk and converts it to a Base64 string.
                byte[] bytes = File.ReadAllBytes("C:\\temp\\Test.pdf");
                string file = Convert.ToBase64String(bytes);

                //Creates a journal entry voucher and saves it Go.
                var journalEntryVoucher = new JournalEntryVoucher
                {
                    VoucherType = JournalEntryVoucherType.ManualJournal,
                    Description = "Test manuelt kassesalg ellerno",
                    VoucherDate = DateTime.Today,
                    CurrencyCode = "NOK",
                    VoucherLines = new[]
                    {
                    new JournalEntryVoucherLine()
                    {
                        Date = DateTime.Today,
                        DebitAccountCode = 1920,
                        DebitVatCode = null,
                        CreditAccountCode = 3000,
                        CreditVatCode = "3",
                        CurrencyCode = "NOK",
                        Amount = 150000,
                        DepartmentCode = null,
                        ProjectCode = null,
                        Description = "Solgte noe for noe til noen..."
                    }
                },
                    File = new JournalEntryFile()
                    {
                        FileName = "Test.pdf",
                        FileType = JournalEntryFileType.Pdf,
                        Base64EncodedData = file
                    }
                };

                //Saves the Journal Entry Voucher
                var result = api.JournalEntryVoucher.Save(journalEntryVoucher);

                Console.WriteLine($"Journal entry voucher created with id: {result.Id}.");
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
