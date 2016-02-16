using System;
using System.Linq;
using GoApi;
using GoApi.Core;
using GoApi.Products;

namespace ProductDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
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