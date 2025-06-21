# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a Clean Architecture .NET 9.0 REST API template for Mapfre Puerto Rico (MPR) that follows Domain-Driven Design principles. The solution provides a comprehensive foundation for enterprise REST APIs with multi-database support, sophisticated security, and extensive code generation capabilities.

## Architecture Structure

The solution follows Clean Architecture with these layers:

- **MPR.RestApiTemplate.Api** - Web API controllers, startup configuration, and API versioning
- **MPR.RestApiTemplate.Application** - Application services, DTOs, AutoMapper profiles
- **MPR.RestApiTemplate.Domain** - Domain entities, interfaces, repository contracts
- **MPR.RestApiTemplate.Infrastructure** - Data access with Entity Framework, repository implementations
- **MPR.RestApiTemplate.Security** - Multi-provider authentication (MapfreSecurity, Okta, EntraID)
- **MPR.RestApiTemplate.Tests.Integration** - Integration tests using xUnit and ASP.NET Core Test Host
- **MPR.RestApiTemplate.Security.Tests** - Security component unit tests
- **MPR.RestApiTemplate.Utilities.CodeGeneration** - T4 templates and PowerShell scripts for code generation

## Common Development Commands

### Building and Running
```bash
# Build entire solution
dotnet build

# Run the API (defaults to https://localhost:5001)
dotnet run --project MPR.RestApiTemplate.Api

# Run all tests
dotnet test

# Run specific test project
dotnet test MPR.RestApiTemplate.Tests.Integration
dotnet test MPR.RestApiTemplate.Security.Tests
```

### Database Operations
```powershell
# Navigate to code generation utilities
cd MPR.RestApiTemplate.Utilities.CodeGeneration

# Scaffold database (SQL Server)
.\ScaffoldDatabase.ps1 -ConnectionString "Server=(localdb)\mssqllocaldb;Database=Northwind;..." -DbProvider SqlServer

# Scaffold database (Oracle)
.\ScaffoldDatabase.ps1 -ConnectionString "Data Source=localhost:1521/XEPDB1;User Id=hr;..." -DbProvider Oracle

# Generate all code layers (requires Visual Studio 2022 Professional)
.\CodeGenerator.ps1

# Create and apply EF migrations
.\MigrationsCreate.ps1
.\MigrationsApply.ps1

# Prepare template for distribution
.\PrepareTemplate.ps1
```

## Key Configuration Files

- **appsettings.json** - Main application configuration including connection strings and security settings
- **appsettings.generated.json** - Auto-generated security policies (created by code generation)
- **mpr.codegen.json** - Code generation configuration controlling what gets generated per entity
- **T4 templates** in `MPR.RestApiTemplate.Utilities.CodeGeneration/Templates/` - Control code generation output

## Multi-Database Support

The template supports both SQL Server (primary) and Oracle databases:
- Both providers use Entity Framework Core with appropriate database-specific drivers
- Database provider is configurable via connection strings and scaffolding scripts

## Security Architecture

The security system supports multiple authentication providers configured via `appsettings.json`:
- **MapfreSecurity** (default) - Custom Mapfre authentication system using `Mapfre.URL.SecurityToken.dll` and `MapfreUserSecurityLibrary.dll` to handle user authentication and authorization against company database
- **Okta** - OAuth 2.0 provider
- **EntraID** - Azure Active Directory

Authorization uses policy-based approach with custom requirements for:
- Permission-based access control
- Role-based security
- Entity-specific operation policies

## Code Generation System

The T4 template-based system generates:
- Repository interfaces and implementations
- Application services and DTOs
- AutoMapper mapping profiles
- Web API controllers with versioning
- Unit of Work pattern implementation
- Integration tests for all endpoints
- Service registrations for dependency injection

Generated code is clearly separated from custom code and marked with generation timestamps.

## API Structure

- **Versioning**: URL segment-based (`/api/v1/...`)
- **Documentation**: Scalar UI at `/scalar/v1`, OpenAPI spec at `/openapi/v1.json`
- **Sample Data**: Uses Northwind database entities (Category, Product, Customer, Order, etc.)
- **Serialization**: Newtonsoft.Json with reference loop handling
- **CRUD Operations**: Auto-generated for all entities with configurable method generation

## Important Development Notes

- The solution uses Northwind as sample database - replace with actual business entities
- Code generation requires Visual Studio 2022 Professional (uses TextTransform.exe)
- Generated files should not be manually edited - modify T4 templates instead
- Security policies are auto-generated based on entity configuration
- Template is designed for packaging as a .NET template via `PrepareTemplate.ps1`
- Integration tests are auto-generated for all API endpoints
- Database scaffolding recreates entity models - custom modifications will be lost