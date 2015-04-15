using System;
using GoApi;
using GoApi.Core;

namespace Reporting
{
    internal class TrialBalance
    {
        /// <summary>
        ///     List the trial balance as at today
        /// </summary>
        public static void TrialBalanceDemo()
        {
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore")
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = new Go(authorizationSettings);

            // List all accounts in the trial balance as at today
            Console.WriteLine("Trial Balance:");
            
            var trialBalanceLines = api.Reporting.TrialBalance.Get(DateTime.Now);

            foreach (var trialBalanceLine in trialBalanceLines)
                Console.WriteLine(trialBalanceLine.AccountCode + " " + trialBalanceLine.AccountName + " " + trialBalanceLine.Balance + " " + trialBalanceLine.Budget);
        }
    }
}
