Getting Started - Tutorial
==========================

After installing the SDK and setting up the sandbox environment, the next thing to do is to create a simple test project with the basics of an integration.

This tutorial walks you through your first test project.

The resulting project can be found in the SDK examples folder: Exampels/Test1/

# Create the project in Visual Studio

TODO: Describe and screenshot New project, Console application, .NET Framework 4.5.1

# Add the PowerOffice Go SDK NuGet package

The first thing we must do is to add the PowerOffice Go SDK NuGet package:

[Instructions on how to install the PowerOffice Go SDK NuGet Package](../NuGetPackage.md)
 
# Add our first code file

For this example, we will put all our code in one file.

1. In the *Solution Explorer*, Right click the project item
2. Select *Add->New Item*
3. Select the *Visual C# Items->Class*-template
4. Set the name to *Program.cs*
5. Click the *Add* button

# Writing our first code (setting up authentication)

First we must add a using statement for the Go.Api and Go.Api.Core namespaces at the top of the file

```csharp
using GoApi;
using GoApi.Core;
```

Now we must set up our authorization settings. 

Here we provide the *application key* and the *client key* (see [Registration and Client activation](../../Registration.md) for more info).

A *token store* must also be provided. The token store is responsible for persisting the access key and refresh key obtained from the authorisation server (see [Authentication, Technical details](../../Details/Authentication.md) for technical details).

The Go-Api provides a basic token store that will create an encrypted file that will hold the needed details. If you at some point need to control the token store it is possible to derive the basic store or implement the ITokenStore interface. 
 

```csharp
class Program
{
    static void Main(string[] args)
    {
		var applicationKey = "AA70F6AD-1QQE-4EBF-9DA7-510962CE7E46";
		var clientKey = "1970F6AD-E35E-4EBF-9DA7-510962CE7E46";
		
		var authorizationSettings = new AuthorizationSettings
		{
		    ApplicationKey = applicationKey,
		    ClientKey = clientKey,
		    TokenStore = new BasicTokenStore("my.tokenstore")
		};

		var authorization = new Authorization(authorizationSettings);
    }
}
```

Now we have set up the authorization. The next thing we need is to obtain an instance of the Go API.

# Instanciating the API

To obtain an instance of the Go API we must instanciate the Go class, and provide it with the authorization object we have created in the step above.

Add the following code to the end of the Main method:

```csharp
	var api = new Go(authorization);
```

If you try to run the program it will connect to the authorization server and request an access key that will be used by all calls to the API functions. Lucikly you don't have to care anymore about this, as the access key handling and refreshing is done behind the scenes by the SDK. 

# Calling an API function (create a new customer)

The next thing we're going to do is to create a a customer.

To access customers services, we need to add a using statement for the Go.Api.Party namespace. Add the following to the top of the file.

```csharp
using GoApi.Party;
```

We can now create our (simple) customer object by adding the following code to the end of the Main method:

```csharp
var customer = new Customer {
	Name = "My first customer",
	VatNumber = "999999999"
}
```

To retrieve or manipulate an entity (in this case a customer) we must use a service (in this case the CustomerService). The CustomerService is access through the Customer property on the api instance we created in the previous step.

To save the customer add following code below the last line in the Main method:

```csharp
api.Customer.Save(myNewCustomer);
```

# Query (get the customer by it's vat number)

Quering data is done using common LINQ queries. To get a customer by it's vat number we add the following code:

```csharp
customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == "999999999");
if (customer != null)
{
    Console.WriteLine("Code: " + customer.Code);
    Console.WriteLine("Name: " + customer.Name);
    Console.WriteLine("VatNumber: " + customer.VatNumber);
}
```

# Cleaning up (delete the customer we created)

To wrap up this tutorial we delete the customer we created with the following code:


```csharp
customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == "999999999");
if (customer != null)
{
	api.Customer.Delete(customer)
}
```

# The entire code

Below you'll see the complete code created in this tutorial:
```csharp
using GoApi;
using GoApi.Core;
using GoApi.Party;

class Program
{
	static void Main(string[] args)
    {
		// Setting up authentication
		var applicationKey = "AA70F6AD-1QQE-4EBF-9DA7-510962CE7E46";
		var clientKey = "1970F6AD-E35E-4EBF-9DA7-510962CE7E46";
		
		var authorizationSettings = new AuthorizationSettings
		{
		    ApplicationKey = applicationKey,
		    ClientKey = clientKey,
		    TokenStore = new BasicTokenStore("my.tokenstore")
		};

		var authorization = new Authorization(authorizationSettings);

		// Instanciating the API
		var api = new Go(authorization);

		// Create a new customer
		var customer = new Customer {
			Name = "My first customer",
			VatNumber = "999999999"
		}
		api.Customer.Save(myNewCustomer);

		// Get the customer by it's vat number
		customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == "999999999");
		if (customer != null)
		{
		    Console.WriteLine("Code: " + customer.Code);
		    Console.WriteLine("Name: " + customer.Name);
		    Console.WriteLine("VatNumber: " + customer.VatNumber);
		}

		// Delete the customer we created
		customer = api.Customer.Get().FirstOrDefault(c => c.VatNumber == "999999999");
		if (customer != null)
		{
			api.Customer.Delete(customer)
		}

 	}
}
```