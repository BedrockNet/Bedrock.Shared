REM Arguments

REM %1:  Projects Path
REM %2:  New Version
REM %3:  Is Delete Packages
REM %4:  Is Bump Package Versions
REM %5:  Is Build Packages
REM %6:  Is Push Packages

REM Example:  build.bat "D:\Bedrock\Bedrock.Shared" "1.0.0" true true true true

IF "%~1" == "" (
	echo Missing parameter
	exit /b
)

IF "%~2" == "" (
	echo Missing parameter
	exit /b
)

IF "%~3" == "" (
	echo Missing parameter
	exit /b
)

IF "%~4" == "" (
	echo Missing parameter
	exit /b
)

IF "%~5" == "" (
	echo Missing parameter
	exit /b
)

IF "%~6" == "" (
	echo Missing parameter
	exit /b
)

REM Clean Nuget Package Directory
IF %3 == true (
	del /s /q /f %1\nuget
)

REM Bump Version Numbers
IF %4 == true (
	dotnet "%CD%"\Bedrock.Shared.Tools\Bedrock.Shared.Tools.VersionBumper\bin\Debug\netcoreapp2.2\Bedrock.Shared.Tools.VersionBumper.dll /p:%1 /v:%2 /s:true /q:false /k:false
)

REM Build Nuget Packages
IF %5 == true (
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared\Bedrock.Shared

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Cache\Bedrock.Shared.Cache
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Cache\Bedrock.Shared.Cache.Memory
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Cache\Bedrock.Shared.Cache.Redis

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Pagination\Bedrock.Shared.Pagination

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Entity\Bedrock.Shared.Entity

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Log\Bedrock.Shared.Log
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Log\Bedrock.Shared.Log.NLog

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Mapper\Bedrock.Shared.Mapper
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Mapper\Bedrock.Shared.Mapper.AutoMapper

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Hash\Bedrock.Shared.Hash
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Hash\Bedrock.Shared.Hash.BCrypt

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Serialization\Bedrock.Shared.Serialization
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Serialization\Bedrock.Shared.Serialization.Binary
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Serialization\Bedrock.Shared.Serialization.Newtonsoft
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Serialization\Bedrock.Shared.Serialization.Xml

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Security\Bedrock.Shared.Security

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Data.Validation\Bedrock.Shared.Data.Validation
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Data.Validation\Bedrock.Shared.Data.Validation.Implementation

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Service\Bedrock.Shared.Service

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Session\Bedrock.Shared.Session
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Session\Bedrock.Shared.Session.Implementation

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Domain\Bedrock.Shared.Domain
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Domain\Bedrock.Shared.Domain.Implementation

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Entity\Bedrock.Shared.Entity.Implementation

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Service\Bedrock.Shared.Service.Implementation

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Data.Repository\Bedrock.Shared.Data.Repository
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Data.Repository\Bedrock.Shared.Data.Repository.EntityFramework

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Model\Bedrock.Shared.Model

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Queue\Bedrock.Shared.Queue

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Web\Bedrock.Shared.Web
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Web\Bedrock.Shared.Web.Api
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Web\Bedrock.Shared.Web.Api.Swagger
	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Web\Bedrock.Shared.Web.Mvc

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Ioc\Bedrock.Shared.Ioc

	dotnet pack -c Debug -o "%CD%"\nuget\ "%CD%"\Bedrock.Shared.Ioc\Bedrock.Shared.Ioc.Autofac
)

REM Push New Packages
IF %6 == true (
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Cache.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Cache.Memory.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Cache.Redis.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Pagination.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Entity.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Log.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Log.NLog.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Mapper.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Mapper.AutoMapper.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Hash.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Hash.BCrypt.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Serialization.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Serialization.Binary.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Serialization.Newtonsoft.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Serialization.Xml.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Security.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Data.Validation.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Data.Validation.Implementation.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Service.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Session.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Session.Implementation.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Domain.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Domain.Implementation.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Entity.Implementation.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Service.Implementation.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Data.Repository.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Data.Repository.EntityFramework.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Model.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Queue.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Web.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Web.Api.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Web.Api.Swagger.%2.nupkg
	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Web.Mvc.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Ioc.%2.nupkg

	nuget.exe push -Source "Bedrock.Shared" -ApiKey AzureDevOps nuget\Bedrock.Shared.Ioc.Autofac.%2.nupkg
)

set /p=Hit ENTER to continue...