using System;
using System.Linq;
using GoApi;
using GoApi.Core;

namespace ImportDemo
{
    public class ImportList
    {
        public static void ListUnpostedImports()
        {
            // Initialize the PowerOffice Go API and request authorization
            // Set up authorization settings
            var authorizationSettings = new AuthorizationSettings
            {
                ApplicationKey = "<You Application Key Here>",
                ClientKey = "<PowerOffice Go Client Key Here>",
                TokenStore = new BasicTokenStore(@"my.tokenstore")
            };
            
            var api = new Go(authorizationSettings);

            // Get list of imports
            Console.WriteLine("List of unposted imports:");
            var imports = api.Import.Get().Where(i => i.IsPosted == false);
            foreach (var import in imports)
                Console.WriteLine("{0}: {1} {2} {3}", import.Id, import.Type, import.Description, import.IsPosted ? "[POSTED]" : "");
            Console.WriteLine();
        }
    }
}
