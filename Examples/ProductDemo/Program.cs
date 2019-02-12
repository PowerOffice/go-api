using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;
using GoApi.Products;

namespace ProductDemo
{
    internal class Program
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
        ///     The purpose of this demo is to show how the API can update and query products and product groups.
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
                    TokenStore = new BasicTokenStore(@"my.tokenstore"),
                    EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
                };

                // Initialize the PowerOffice Go API and request authorization
                var api = await Go.CreateAsync(authorizationSettings);

                //Create new product group
                Console.WriteLine("Creating product group");
                var productGroup = new ProductGroup
                {
                    Code = "1",
                    Name = "Sandwiches",
                    Description = "Product group for all the sandwiches",
                    Type = ProductType.Product,
                    SalesAccount = 3000,
                    VatExemptSalesAccount = 3100,
                    Unit = "each"
                };

                //Saves the product group
                Console.WriteLine("Saving product group \"" + productGroup.Name + "\".");
                api.ProductGroup.Save(productGroup);

                //Get product group by it's code
                var savedProductGroup = api.ProductGroup.Get().First(pg => pg.Code == "1");

                //Edit the description for a saved product group
                Console.WriteLine("Editing product group: \"" + savedProductGroup.Name + "\".");
                savedProductGroup.Description = "Product group for most sandwiches";
                savedProductGroup.Name = "Footlong sandwiches";
                var editedProductGroup = api.ProductGroup.Save(savedProductGroup);
                Console.WriteLine("Edited description. It is now \"" + editedProductGroup.Description + "\"");
                Console.WriteLine("Edited name. It is now \"" + editedProductGroup.Name + "\"");

                //Create new product
                var product = new Product
                {
                    Code = "1",
                    Description = "One footlong sandwich",
                    Name = "Footlong edited",
                    CostPrice = new decimal(10.5),
                    SalesPrice = new decimal(50),
                    ProductGroupId = savedProductGroup.Id,
                    SalesAccount = 3000,
                    VatExemptSalesAccount = 3100,
                    Type = ProductType.Product,
                    Gtin = "",
                    Unit = "each"
                };

                //Saves the product 
                Console.WriteLine("Saving product \"" + product.Name + "\".");
                api.Product.Save(product);

                //Get all the products
                var allProducts = api.Product.Get();

                //Deletes the with product code 1
                Console.WriteLine("Deleting product with product code \"1\".");
                api.Product.Delete(allProducts.First(p => p.Code == "1"));

                //Deletes the saved product group
                Console.WriteLine("Deleting product group \"" + editedProductGroup.Name + "\".");
                api.ProductGroup.Delete(editedProductGroup);

                Console.WriteLine("Demo finished! Press any key to exit...");
                Console.ReadKey();
            }
            catch (ApiException e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadKey();
            }
        }
    }
}