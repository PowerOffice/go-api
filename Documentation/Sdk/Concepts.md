SDK - Concepts
==============

To keep the SDK easy to use we define a few concepts that should reflect how we intendt the SDK to be used.

The definitions in this chapter will be used throughout the documentation. 

# Data Object

A Data Object represents an entity. The Data Object consists only of properties that can be read or written to. Data Objects doesn't implement any manipulation methods, nor do they contain methods for saving or loading.

A prime example of a Data Object is Customer.

```csharp
var customer = new Customer();
customer.Name = "A Customer"
```

To retreive or store Data Objects you must use a Service.

# Service

Services is used to retreive data from, or manipulate data on the server.


When you want to create, update, or delete an entity you must use a Service, with which you provide the entity to be manipulated.  

For instance, if you would like to save a Customer, you will have to get an instance of a CustomerService:

```csharp
var api = new Go(authorization);
api.Customer.Save(customer);
```

Most services also provide methods to get lists of Data Objects. The filtering, ordering and paging of the lists is specified using Queries.

# Query

Queries is the method for specifying filters, ordering and paging when you want to retrieve lists of Data Objects from the server. In most cases you don't want all rows in a list, but rather a filtered list, you would then use standard LINQ queries to filter the data. 
 
If you where to get a list of the 50 first customers with a name starting with the letter A you could do something like this:

```csharp
api.Customer.Get()
	.Where(c => c.Name.StartsWith("A"))
	.OrderBy(c => c.Name)
	.Skip(0)
	.Take(50);
```
