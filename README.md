# Introduction
Bedrock.Shared is a .NET Standard library for writing .NET applications.

In general, Bedrock is a template for developing ASP.NET Core applications using Onion Architecture and S.O.L.I.D. design principles.

It contains a set of shared libraries in the form of Nuget packages and a template application that leverages these shared libraries.  It also contains an application for handling enterprise security (AuthZ).  The project also contains a tool to generate boilerplate domain code inferred from a database schema instance. 

Bedrock is comprised of four (5) repositories:

(1)  Bedrock.Shared -  this repository; the shared library

(2)  Bedrock.Shared.Security.UI -  Angular enterprise security application

(3)  Bedrock.Shared.Security.Api -  ASP.NET Core Web API enterprise security application that leverages the Bedrock.Shared library

(4)  Bedrock.Template.Api -  Angular and ASP.NET Core Web API template application that leverages the Bedrock.Shared library and Bedrock.Shared.Security application

(5)  Bedrock.DomainBuilder - Winform application to generate boilerplate domain layer code like domain entities/enumerations/services, EF DbContext, EF configuration mapping files etc...

The template application is wired up to authenticate with Azure AAD B2C, but this is turned off at the moment within Startup.

More description and help to follow.

# Getting Started
To come

# Build and Test
To come

# Contribute
To come

# Credits
To come
