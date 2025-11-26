using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Core.Global;
using GoApi.Party;

namespace CustomerDemo
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await RunDemo();
        }

        /// <summary>
        ///     The purpose of this demo is to test some of the Customer functionallity available to PowerOffice Go users.
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

                // Create new customer
                Console.WriteLine("Create new customer...");
                var myNewCustomer = new Customer
                {
                    Name = "My new customer",
                    VatNumber = "999999997"
                };
                api.Customer.Save(myNewCustomer);

                Console.WriteLine("Name: " + myNewCustomer.Name);

                // Get a list of customers starting with Power (restricted to 50 rows)
                Console.WriteLine("Getting customers with name starting with \"My\". Max 50...");
                var customers = api.Customer.Get().Where(c => c.Name.ToUpper().StartsWith("MY")).Skip(0).Take(50);
                foreach (var customer1 in customers)
                    Console.WriteLine("Name: " + customer1.Name);
                
                // Look up a customer with a given Vat number
                Console.WriteLine("Get Customer with Vat Number: " + myNewCustomer.VatNumber);

                var customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == myNewCustomer.VatNumber);
                if (customer != null)
                    Console.WriteLine("Name: " + customer.Name);
                
                // Change customer name
                customer.Name = "My Customer AS";

                // Save the customer
                Console.WriteLine("Saving...");
                customer = api.Customer.Save(customer);
                if (customer != null)
                    Console.WriteLine("Name: " + customer.Name);
                
                // Delete the customer
                Console.WriteLine("Deleting...");
                api.Customer.Delete(customer);

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
    }
}
