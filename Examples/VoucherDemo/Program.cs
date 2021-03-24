using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Voucher;

namespace VoucherDemo
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
        ///     The purpose of this demo is to show how different vouchers can be created through the API.
        ///     There is one method for each of the Voucher Types supported by this service.
        ///     All methods will create a voucher, then create a reversal voucher for the previous voucher.
        ///     Reversal Vouchers can be used to reverse the accounting entries that the voucher that is reversed created.
        ///     This is a new feature from version 2.6.0 and is the preferred way to import vouchers to PowerOffice Go.
        /// </summary>
        private static async Task RunDemo()
        {
            try
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

                //Prints out the client we're working on
                var currentClient = api.Client.Get();
                Console.WriteLine("Client:");
                Console.WriteLine($"{currentClient.Name}, LockDate: {currentClient.LockDate}");
                Console.WriteLine();

                Console.WriteLine("Creating outgoing invoice:");
                CreateOutgoingInvoiceAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating incoming invoice:");
                CreateIncomingInvoiceAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating expense:");
                CreateExpenseAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating manual voucher:");
                CreateManualVoucherAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating year end voucher:");
                CreateYearEndVoucherAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating payroll journal:");
                CreatePayrollJournalAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating cash journal:");
                CreateCashJournalAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Creating bank journal:");
                CreateBankJournalAndReverse(api);
                Console.WriteLine();

                Console.WriteLine("Done");
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            // Wait for user input
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }

        private static void CreateBankJournalAndReverse(Go api)
        {
            var apiBankJournal = new BankJournalVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                Description = "Testbilag",
                Lines = new List<BankJournalVoucherLine>
                {
                    new BankJournalVoucherLine
                    {
                        AccountCode = 10000,
                        Amount = -50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Kundekonto"
                    },
                    new BankJournalVoucherLine
                    {
                        AccountCode = 1920,
                        Amount = 50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "kontantlinje"
                    }
                }
            };

            //Saves and posts a Bank Journal to PowerOffice Go
            var bankJournal = api.Voucher.BankJournal.Save(apiBankJournal);
            Console.WriteLine($"Bank Journal: {bankJournal.Id} - Is Bank Journal Reversed ? {bankJournal.IsReversed}");

            //Reverses the newly created Bank Journal. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {bankJournal.Id}");
            var isReversed = api.Voucher.BankJournal.Reverse(bankJournal.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            bankJournal = api.Voucher.BankJournal.Get(bankJournal.Id.Value);
            Console.WriteLine($"Is Bank Journal reversed after reverse ? {bankJournal.IsReversed}");
        }

        private static void CreateCashJournalAndReverse(Go api)
        {
            var apiCashJournal = new CashJournalVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                Description = "Testbilag",
                Lines = new List<CashJournalVoucherLine>
                {
                    new CashJournalVoucherLine
                    {
                        AccountCode = 3000,
                        Amount = -50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Salgslinje",
                        VatCode = "3"
                    },
                    new CashJournalVoucherLine
                    {
                        AccountCode = 1920,
                        Amount = 50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "kontantlinje"
                    }
                }
            };

            //Saves and posts a Cash Journal to PowerOffice Go
            var cashJournal = api.Voucher.CashJournal.Save(apiCashJournal);
            Console.WriteLine($"Cash Journal: {cashJournal.Id} - Is Cash Journal Reversed ? {cashJournal.IsReversed}");

            //Reverses the newly created Cash Journal. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {cashJournal.Id}");
            var isReversed = api.Voucher.CashJournal.Reverse(cashJournal.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            cashJournal = api.Voucher.CashJournal.Get(cashJournal.Id.Value);
            Console.WriteLine($"Is Cash Journal reversed after reverse ? {cashJournal.IsReversed}");
        }

        private static void CreatePayrollJournalAndReverse(Go api)
        {
            var apiPayrollJournal = new PayrollJournalVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                Description = "Testbilag",
                Lines = new List<PayrollJournalVoucherLine>
                {
                    new PayrollJournalVoucherLine
                    {
                        AccountCode = 5000,
                        Amount = 50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Lønnslinje"
                    },
                    new PayrollJournalVoucherLine
                    {
                        AccountCode = 1920,
                        Amount = -50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Bank"
                    }
                }
            };
            //Saves and posts a Payroll Journal to PowerOffice Go
            var payroll = api.Voucher.PayrollJournal.Save(apiPayrollJournal);
            Console.WriteLine($"Payroll Voucher: {payroll.Id} - Is Payroll Voucher Reversed ? {payroll.IsReversed}");

            //Reverses the newly created Payroll Journal. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {payroll.Id}");
            var isReversed = api.Voucher.PayrollJournal.Reverse(payroll.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            payroll = api.Voucher.PayrollJournal.Get(payroll.Id.Value);
            Console.WriteLine($"Is Payroll Voucher reversed after reverse ? {payroll.IsReversed}");

        }

        private static void CreateYearEndVoucherAndReverse(Go api)
        {
            var apiYearEndJournal = new YearEndJournalVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = new DateTime(2018, 12, 31),
                Description = "Testbilag",
                Lines = new List<YearEndJournalVoucherLine>
                {
                    new YearEndJournalVoucherLine
                    {
                        AccountCode = 1000,
                        Amount = 50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "eiendelskonto"
                    },
                    new YearEndJournalVoucherLine
                    {
                        AccountCode = 2000,
                        Amount = -50000,
                        CID = null,
                        CurrencyCode = "NOK",
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "ekkonto"
                    }
                }
            };

            //Saves and posts a Year End Journal to PowerOffice Go
            var yearEndVoucher = api.Voucher.YearEndJournal.Save(apiYearEndJournal);
            Console.WriteLine($"Year end voucher: {yearEndVoucher.Id} - Is year end voucher Reversed ? {yearEndVoucher.IsReversed}");

            //Reverses the newly created Year End Journal. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {yearEndVoucher.Id}");
            var isReversed = api.Voucher.YearEndJournal.Reverse(yearEndVoucher.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            yearEndVoucher = api.Voucher.YearEndJournal.Get(yearEndVoucher.Id.Value);
            Console.WriteLine($"Is Year end voucher reversed after reverse ? {yearEndVoucher.IsReversed}");

        }

        private static void CreateManualVoucherAndReverse(Go api)
        {

            var apiManualJournal = new ManualJournalVoucher
            {
                CurrencyCode = "NOK",
                VoucherDate = DateTime.Today,
                Description = "Testbilag",
                Lines = new List<ManualJournalVoucherLine>
                {
                    new ManualJournalVoucherLine
                    {
                        AccountCode = 3000,
                        Amount = -50000,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        Description = "Salgslinje",
                        VatCode = "3"
                    },
                    new ManualJournalVoucherLine
                    {
                        AccountCode = 1920,
                        Amount = 50000,
                        CurrencyCode = "NOK",
                        Date = DateTime.Today,
                        Description = "kontantlinje"
                    }
                }
            };

            //Saves and posts a Manual Journal to PowerOffice Go
            var manualVoucher = api.Voucher.ManualJournal.Save(apiManualJournal);
            Console.WriteLine($"Manual Journal: {manualVoucher.Id} - Is manual journal Reversed ? {manualVoucher.IsReversed}");

            //Reverses the newly created Manual Journal. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {manualVoucher.Id}");
            var isReversed = api.Voucher.ManualJournal.Reverse(manualVoucher.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            manualVoucher = api.Voucher.ManualJournal.Get(manualVoucher.Id.Value);
            Console.WriteLine($"Is Manual Journal reversed after reverse ? {manualVoucher.IsReversed}");
        }

        private static void CreateExpenseAndReverse(Go api)
        {
            var apiExpenseVoucher = new ExpenseVoucher()
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(10),
                Description = "Testbilag",
                EmployeeCode = 1,
                EmployeeBankAccountCode = "12061614271"

            };

            apiExpenseVoucher.Lines = new List<ExpenseVoucherLine>
            {
                new ExpenseVoucherLine
                {
                    AccountCode = 4000,
                    Amount = 50000,
                    DepartmentCode = null,
                    ProjectCode = null,
                    ProductCode = null,
                    Description = "Noe greier",
                    VatCode = "1"
                },
                new ExpenseVoucherLine
                {
                    AccountCode = 4000,
                    Amount = 50000,
                    DepartmentCode = null,
                    ProjectCode = null,
                    ProductCode = null,
                    Description = "Mere greier"
                }
            };

            //Saves and posts a Expense to PowerOffice Go
            var expense = api.Voucher.Expense.Save(apiExpenseVoucher);
            Console.WriteLine($"Expense: {expense.Id} - Is Expense Reversed ? {expense.IsReversed}");

            //Reverses the newly created Expense. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {expense.Id}");
            var isReversed = api.Voucher.Expense.Reverse(expense.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            expense = api.Voucher.Expense.Get(expense.Id.Value);
            Console.WriteLine($"Is expense reversed after reverse ? {expense.IsReversed}");
        }

        private static void CreateIncomingInvoiceAndReverse(Go api)
        {
            var apiIncInvoice = new IncomingInvoiceVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(10),
                Cid = "1100123000017",
                SupplierCode = 20013,
                SupplierBankAccountCode = "12061614271",
                InvoiceNo = new Random().Next(1, Int32.MaxValue).ToString(),
                Lines = new List<IncomingInvoiceVoucherLine>
                {
                    new IncomingInvoiceVoucherLine
                    {
                        AccountCode = 4000,
                        Amount = 50000,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Kjøpslinje",
                        VatCode = "1"
                    },
                    new IncomingInvoiceVoucherLine
                    {
                        AccountCode = 4100,
                        Amount = 50000,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = null,
                        Description = "Noe greier"
                    }
                }
            };

            //Saves and posts a Incoming Invoice to PowerOffice Go
            var invoice = api.Voucher.IncomingInvoice.Save(apiIncInvoice);
            Console.WriteLine($"InvoiceId: {invoice.Id} - Is Invoice Reversed ? {invoice.IsReversed}");

            //Reverses the newly created Incoming Invoice. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {invoice.Id}");
            var isReversed = api.Voucher.IncomingInvoice.Reverse(invoice.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            invoice = api.Voucher.IncomingInvoice.Get(invoice.Id.Value);
            Console.WriteLine($"Is invoice reversed after reverse ? {invoice.IsReversed}");
        }

        private static void CreateOutgoingInvoiceAndReverse(Go api)
        {
            var apiOutgoingInvoice = new OutgoingInvoiceVoucher
            {
                CurrencyCode = "NOK",
                DepartmentCode = "123",
                ProjectCode = "122",
                VoucherDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                Cid = "1100123000017",
                CustomerCode = 10013,
                InvoiceNo = new Random().Next(1, Int32.MaxValue),
                PurchaseOrderNo = "Purchase",
                ContractNo = "Contract",
                DeliveryTerm = "CFR",
                CustomerReference = "Klaus",
                DeliveryDate = DateTime.Today.AddDays(1),
                OurReferenceEmployeeCode = 12,
                Lines = new List<OutgoingInvoiceVoucherLine>
                {
                    new OutgoingInvoiceVoucherLine
                    {
                        AccountCode = 3000,
                        Amount = 50000,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = "1234",
                        Description = "Med mva",
                        VatCode = "3",
                        Quantity = 1
                    },
                    new OutgoingInvoiceVoucherLine
                    {
                        AccountCode = 3900,
                        Amount = 50000,
                        DepartmentCode = null,
                        ProjectCode = null,
                        ProductCode = "1234",
                        Description = "Uten mva",
                        Quantity = 10,
                        UnitCostPrice = 10,
                        Unit = "BX",
                        DiscountPercent = 5,
                    }
                }
            };

            //Saves and posts a Outgoing Invoice to PowerOffice Go
            var invoice = api.Voucher.OutgoingInvoice.Save(apiOutgoingInvoice);
            Console.WriteLine($"InvoiceId: {invoice.Id} - Is Invoice Reversed ? {invoice.IsReversed}");

            //Reverses the newly created Outgoing Invoice. This will reverse all accounting entries created by the previous voucher.
            Console.WriteLine($"Reversing voucher with id: {invoice.Id}");
            var isReversed = api.Voucher.OutgoingInvoice.Reverse(invoice.Id.Value).Success;
            Console.WriteLine($"Was reversal request a success ? {isReversed}");

            invoice = api.Voucher.OutgoingInvoice.Get(invoice.Id.Value);
            Console.WriteLine($"Is invoice reversed after reverse ? {invoice.IsReversed}");

        }
    }
}
