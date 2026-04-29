# FarmerFactory

A comprehensive .NET 10 application for managing farmer factory operations, including clients, products, and purchases. Built with a modern layered architecture utilizing ASP.NET Core, Entity Framework Core, and Blazor.

## Table of Contents

- [Overview](#overview)
- [Project Architecture](#project-architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Design Patterns](#design-patterns)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Database Schema](#database-schema)
- [Key Features](#key-features)
- [Configuration](#configuration)
- [Testing](#testing)
- [Contributing](#contributing)

## Overview

FarmerFactory is a multi-layered application designed to streamline farmer factory operations. It manages:
- **Clients**: Farmer clients who make purchases
- **Products**: Agricultural products available for purchase
- **Purchases**: Transaction records with purchase limits and time-based constraints

The application enforces business rules including purchase limits to prevent bulk purchases exceeding allowed quantities within specified timeframes.

## Project Architecture

The solution follows a **Layered Architecture** pattern with clear separation of concerns:

```
┌─────────────────────────────────────────────────┐
│         Presentation Layer                       │
│  ┌──────────────────────────────────────────┐   │
│  │ FarmerFactory.Web (Blazor Server)        │   │
│  │ FarmerFactory.AppHost (Orchestration)    │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
                      ↓ (HTTP Calls)
┌─────────────────────────────────────────────────┐
│         API Layer                                │
│  ┌──────────────────────────────────────────┐   │
│  │ FarmerFactory.Api (REST Endpoints)       │   │
│  │ Controllers & Routing                    │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│         Business Logic Layer                     │
│  ┌──────────────────────────────────────────┐   │
│  │ FarmerFactory.Services                   │   │
│  │ - ClientService                          │   │
│  │ - ProductService                         │   │
│  │ - PurchaseService (Business Rules)       │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│         Data Access Layer                        │
│  ┌──────────────────────────────────────────┐   │
│  │ FarmerFactory.Repositories                │   │
│  │ - ClientRepository                       │   │
│  │ - ProductRepository                      │   │
│  │ - PurchaseRepository                     │   │
│  │ - Entity Framework DbContext             │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│         Database Layer                           │
│  SQLite Database                                │
│  (DataSource=FarmerFactory.db)                  │
└─────────────────────────────────────────────────┘
```

### Cross-Cutting Concerns

```
┌─────────────────────────────────────────────────┐
│         Common Layer (FarmerFactory.Common)      │
│ - Entity Models (DTOs)                          │
│ - Custom Exceptions                            │
│ - Shared Constants & Utilities                  │
└─────────────────────────────────────────────────┘
```

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Runtime** | .NET | 10 |
| **Framework** | ASP.NET Core | 10 |
| **Web UI** | Blazor Server | 10 |
| **API** | REST with OpenAPI | 10 |
| **Database** | SQLite | In-Memory |
| **ORM** | Entity Framework Core | 10 |
| **Mapping** | AutoMapper | Latest |
| **Versioning** | API Versioning | Latest |
| **Testing** | xUnit | Latest |
| **Orchestration** | .NET Aspire | Latest |

## Project Structure

### 1. **FarmerFactory.Api** (Presentation - REST API Layer)
```
FarmerFactory.Api/
├── Controllers/
│   ├── ClientController.cs          # Client CRUD endpoints
│   ├── ProductController.cs         # Product management endpoints
│   └── PurchaseController.cs        # Purchase transaction endpoints
├── Configuration/
│   └── ServiceCollectionExtensions.cs # Dependency Injection setup
├── Program.cs                       # API startup configuration
└── appsettings.json                # API configuration
```

**Key Responsibilities:**
- HTTP endpoint routing and handling
- Request/response serialization
- API versioning (v1.0)
- Exception handling middleware
- OpenAPI/Swagger documentation

### 2. **FarmerFactory.Web** (Presentation - Blazor UI Layer)
```
FarmerFactory.Web/
├── Components/                      # Blazor components
├── Services/
│   ├── ClientService.cs            # HTTP client for clients
│   ├── ProductService.cs           # HTTP client for products
│   └── PurchaseService.cs          # HTTP client for purchases
├── App.razor                       # Root component
├── Program.cs                      # Blazor startup
└── appsettings.json               # Web configuration
```

**Key Responsibilities:**
- User interface using Blazor Server
- Client-side business logic wrappers
- HTTP communication with API
- User interaction handling

### 3. **FarmerFactory.Services** (Business Logic Layer)
```
FarmerFactory.Services/
├── Clients/
│   ├── ClientService.cs            # Client business logic
│   ├── IClientService.cs           # Service interface
│   └── ClientProfile.cs            # AutoMapper profile
├── Products/
│   ├── ProductService.cs           # Product business logic
│   ├── IProductService.cs          # Service interface
│   └── ProductProfile.cs           # AutoMapper profile
└── Purchases/
    ├── PurchaseService.cs          # Purchase logic & validation
    ├── IPurchaseService.cs         # Service interface
    └── PurchaseProfile.cs          # AutoMapper profile
```

**Key Responsibilities:**
- Business rule enforcement
- Data transformation via AutoMapper
- Service orchestration
- Purchase limit validation

**Critical Business Logic (PurchaseService):**
- Validates purchase quantity limits (max 1 unit per minute per client/product)
- Checks time-based purchase history (1-minute window)
- Enforces cumulative purchase limits
- Throws `TooManyRequestsException` when limits are exceeded

### 4. **FarmerFactory.Repositories** (Data Access Layer)
```
FarmerFactory.Repositories/
├── Models/
│   ├── Client.cs                   # Entity: Client
│   ├── Product.cs                  # Entity: Product
│   └── Purchase.cs                 # Entity: Purchase
├── Clients/
│   ├── ClientRepository.cs
│   └── IClientRepository.cs
├── Products/
│   ├── ProductsRepository.cs
│   └── IProductsRepository.cs
├── Purchases/
│   ├── PurchaseRepository.cs
│   └── IPurchaseRepository.cs
└── CoreApiDbContext.cs             # EF Core DbContext
```

**Key Responsibilities:**
- Entity Framework Core configuration
- Database migrations
- CRUD operations
- Query execution
- Time-based filtering (e.g., GetByTimeAsync)

### 5. **FarmerFactory.Common** (Cross-Cutting Concerns)
```
FarmerFactory.Common/
├── Entities/
│   ├── Client/
│   │   ├── ClientRequest.cs
│   │   └── ClientResponse.cs
│   ├── Product/
│   │   ├── ProductRequest.cs
│   │   └── ProductResponse.cs
│   └── Purchase/
│       ├── PurchaseRequest.cs
│       └── PurchaseResponse.cs
└── Exceptions/
    ├── TooManyRequestsException.cs # Custom HTTP 429 exception
    └── [Other custom exceptions]
```

**Key Responsibilities:**
- Data Transfer Objects (DTOs)
- Custom exception definitions
- Shared constants
- Validation attributes

### 6. **FarmerFactory.Test** (Testing Layer)
```
FarmerFactory.Test/
├── Repositories/
│   ├── Clients/ClientRepositoryTest.cs
│   ├── Products/ProductRepositoryTest.cs
│   └── Purchases/PurchaseRepositoryTest.cs
├── Configuration/
│   └── ConfigurationManager.cs
└── [Additional test fixtures]
```

**Key Responsibilities:**
- Unit tests for repositories
- Test data configuration
- Test database setup

### 7. **FarmerFactory.AppHost** (Orchestration)
Service orchestration and startup configuration for distributed application components.

## Design Patterns

### 1. **Layered Architecture**
Clear separation of concerns across presentation, business logic, and data access layers.

```csharp
// Example flow:
Controller → Service → Repository → Database
```

### 2. **Repository Pattern**
Abstraction of data access logic through repository interfaces.

```csharp
// Interface definition
public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAsync();
    Task<Purchase> PostAsync(Purchase purchase);
    Task<Purchase> GetByTimeAsync(DateTime fromTime, int clientId, int productId);
}
```

### 3. **Dependency Injection**
Constructor-based DI for loose coupling and testability.

```csharp
// Service registration
services.AddTransient<IPurchaseRepository, PurchaseRepository>();
services.AddTransient<IPurchaseService, PurchaseService>();

// Constructor injection
public class PurchaseService : IPurchaseService
{
    public PurchaseService(IPurchaseRepository repository, IMapper mapper)
    {
        _purchaseRepository = repository;
        _mapper = mapper;
    }
}
```

### 4. **Mapper Pattern (AutoMapper)**
Object transformation between entities and DTOs.

```csharp
// Mapping configuration
public class PurchaseProfile : Profile
{
    public PurchaseProfile()
    {
        CreateMap<PurchaseRequest, Purchase>();
        CreateMap<Purchase, PurchaseResponse>();
    }
}

// Usage
var purchaseEntity = _mapper.Map<Purchase>(purchaseRequest);
var response = _mapper.Map<PurchaseResponse>(result);
```

### 5. **Service Locator Pattern (API Controllers)**
Controllers depend on service interfaces to handle business logic.

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PurchaseController : ControllerBase
{
    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
}
```

### 6. **DTO (Data Transfer Object) Pattern**
Separate objects for API requests and responses.

```csharp
public class PurchaseRequest
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Qty { get; set; }
}

public class PurchaseResponse
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public DateTime PurchaseTime { get; set; }
}
```

### 7. **API Versioning**
RESTful API with version management.

```csharp
// Configuration
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Route
[Route("api/v{version:apiVersion}/[controller]")]
```

### 8. **Exception Handling & Custom Exceptions**
Structured error handling with custom exceptions for business logic violations.

```csharp
// Custom exception
public class TooManyRequestsException : Exception { }

// Middleware handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        if (exceptionHandler?.Error is TooManyRequestsException)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        }
    });
});
```

## Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio 2026 (or any .NET 10 compatible IDE)
- SQLite (included with Entity Framework Core)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd FarmerFactory
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Run database migrations**
   ```bash
   cd FarmerFactory.Api
   dotnet ef database update
   ```

4. **Start the API server**
   ```bash
   cd FarmerFactory.Api
   dotnet run
   # API runs on: https://localhost:5249
   ```

5. **Start the Blazor Web UI** (in another terminal)
   ```bash
   cd FarmerFactory.Web
   dotnet run
   # Web runs on: https://localhost:7245
   ```

6. **Access the application**
   - Blazor UI: `https://localhost:7245`
   - API: `https://localhost:5249`
   - OpenAPI Docs: `https://localhost:5249/openapi/v1.json`

## API Documentation

### Base URL
```
https://localhost:5249/api/v1
```

### Clients Endpoints

#### Get All Clients
```http
GET /api/v1/client
Content-Type: application/json

Response: 200 OK
[
  {
    "id": 1,
    "name": "John Farmer",
    "email": "john@farm.com"
  }
]
```

### Products Endpoints

#### Get All Products
```http
GET /api/v1/product
Content-Type: application/json

Response: 200 OK
[
  {
    "id": 1,
    "name": "Corn",
    "price": 25.50,
    "stock": 1000
  }
]
```

### Purchases Endpoints

#### Get All Purchases
```http
GET /api/v1/purchase
Content-Type: application/json

Response: 200 OK
[
  {
    "id": 1,
    "clientId": 1,
    "productId": 1,
    "qty": 1,
    "purchaseTime": "2024-01-15T10:30:00Z"
  }
]
```

#### Create Purchase
```http
POST /api/v1/purchase
Content-Type: application/json

Body:
{
  "clientId": 1,
  "productId": 1,
  "qty": 1
}

Response: 200 OK
{
  "id": 1,
  "clientId": 1,
  "productId": 1,
  "qty": 1,
  "purchaseTime": "2024-01-15T10:30:00Z"
}

Response: 429 Too Many Requests
{
  "message": "Too many requests"
}
```

### Error Responses

#### 400 Bad Request
```json
{
  "message": "An error occurred",
  "error": "Invalid request data"
}
```

#### 429 Too Many Requests
```json
{
  "message": "Too many requests"
}
```

#### 500 Internal Server Error
```json
{
  "message": "An error occurred"
}
```

## Database Schema

### Entity-Relationship Diagram

```
┌─────────────────┐       ┌──────────────────┐
│     Client      │       │     Product      │
├─────────────────┤       ├──────────────────┤
│ Id (PK)         │       │ Id (PK)          │
│ Name            │       │ Name             │
│ Email           │       │ Price            │
└─────────────────┘       │ Stock            │
        │                 └──────────────────┘
        │                         │
        └────────────┬────────────┘
                     │
              ┌──────────────────┐
              │    Purchase      │
              ├──────────────────┤
              │ Id (PK)          │
              │ ClientId (FK)    │──→ Client
              │ ProductId (FK)   │──→ Product
              │ Qty              │
              │ PurchaseTime     │
              └──────────────────┘
```

### Table Definitions

#### Clients
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PRIMARY KEY | Unique identifier |
| Name | NVARCHAR(255) | NOT NULL | Client name |
| Email | NVARCHAR(255) | NOT NULL | Client email |

#### Products
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PRIMARY KEY | Unique identifier |
| Name | NVARCHAR(255) | NOT NULL | Product name |
| Price | DECIMAL(18,2) | NOT NULL | Product price |
| Stock | INT | NOT NULL | Available stock |

#### Purchases
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PRIMARY KEY | Unique identifier |
| ClientId | INT | FOREIGN KEY | Reference to Client |
| ProductId | INT | FOREIGN KEY | Reference to Product |
| Qty | INT | NOT NULL | Quantity purchased |
| PurchaseTime | DATETIME | NOT NULL | Purchase timestamp |

## Key Features

### 1. **Purchase Limit Enforcement**
Prevents bulk purchases within a specified timeframe (1 minute):
- Maximum 1 unit per purchase
- Cumulative limit check within 60-second window
- Throws `429 Too Many Requests` when limit exceeded

**Business Rule Logic:**
```
IF (RequestQuantity > 1)
    THROW TooManyRequestsException
ELSE
    previousPurchase = GET purchase from last 1 minute
    IF (previousPurchase + currentQuantity > 1)
        THROW TooManyRequestsException
```

### 2. **RESTful API**
- HTTP/2 support
- OpenAPI/Swagger documentation
- Standardized request/response formats
- Proper HTTP status codes

### 3. **Blazor Server UI**
- Real-time interactive components
- Server-side rendering
- Secure by default with anti-forgery tokens

### 4. **Database Persistence**
- SQLite for local development
- Entity Framework Core ORM
- Automatic migrations on startup

### 5. **API Versioning**
- Version routing support
- Backward compatibility
- Version reporting in headers

### 6. **Exception Handling**
- Global exception middleware
- Custom exception mapping
- Development/production error details

## Configuration

### API Configuration (`FarmerFactory.Api/appsettings.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=FarmerFactory.db"
  },
  "AllowedHosts": "*"
}
```

### Database Connection
```csharp
// In-memory SQLite
builder.Services.AddDbContext<CoreApiDbContext>(options =>
    options.UseSqlite("DataSource=FarmerFactory.db"));
```

### API Versioning Configuration
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
```

### Dependency Injection Setup
```csharp
builder.Services
    .AddMapperServices()      // AutoMapper setup
    .AddMyDependencyGroup()   // Service & Repository registration
    .AddOpenApi();            // OpenAPI documentation
```

## Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test FarmerFactory.Test

# Run specific test class
dotnet test --filter "FullyQualifiedName~ClientRepositoryTest"

# Run specific test method
dotnet test --filter "TestMethod=GetAsync_ReturnsAllClients"
```

### Test Projects
- **FarmerFactory.Test.Repositories.Clients** - Client repository tests
- **FarmerFactory.Test.Repositories.Products** - Product repository tests
- **FarmerFactory.Test.Repositories.Purchases** - Purchase repository tests

### Test Example
```csharp
[Fact]
public async Task PostAsync_WithValidPurchase_ReturnsCreatedPurchase()
{
    // Arrange
    var purchaseRequest = new PurchaseRequest
    {
        ClientId = 1,
        ProductId = 1,
        Qty = 1
    };

    // Act
    var result = await _purchaseService.PostAsync(purchaseRequest);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.ClientId);
}
```

## File Structure Summary

```
FarmerFactory/
├── FarmerFactory.Api/                    # REST API Layer
│   ├── Controllers/
│   ├── Configuration/
│   └── Program.cs
├── FarmerFactory.Web/                    # Blazor UI Layer
│   ├── Components/
│   ├── Services/
│   └── Program.cs
├── FarmerFactory.Services/               # Business Logic Layer
│   ├── Clients/
│   ├── Products/
│   ├── Purchases/
│   └── Profiles/
├── FarmerFactory.Repositories/           # Data Access Layer
│   ├── Models/
│   ├── Clients/
│   ├── Products/
│   ├── Purchases/
│   └── CoreApiDbContext.cs
├── FarmerFactory.Common/                 # Cross-Cutting Concerns
│   ├── Entities/
│   └── Exceptions/
├── FarmerFactory.Test/                   # Unit Tests
│   └── Repositories/
├── FarmerFactory.AppHost/                # Service Orchestration
└── README.md                             # This file
```

## Architecture Principles

### SOLID Principles
- **S**ingle Responsibility: Each layer has one reason to change
- **O**pen/Closed: Services are open for extension via interfaces
- **L**iskov Substitution: Implementations are interchangeable
- **I**nterface Segregation: Specific interfaces for each service
- **D**ependency Inversion: Depend on abstractions, not concrete types

### Clean Architecture
- Clear layer boundaries with minimal coupling
- Business logic independent of frameworks
- Easy to test with dependency injection
- Framework-agnostic core logic

### Best Practices
- Async/await for non-blocking I/O
- Using DTOs for API contracts
- Proper exception handling
- Time-based validation for purchase limits
- API versioning for backward compatibility

## Contributing

### Code Style
- Follow Microsoft C# Coding Conventions
- Use meaningful variable names
- Add XML documentation comments for public APIs
- Keep methods focused and single-responsibility

### Pull Request Process
1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Ensure all tests pass
5. Submit a pull request with clear description

## Future Enhancements

- [ ] Authentication & Authorization (JWT)
- [ ] Advanced filtering & pagination
- [ ] Purchase history analytics
- [ ] Inventory management
- [ ] Notification system
- [ ] Caching layer (Redis)
- [ ] Background jobs (Hangfire)
- [ ] Logging & monitoring (Serilog, Application Insights)
- [ ] Unit tests for services
- [ ] Integration tests
- [ ] Docker containerization
- [ ] CI/CD pipeline

## Troubleshooting

### Database Issues
```bash
# Drop and recreate database
dotnet ef database drop --force
dotnet ef database update
```

### Port Already in Use
```bash
# Change port in launchSettings.json
# API: applicationUrl: "https://localhost:5250"
# Web: applicationUrl: "https://localhost:7246"
```

### API Connection Issues
Ensure the API base address in `FarmerFactory.Web/Program.cs` matches:
```csharp
builder.Services.AddHttpClient<IPurchaseService, PurchaseService>(client =>
{
    client.BaseAddress = new("http://localhost:5249");
});
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or contributions, please refer to the project documentation or contact the development team.

---

**Last Updated:** 2024
**Version:** 1.0
**Status:** Active Development
