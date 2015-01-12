using System;
using System.Linq;
using AuthorizationDemo;
using GoApi.Party;

namespace CustomerDemo
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to test some of the Customer functioniallity available to integration developers.
        /// </summary>
        private static void Main(string[] args)
        {
            // Create a CustomerService instance
            var customerService = new CustomerService(Authorize.TestClientAuthorization());

            // Look up a customer with a given Vat number
            const string vatNumber = "123456789";

            Console.WriteLine("Get Customer with Vat Number: " + vatNumber);

            var customer = customerService.GetAll().FirstOrDefault(c => c.VatNumber == vatNumber);
            if (customer != null)
            {
                Console.WriteLine("Id: " + customer.Id);
                Console.WriteLine("Name: " + customer.Name);
                Console.WriteLine("Vat Number: " + customer.VatNumber);
                Console.WriteLine();
            }

            // Change customer name
            customer.Name = "My Customer AS";

            // Save the customer
            Console.WriteLine("Save");
            customer = customerService.Save(customer);
            if (customer != null)
            {
                Console.WriteLine("Id: " + customer.Id);
                Console.WriteLine("Name: " + customer.Name);
                Console.WriteLine("Vat Number: " + customer.VatNumber);
                Console.WriteLine();
            }

            // Get a list of customers starting with Power (restricted to 50 rows)
            Console.WriteLine("Getting customers with name starting with \"Power\". Max 50");
            var customers = customerService.GetAll().Where(c => c.Name.StartsWith("Power")).Skip(0).Take(50);
            foreach (var customer1 in customers)
            {
                Console.WriteLine(customer1.Name);
            }

            
            // Wait for user input
            Console.ReadKey();
        }
    }
}
