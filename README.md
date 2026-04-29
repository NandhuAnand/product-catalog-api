# Product Catalog REST API

## Overview
This is a .NET 8 RESTful API for Product CRUD operations with related Item quantity tracking.

## Tech Stack
- .NET 8 Web API
- SQL Server
- Entity Framework Core
- JWT Authentication
- Role-based Authorization
- FluentValidation
- Swagger/OpenAPI
- xUnit, Moq, WebApplicationFactory
- Docker and Docker Compose
- Structured error handling with ProblemDetails

## Architecture
The solution follows Clean Architecture principles:

- Domain: Product and Item entities, domain rules
- Application: DTOs, services, validators, interfaces
- Infrastructure: EF Core DbContext, repositories, UnitOfWork
- API: Controllers, authentication, exception handling, Swagger

## API Endpoints

### Auth
POST /api/v1/auth/login  
POST /api/v1/auth/refresh-token

### Products
GET /api/v1/products  
GET /api/v1/products/{id}  
POST /api/v1/products  
PUT /api/v1/products/{id}  
DELETE /api/v1/products/{id}

## Demo Users

Admin:
- username: admin
- password: Admin@123
- access: GET, POST, PUT, DELETE

Guest:
- username: guest
- password: Guest@123
- access: GET only

## Authentication Flow
The login endpoint generates a JWT access token and refresh token.  
The JWT contains role claims used by ASP.NET Core authorization.

## Error Handling
The API uses .NET 8 IExceptionHandler with ProblemDetails for consistent error responses.

## Validation
FluentValidation is used for request validation in the application layer.

## Performance Considerations
- AsNoTracking for read-only EF Core queries
- Pagination for collection endpoints
- SQL indexes on ProductName, CreatedOn, and ProductId
- Response compression enabled
- Async/await used throughout the application

## Security Measures

- JWT authentication with role-based authorization
- Short-lived access tokens
- Basic refresh token endpoint (can be extended with rotation and persistence)
- Input validation using FluentValidation
- CORS policy enabled
- HTTPS enforced using middleware
- Security headers added to mitigate common web vulnerabilities

## Database
SQL Server is used with EF Core migrations.

Run migration:

```powershell
dotnet ef database update --project src/ProductCatalog.Infrastructure --startup-project src/ProductCatalog.Api