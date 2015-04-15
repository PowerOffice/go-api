using System;
using System.Linq;
using GoApi;
using GoApi.Core;

namespace Reporting
{
    public class AccountTransactions
    {
        public static void AccountTransactionsDemo()
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

            // List all transactions on account 3000 between 2014-01-01 and today
            const int accountNo = 3000;
            var fromDate = new DateTime(2014, 1, 1);
            var toDate = DateTime.Now;

            Console.WriteLine("Account Transactions:");
            var accountTransactions = api.Reporting.AccountTransactions.Get(accountNo, fromDate, toDate).ToList();
            foreach (var transaction in accountTransactions)
                Console.WriteLine(transaction.Date + " " + transaction.Text + " " + transaction.VoucherType + " " + transaction.Amount + " Number of voucher images: " + transaction.VoucherImagesCount);

            // Load image for the first transaction with an attached voucher image
            var transactionWithImages = accountTransactions.FirstOrDefault(t => t.VoucherImagesCount > 0);
            if (transactionWithImages != null)
            {
                Console.WriteLine("Load first image for voucher {0}", transactionWithImages.VoucherNo);

                var imageStream = api.Blob.GetVoucherImage(transactionWithImages.VoucherNo, 1);
                // At this point imageStream contains a jpeg image of the first page of the voucher
            }
        }
    }
}
