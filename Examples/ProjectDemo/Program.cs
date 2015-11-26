using System;
using System.Linq;
using GoApi;
using GoApi.Core;
using GoApi.Projects;

namespace ProjectDemo
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

                // Create new customer
                Console.WriteLine("Create new project...");
                var myNewProject = new Project
                {
                    Code = "P105",
                    Name = "My new project"
                };
                api.Project.Save(myNewProject);

                Console.WriteLine("Name: " + myNewProject.Name);

                // Get a list of customers starting with Power (restricted to 50 rows)
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