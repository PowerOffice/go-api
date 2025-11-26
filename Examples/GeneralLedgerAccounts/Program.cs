using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.AccountingSettings;
using GoApi.Core;
using GoApi.Core.Global;

namespace GeneralLedgerAccounts
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        /// <summary>
        /// The purpose of this demo is to create a new General Ledger Account and print out the chart of accounts for a client.
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

            //Add a new General Ledger Account
            var account3016 = new GeneralLedgerAccount()
            {
                Code = 3016,
                Name = "Salgskonto 3016",
                CurrencyCode = "NOK",
                IsActive = true,
                VatCode = "3"
            };

            account3016 = api.GeneralLedgerAccount.Save(account3016);

            //Query all General Ledger Accounts (Chart of Accounts)
            var generalLedgerAccounts = api.GeneralLedgerAccount.Get().OrderBy(a => a.Code).ToArray();

            foreach (var generalLedgerAccount in generalLedgerAccounts)
            {
                Console.WriteLine($"{generalLedgerAccount.Code} - {generalLedgerAccount.Name} - IsActive: {generalLedgerAccount.IsActive}");
            }

            //Delete General Ledger Account
            api.GeneralLedgerAccount.Delete(account3016);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
