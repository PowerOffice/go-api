using System;
using System.Linq;
using System.Threading.Tasks;
using GoApi;
using GoApi.Core;
using GoApi.Global;
using GoApi.Projects;

namespace ProjectDemo
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
        ///     The purpose of this demo is to show how projects can be created and queried.
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

                // Create new Project
                Console.WriteLine("Create new project...");
                var myNewProject = new Project
                {
                    Code = "P105",
                    Name = "My new project"
                };
                api.Project.Save(myNewProject);

                Console.WriteLine("Name: " + myNewProject.Name);

                // Get a list of projects starting with My (restricted to 50 rows)
                Console.WriteLine("Getting projects with name starting with \"My\". Max 50...");
                var customers = api.Project.Get().Where(c => c.Name.ToUpper().StartsWith("MY")).Skip(0).Take(50);
                foreach (var customer1 in customers)
                    Console.WriteLine("Name: " + customer1.Name);

                // Look up a project with a given project code
                Console.WriteLine("Get Project with Code Number: " + myNewProject.Code);

                var project = api.Project.Get().FirstOrDefault(c => c.Code == myNewProject.Code);
                if (project != null)
                    Console.WriteLine("Name: " + project.Name);

                // Change project name name
                project.Name = "My Project";

                // Save the project
                Console.WriteLine("Saving...");
                project = api.Project.Save(project);
                if (project != null)
                    Console.WriteLine("Name: " + project.Name);

                // Delete the project
                Console.WriteLine("Deleting...");
                api.Project.Delete(project);

                Console.WriteLine("Done");
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