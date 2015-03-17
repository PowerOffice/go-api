using System;
using GoApi;
using GoApi.Core;

namespace AuthorizationDemo
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to test if the PowerOffice Go Authentication server will allow us
        ///     to be authorized to access the PowerOffice GO API.
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                // Create authorization settings for the test client
                Console.WriteLine("Setting up authorization settings..");
                // Set up authorization settings
                var authorizationSettings = new AuthorizationSettings
                {
                    ApplicationKey = "<You Application Key Here>",
                    ClientKey = "<PowerOffice Go Client Key Here>",
                    TokenStore = new BasicTokenStore(@"my.tokenstore")
                };

                // Request authorization from the PowerOffice GO authentication server
                Console.WriteLine("Requesting authorization..");
                var api = new Go(authorizationSettings);

                // If no exceptions is thrown by now, we have sucessfully been authorized to access the PowerOffice GO API
                Console.WriteLine("Authorization granted!");
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
