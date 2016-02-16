using System;
using System.Linq;
using GoApi;
using GoApi.Core;
using GoApi.Import;
using GoApi.Party;
using GoApi.Products;
using GoApi.SalesOrders;

namespace ImportDemo
{
    public class SalesOrderImport
    {
        /// <summary>
        ///     This is is very simple demo of how to create a sales order import.
        /// </summary>
        public static void SalesOrderImportDemo()
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

            //First we need to make sure that all customers that we're using are created in PowerOffice Go
            //For more a more detailed example on how to do changes to customers, check out the CustomerDemo on https://github.com/PowerOffice/go-api/tree/master/Examples
            UpsertCustomers(api);

            //Then we need to make sure that all products that we're using are created in PowerOffice Go
            //For more a more detailed example on how to do changes to products, check out the ProductDemo on https://github.com/PowerOffice/go-api/tree/master/Examples
            UpsertProducts(api);

            //Now it's time to import the sales orders
            // Create the import object and upload it to the server
            var import = CreateAndSaveImport(api);

            // List un-posted imports
            ListUnpostedImports(api);

            // The user can post the journal manually from the PowerOffice GO user interface,
            // or the journal can be posted by code.
            PostImport(api, import);

            Console.WriteLine("Demo finished! Press any key to exit...");
            Console.ReadKey();
        }

        private static void UpsertCustomers(Go api)
        {
            //Query for all customers in PowerOffice
            var allCustomers = api.Customer.Get();

            //Trying to get customer that is beeing used in this example
            var customer = allCustomers.FirstOrDefault(c => c.Code == 12345);

            //Checking if the customer we're gonna use exist in PowerOffice Go
            if (customer != null)
            {
                //Customer exists already in Go - we can update it if needed
                customer.Name = "Edited customer name";

                Console.WriteLine("Saving changes to customer with customer code " + customer.Code +
                                  " to PowerOffice Go.");
                api.Customer.Save(customer);
            }
            else
            {
                //Customer does not exist in Go, we then create a new one
                customer = new Customer
                {
                    Code = 12345,
                    Name = "Customer 12345",
                    VatNumber = "999999999"
                };

                Console.WriteLine("Saving customer " + customer.Name + " to PowerOffice Go.");
                api.Customer.Save(customer);
            }
        }

        /// <summary>
        ///     Upserts the products.
        /// </summary>
        /// <param name="api">The API.</param>
        private static void UpsertProducts(Go api)
        {
            //Query for all products in PowerOffice
            var allProducts = api.Product.Get();

            //Trying to get product that is used in this example
            var product = allProducts.FirstOrDefault(p => p.Code == "240");

            //Checking if the product i'm gonna use exist in PowerOffice Go
            if (product != null)
            {
                //Product exists already in Go - we can update it then if needed
                product.Name = "Edited testproduct 240";

                Console.WriteLine("Saving changes to product with product code " + product.Code + " to PowerOffice Go.");
                api.Product.Save(product);
            }
            else
            {
                //Product does not exist in Go - we then create it
                product = new Product
                {
                    Code = "240",
                    Name = "Testproduct 240",
                    Description = "Description for testproduct 240",
                    SalesAccount = 3000,
                    VatExemptSalesAccount = 3100,
                    SalesPrice = 250,
                    CostPrice = 100
                };

                Console.WriteLine("Saving product " + product.Name + " to PowerOffice Go.");
                api.Product.Save(product);
            }
        }

        private static void ListUnpostedImports(Go api)
        {
            // Get list of imports
            Console.WriteLine("List of unposted imports:");
            var imports = api.Import.Get().Where(i => i.IsPosted == false);
            foreach (var import in imports)
                Console.WriteLine("{0}: {1} {2} {3}", import.Id, import.Type, import.Description,
                    import.IsPosted ? "[POSTED]" : "");
            Console.WriteLine();
        }

        /// <summary>
        ///     Posts the import.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="import">The import.</param>
        private static void PostImport(Go api, Import import)
        {
            Console.WriteLine("Posting journal...");

            api.Import.Post(import);

            Console.WriteLine("Done...");
        }

        private static Import CreateAndSaveImport(Go api)
        {
            // Set up a journal for import
            Console.WriteLine("Creating Sales Order import");

            // Create the import "header"
            var import = new Import
            {
                Description = "Sample Sales Order Import",
                Date = DateTime.Now
            };

            Console.WriteLine("Adding sales orders");

            //Creating a sales order
            var salesOrder = new SalesOrder(1234, DateTime.Now, 12345)
            {
                Reference = "Sales order reference",
                PaymentTerms = 14,
                Currency = "NOK"
            };

            //Creating a sales order line for 5x product 240 with 10% discount
            var firstOrderLine = new SalesOrderLine("240", 5, 10);
            salesOrder.AddSalesOrderLine(firstOrderLine);

            //Optional: Creating a sales order description line
            var firstDescriptionLine = new SalesOrderLine("This is a description that will appear on the order");
            salesOrder.AddSalesOrderLine(firstDescriptionLine);

            //Adding Sales order to import object
            import.SalesOrders.Add(salesOrder);

            // Save the journal to the server. When this call has finished, the order import will
            // appear in the Journal Import list in PowerOffice Go.
            Console.WriteLine("Saving journal...");

            //Saves the import
            api.Import.Save(import);

            Console.WriteLine("Journal was saved and assign the Id: " + import.Id);
            return import;
        }
    }
}