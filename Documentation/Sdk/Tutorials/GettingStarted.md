Getting Started - Tutorial
==========================

After installing the SDK and setting up the sandbox environment, the next thing to do is to create a simple test project with the basics of an integration.

This tutorial walks you through your first test project.

The resulting project can be found in the SDK examples folder: Exampels/Test1/

# Create the project in Visual Studio

TODO: Describe and screenshot New project, Console application, .NET Framework 4.5.1

# Reference the GoApi assembly

The first thing to do is to add a reference to the GoApi assembly.

1. In the *Solution Explorer*, Right click *Test1->References*
2. Select *Add Reference*
3. Select *Browse*
4. Click the *Browse* button
5. In the file dialog, navigate to *&lt;SdkFolder&gt;/Bin/*
6. Select *GoApi.dll* 
 
# Add our first code file

For this example, we will put all our code in one file.

1. In the *Solution Explorer*, Right click the project item
2. Select *Add->New Item*
3. Select the *Visual C# Items->Class*-template
4. Set the name to *Program.cs*
5. Click the *Add* button

# Writing our first code

First we must add a using statement for the Go.Api.Core namespace at the top of the file

```csharp
using GoApi.Core;
```

Now we must set up our authorization settings. 

Here we provide the *application key* and the *client key* (see [Registration and Client activation](../../Registration.md) for more info).

A *token store* must also be provided. The token store is responsible for persisting the access key and refresh key obtained from the authorisation server (see [Authentication, Technical details](../Details/Authentication.md) for technical details).

The Go.Api provides a basic token store that will create an encrypted file that will hold the needed details. If you at some point need to control the token store it is possible to derive the basic store or implement the ITokenStore interface. 
 

```csharp
    class Program
    {
        static void Main(string[] args)
        {
			var developerKey = "AA70F6AD-1QQE-4EBF-9DA7-510962CE7E46";
			var clientKey = "1970F6AD-E35E-4EBF-9DA7-510962CE7E46";
			
			var authorizationSettings = new AuthorizationSettings
			{
			    DeveloperKey = integrationDeveloperKey,
			    ClientKey = clientKey,
			    TokenStore = new BasicTokenStore("my.tokenstore")
			};

			var authorization = new Authorization(authorizationSettings);
        }
    }
```

Now we have set up the authorization. If you try to run the program it will connect to the authorization server and request an access key that can be used to call API functions.

# Calling an API function

The next thing we're going to do is to get a customer by it's organization number.

To access customers services, we need to add a using statement for the Go.Api.Party namespace. Add the following to the top of the file.

```csharp
using GoApi.Party;
```

To retrieve or manipulate an entity (in this case a customer) we must use a service (in this case the CustomerService).

All services takes one parameter, our authorization object instance.

Add the following code below the last line in the Main method:

```csharp
var customerService = new CustomerService(authorization);

var customer = customerService.GetByOrganizationNo("123456789");
if (customer != null)
{
    Console.WriteLine("Code: " + customer.Code);
    Console.WriteLine("Name: " + customer.Name);
    Console.WriteLine("OrganizationNo: " + customer.OrganizationNo);
}

Console.ReadKey();
```

What we do here is to instanciate a CustomerService instance, and use the GetByOrganizationNo method to request the customer with organization number 123456789.

If a customer with that organization number exists we will the method will retun a Customer object with all it's properties set.

Next we write a few of the properties to the console.

# Wrap up

That was the very first tutorial. If everything worked out, we have now connected to PowerOffice GO and made our first API call.
