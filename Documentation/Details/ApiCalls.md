API Calls, Technical details
============================

This document describes the basics of GO-API calls. 

**If you use the C# GO-API SDK you do not need to know the technical details in this document** 

The examples in this document assumes you have read the [Authentication section](Authentication.md). Details about the *access key* will not be discussed here.

# Overview

## Design pattern

The GO-API calls is loosely based on the [REST design pattern](http://en.wikipedia.org/wiki/Representational_state_transfer). We do however some times deviate from the pattern.

## HTTP Verbs

We try to use the HTTP verbs as follows:

 Verb 	| Description
:-------|:------------
GET  	| Retreive data of an entity or a list of entities.
POST	| Create a new entity or a list of entities.
PUT		| Update an existing entity or a list of entities.
DELETE	| Remove an existing entity or a list of entities.


## Filters

Filters for request follows the [oData spesification](http://msdn.microsoft.com/en-us/library/azure/dd894031.aspx) for oData query options. 

## Data format

All input and response data is transfered using the [JSON format](http://www.json.org/). Response data is always wrapped in an object with a `success` boolean property and a `data` property containing the actual object returned by the api call.

Data is always expected to be UTF-8 encoded.

## Error handling

All well formed request should return HTTP 200 OK. If business logic or parameter data provided is invalid the `success` property will be set to `false`, and there will be no `data` property. Instead a `validation` property will be returned with details of the error.

*TODO: More about the validation object* 


# Examples

## Get a customer

If the customer id is known (for instance 1234-1234-1234-1234), you can request the customer data like this:

	GET http://api.go.poweroffice.net/customer/1234-1234-1234-1234 HTTP/1.1
	Authorization: Bearer [Access Key]

The response would then be something like this:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"1234-1234-1234-1234"
			"code":"123",
			"name":"Example Customer",
			"organizationNo":"123456789"
		},
		"success":true
	}

If the organization number is now the customer can be requested using oData query filter:

	GET http://api.go.poweroffice.net/customer/?organizationNo=123456789 HTTP/1.1
	Authorization: Bearer [Access Key]

## Create a customer

To create a new customer you would use the `POST` verb. Some minimum data must be provided, please see the [GO-API documention](../Sdk/GO SDK.chm) for details. If the customer is determined to be an existing customer, the customer will instead be updated.

	POST http://api.go.poweroffice.net/customer/ HTTP/1.1
	Authorization: Bearer [Access Key]
	Content-Type: application/json; charset=utf-8

	{
		"code":"123",
		"name":"My first customer AS",
		"legalName":"My first customer AS",
		"organizationNo":"123456789"
	}

The request will return the full Customer data object after it has been updated:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"1234-1234-1234-1234"
			"code":"123",
			"name":"My first customer AS",
			"legalName":"My first customer AS",
			"organizationNo":"123456789"
		},
		"success":true
	}

## Update customer

You can update a customer using the `PUT` verb:

	PUT http://api.go.poweroffice.net/customer/ HTTP/1.1
	Authorization: Bearer [Access Key]
	Content-Type: application/json; charset=utf-8

	{
		"id":"1234-1234-1234-1234",
		"name":"My First Customer AS"
	}

The request will return the full Customer data object after it has been updated:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"1234-1234-1234-1234"
			"code":"123",
			"name":"My first customer AS",
			"legalName":"My first customer AS",
			"organizationNo":"123456789"
		},
		"success":true
	}

## Delete customer

To delete a customer you should use the `DELETE` verb. 

	DELETE http://api.go.poweroffice.net/customer/1234-1234-1234-1234 HTTP/1.1
	Authorization: Bearer [Access Key]

The response after a success full delete would be something like this:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"success":true
	}


Customers with any child data created can not be deleted. If this is the case, the customer property `archived` should be set to true instead, using the `PUT` verb, to remove the customer from the list of active customers.

If you try to delete a customer that can not be deleted the response from the server will be something like this:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"validation":
		{
			summary:"The entity could not be deleted due to related data"
		},
		"success":false
	}
