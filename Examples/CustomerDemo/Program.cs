using System;
using System.Linq;
using AuthorizationDemo;
using GoApi;
using GoApi.Core;
using GoApi.Party;

namespace CustomerDemo
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to test some of the Customer functioniallity available to PowerOffice Go users.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                // Initialize the PowerOffice Go API and request authorization
                var api = new Go(Authorize.TestClientAuthorization());

                // Create new customer
                Console.WriteLine("Create new customer...");
                var myNewCustomer = new Customer
                {
                    Name = "My new customer",
                    VatNumber = "999999997"
                };
                api.Customer.Save(myNewCustomer);

                PrintCustomer(myNewCustomer);

                // Get a list of customers starting with Power (restricted to 50 rows)
                Console.WriteLine("Getting customers with name starting with \"My\". Max 50...");
                var customers = api.Customer.Get().Where(c => c.Name.ToUpper().StartsWith("MY")).Skip(0).Take(50);
                foreach (var customer1 in customers)
                    PrintCustomer(customer1);

                // Look up a customer with a given Vat number
                Console.WriteLine("Get Customer with Vat Number: " + myNewCustomer.VatNumber);

                var customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == myNewCustomer.VatNumber);
                if (customer != null)
                    PrintCustomer(customer);

                // Change customer name
                customer.Name = "My Customer AS";

                // Save the customer
                Console.WriteLine("Saving...");
                customer = api.Customer.Save(customer);
                if (customer != null)
                    PrintCustomer(customer);

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

        /// <summary>
        ///     Prints customer info to the console.
        /// </summary>
        /// <param name="customer">The customer.</param>
        private static void PrintCustomer(Customer customer)
        {
            Console.WriteLine("Id: " + customer.Id);
            Console.WriteLine("Name: " + customer.Name);
            Console.WriteLine("Vat Number: " + customer.VatNumber);
            Console.WriteLine();
        }
    }
}
