using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Reporting.Ledger;

namespace Reporting
{
    public class LedgerReportsAndMatching
    {
        /// <summary>
        /// This demo shows how the customer ledger can be printed out, matched and unmatched.
        /// The same code can run for supplier ledger by changing api.Reporting.CustomerLedger to api.Reporting.SupplierLedger.
        /// </summary>
        public static async Task CustomerLedger()
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

            Console.WriteLine("Customer ledger:");
            PrintStatement(api);
            //PrintOpenItems(api);

            //Create a match request with all IDs that you want matched or unmatched.
            var matchRequest = new MatchRequest(DateTime.Today, new long[] {2612317, 2890048});

            //Request to clear matches
            api.Reporting.CustomerLedger.UnmatchEntries(matchRequest);

            //Request to match entries together
            api.Reporting.CustomerLedger.MatchEntries(matchRequest);

            //NOTE: The same API methods also exists on api.Reporting.SupplierLedger
        }

        /// <summary>
        ///     Prints the statement for past 14 days - this includes all customer ledger transactions.
        /// </summary>
        /// <param name="api">The API.</param>
        private static void PrintStatement(Go api)
        {
            var customerLedgerLines =
                api.Reporting.CustomerLedger.GetStatement(DateTime.Today.AddDays(-14), DateTime.Today).ToList();
            foreach (var customerLedgerEntry in customerLedgerLines)
            {
                Console.WriteLine(customerLedgerEntry.Id + " | " + customerLedgerEntry.Customer.Name + "(" +
                                  customerLedgerEntry.Customer.Code + ")" + " | " + customerLedgerEntry.Amount + " | " +
                                  customerLedgerEntry.Balance + " | " +
                                  customerLedgerEntry.PostingDate.ToString("dd.MM.yyyy") + " | " +
                                  customerLedgerEntry.MatchId);
            }
        }

        /// <summary>
        ///     Prints all open items (all transactions on customer ledger that haven't been matched out
        /// </summary>
        /// <param name="api">The API.</param>
        private static void PrintOpenItems(Go api)
        {
            var customerLedgerLines = api.Reporting.CustomerLedger.GetOpenItems(DateTime.Today).ToList();
            foreach (var customerLedgerEntry in customerLedgerLines)
            {
                Console.WriteLine(customerLedgerEntry.Id + " | " + customerLedgerEntry.Customer.Name + "(" +
                                  customerLedgerEntry.Customer.Code + ")" + " | " + customerLedgerEntry.Amount + " | " +
                                  customerLedgerEntry.Balance + " | " +
                                  customerLedgerEntry.PostingDate.ToString("dd.MM.yyyy") + " | " +
                                  customerLedgerEntry.MatchId);
            }
        }
    }
}