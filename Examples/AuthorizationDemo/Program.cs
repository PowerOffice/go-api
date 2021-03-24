using System;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Core.Global;

namespace AuthorizationDemo
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
        ///     The purpose of this demo is to test if the PowerOffice Go Authentication server will allow us
        ///     to be authorized to access the PowerOffice GO API.
        /// </summary>
        private static async Task RunDemo()
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
                    TokenStore = new BasicInMemoryTokenStore(),
                    EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
                };

                // Request authorization from the PowerOffice GO authentication server
                Console.WriteLine("Requesting authorization..");
                var api = await Go.CreateAsync(authorizationSettings);

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
