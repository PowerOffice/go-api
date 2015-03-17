SDK - Introduction
==================

To provide application developerswith an easy entry and a stream lined experience, PowerOffice provides, maintains and supports a C# SDK for developing applications for PowerOffice Go.

The SDK aims at providing third party developers with a development kit that makes developing integrations with PowerOffice Go quick, simple and a pleasent experience.

The SDK is constantly beeing maintained and expanded based on users feed back. It is important for us to give you, the developer, the best experience possible when developing integrations for your customers.


# First steps to developing an PowerOffice Go API application

The first thing you must do is to register as an integration parter to obtain an *application key*. Please read the [Registration and Client activation](../Registration.md) section.

## Download SDK
The SDK is distributed as a [NuGet](https://www.nuget.org/) package. Read [this section](NuGetPackage.md) for information on how to download the [PowerOffice Go Sdk package](https://www.nuget.org/packages/PowerOfficeGoSdk/)

## Documentation and examples
The entire Documentation and C# Examples can be downloaded from our [github project](https://github.com/PowerOffice/go-api)

You can download everything as a [zip file](https://github.com/PowerOffice/go-api/archive/master.zip)

### Tutorials
Please look at the tutorials for a quick introduction on how to use the C# PowerOffice Go SDK

[Getting Started Tutorial](Tutorials/GettingStarted.md)

### Examples

We maintain example C# projects for the most common scenarios in our [github project.](https://github.com/PowerOffice/go-api)

### Reference Documentation
The SDK reference documentation is available online at [api.poweroffice.net](http://api.poweroffice.net/web/documentation)
Or as a compiled help file (chm) from [github.](https://github.com/PowerOffice/go-api/raw/master/Documentation/Sdk/GO%20SDK.chm)


# Development enviroment / Sandbox

## Supported development environment
- Microsoft Visual Studio 2013 or later
- .NET Framework 4.5.1 or later

## Sandbox
The Go API SDK can be set to work within a sandboxed environment through the global setting
```csharp
GoApi.Global.Settings.Mode = Settings.EndPointMode.Sandbox;
``` 
> *** HOWEVER THE SANDBOX ENVIRONMENT IS CURRENLTY NOT SUPPORTED ***




