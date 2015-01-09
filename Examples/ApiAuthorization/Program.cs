using System;
using GoApi.Core;

namespace ApiAuthorization
{
    internal class Program
    {
        /// <summary>
        ///     The purpose of this demo is to test if the PowerOffice GO Authentication server will allow us
        ///     to be authorized to access the PowerOffice GO API.
        /// </summary>
        private static void Main(string[] args)
        {
            // Create authorization settings for the test client
            Console.WriteLine("Setting up authorization settings..");
            var authorizationSettings = Authorize.CreateAuthorizationSettings(DemoSettings.TestClientKey);

            // Request authorization from the PowerOffice GO authentication server
            Console.WriteLine("Requesting authorization..");
            var authorization = new Authorization(authorizationSettings);

            // If no exceptions is thrown by now, we have sucessfully been authorized to access the PowerOffice GO API
            Console.WriteLine("Authorization granted!");

            // Wait for user input
            Console.ReadKey();        
        }
    }
}
