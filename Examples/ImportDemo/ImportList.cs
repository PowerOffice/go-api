using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;

namespace ImportDemo
{
    public class ImportList
    {
        /// <summary>
        /// Lists the unposted imports.
        /// </summary>
        public static async Task ListUnpostedImports()
        {
            // Initialize the PowerOffice Go API and request authorization
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore"),
                EndPointHost = Settings.EndPointMode.Production //For authorization against the demo environment - Change this to Settings.EndPointMode.Demo
            };

            var api = await Go.CreateAsync(authorizationSettings);

            // Get list of imports
            Console.WriteLine("List of unposted imports:");
            var imports = api.Import.Get().Where(i => i.IsPosted == false);
            foreach (var import in imports)
                Console.WriteLine("{0}: {1} {2} {3}", import.Id, import.Type, import.Description, import.IsPosted ? "[POSTED]" : "");
            Console.WriteLine();
        }
    }
}
