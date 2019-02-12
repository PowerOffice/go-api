using System;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;

namespace Reporting
{
    internal class TrialBalance
    {
        /// <summary>
        ///     List the trial balance as at today
        /// </summary>
        public static async Task TrialBalanceDemo()
        {
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore"),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            // Initialize the PowerOffice Go API and request authorization
            var api = await Go.CreateAsync(authorizationSettings);

            // List all accounts in the trial balance as at today
            Console.WriteLine("Trial Balance:");
            
            var trialBalanceLines = api.Reporting.TrialBalance.Get(DateTime.Now);

            foreach (var trialBalanceLine in trialBalanceLines)
                Console.WriteLine(trialBalanceLine.AccountCode + " " + trialBalanceLine.AccountName + " " + trialBalanceLine.Balance + " " + trialBalanceLine.Budget);
        }
    }
}
