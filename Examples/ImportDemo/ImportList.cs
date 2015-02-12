using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizationDemo;
using GoApi;

namespace ImportDemo
{
    public class ImportList
    {
        public static void ListUnpostedImports()
        {
            // Initialize the PowerOffice Go API and request authorization
            var api = new Go(Authorize.TestClientAuthorization());

            // Get list of imports
            Console.WriteLine("List of unposted imports:");
            var imports = api.Import.Get().Where(i => i.IsPosted == false);
            foreach (var import in imports)
                Console.WriteLine("{0}: {1} {2} {3}", import.Id, import.Type, import.Description, import.IsPosted ? "[POSTED]" : "");
            Console.WriteLine();
        }
    }
}
