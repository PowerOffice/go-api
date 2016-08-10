API Calls, Filtering
====================

Filters for request follows the [oData spesification](http://msdn.microsoft.com/en-us/library/azure/dd894031.aspx) for oData query options.

To filter a request, add the $filter parameter to the request. The filter must be enclosed in parenthesis.

#Overview

##Common operators

Operator			| URI expression
--------------------|---------------
Equal	 			| eq
GreaterThan 		| gt
GreaterThanOrEqual	| ge
LessThan			| lt
LessThanOrEqual		| le
NotEqual			| ne
And					| and
Not					| not
Or					| or

## Query String Encoding

The following characters must be encoded if they are to be used in a query string:

Character			| Encoded
--------------------|---------------
Forward slash (/)	| %2F
Question mark (?)	| %3F
Colon (:)			| %3A
'At' symbol (@)		| %40
Ampersand (&)		| %26
Equals sign (=)		| %3D
Plus sign (+)		| %2B
Comma (,)			| %2C
Dollar sign ($)		| %24

## Fields
All properties returned for a data object can be filtered. 

> *** IMPORTANT NOTE ON CASING IN FILTERS ***
> Filter fields must be in ["Pascal Case" casing](https://msdn.microsoft.com/en-us/library/x2dbyw72%28v=vs.71%29.aspx), even though the data object is returned with camel casing.

## Results

All results of a filtered query will be in the format

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"61"
			"code":"123",
			"name":"My first customer AS",
			"vatNumber":"123456789",
			"since": "2015-11-26",
		},
		{
			"id":"62"
			"code":"321",
			"name":"My second customer AS",
			"vatNumber":"987654321",
			"since": "2015-11-26",
		},
		"success":true, // The request was successfull 
		"count": 2		// Two records was returned
	}

## Other modificators

### Order the result set ascending or descending	 

	$orderby=Name asc


### Skip records in the result set

Do not return the first 10 records in the result set:

 	$skip=10

### Max number of records to return

Return a maximum of 10 records:

	$top=10

Note that the count property in the result will contain the total number of records in the filtered result set independant of $skip and $top:


## Examples

### Filter customer on VatNumber:

	GET https://api.poweroffice.net/customer?$filter=(VatNumber eq '123456789') HTTP/1.1
	Authorization: Bearer [Access Key]

Result:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"61"
			"code":"123",
			"name":"My first customer AS",
			"vatNumber":"123456789",
			"since": "2015-11-26",
		},
		"success":true,
		"count": 1
	}

### Filter customers since a date:

	GET https://api.poweroffice.net/customer?$filter=(Since ge DateTime'2015-11-26') HTTP/1.1
	Authorization: Bearer [Access Key]

Result:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"61"
			"code":"123",
			"name":"My first customer AS",
			"vatNumber":"123456789",
			"since": "2015-11-26",
		},
		...
		"success":true,
		"count": 123
	}

### Filter customers on name:
> Note that filters is case sensitive. To perform a case insensitive search you must use the `tolower` function:

 	GET https://api.poweroffice.net/customer?$filter=(tolower(Name) eq 'my first customer as') HTTP/1.1
	Authorization: Bearer [Access Key]


### Filter customers names starting with:

 	GET https://api.poweroffice.net/customer?$filter=(startswith(Name, 'My ') eq true) HTTP/1.1
	Authorization: Bearer [Access Key]

Starts with, case insensitive:

 	GET https://api.poweroffice.net/customer?$filter=(startswith(tolower(Name), 'my ') eq true) HTTP/1.1
	Authorization: Bearer [Access Key]

Result:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"61"
			"code":"123",
			"name":"My first customer AS",
			"vatNumber":"123456789",
			"since": "2015-11-26",
		},
		{
			"id":"62"
			"code":"321",
			"name":"My second customer AS",
			"vatNumber":"987654321",
			"since": "2015-11-26",
		},
		"success":true,
		"count": 2
	}


### Filter customers on name and date:

	GET https://api.poweroffice.net/customer?$filter=(Name eq 'My first customer AS' and Since ge DateTime'2015-11-26') HTTP/1.1
	Authorization: Bearer [Access Key]

Result:

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8

	{
		"data":
		{
			"id":"61"
			"code":"123",
			"name":"My first customer AS",
			"vatNumber":"123456789",
			"since": "2015-11-26",
		},
		"success":true,
		"count": 1
	}
