# SmartPlate – Personalised Number Plate Marketplace  
### Modular Monolith Version (Initial Release)

SmartPlate is a **clean, modular ASP.NET Core 9 Web API** that simulates the buying and selling of personalised number plates.  
This version is intentionally built as a **modular monolith** to allow fast development, easy maintenance, and effortless future scaling into microservices.

> Development is currently paused due to a family visit. I will resume work starting **05.12.2025**, but I am available for interviews in the meantime.
---
## Features

### Implemented
###  Authentication & Users
- User registration & login  
- JWT-based authentication  
- Role-based authorisation (**Admin**, **User**)
- Unit tests for controllers and services 

###  Plates Management
- CRUD operations  
- Search & filtering  
- Ownership tracking  

###  Orders 
- Create purchase orders  
- Transfer ownership between users  

**Architecture & Quality**
- Modular structure: `Users`, `Plates`, `PlateOwnership`, `Orders`  
- EF Core 9 + SQLite  
- FluentValidation for request validation  
- Swagger documentation

---
##  Project structure
```
SmartPlate
│
├── Controllers/ # Web API controllers handling HTTP requests
│ ├── OrderController.cs # CRUD & order-related endpoints
│ ├── PlateController.cs # Plate management endpoints
│ ├── PlateListingController.cs# Manage plate listings
│ ├── PlateOwnershipController.cs # Handles ownership transfers
│ └── UserController.cs # User registration, login, and management
│
├── DTOs/ # Data Transfer Objects (request/response models)
│ ├── Order/ 
│ ├── Plate/ 
│ ├── PlateListing/ 
│ ├── PlateOwnership/ 
│ └── User/
│
├── Data/ # Database context and configurations
│ ├── Configurations/ # Entity configurations for EF Core
│ │ ├── OrderConfiguration.cs
│ │ ├── PlateBidConfiguration.cs
│ │ ├── PlateConfiguration.cs
│ │ ├── PlateListingConfiguration.cs
│ │ ├── PlateOwnershipRecordConfiguration.cs
│ │ └── UserConfiguration.cs
│ └── AppDbContext.cs # Main EF Core DbContext
│
├── Helpers/ # Utility classes and helpers
│ ├── JsonDateTimeConverter.cs # Custom JSON DateTime serialisation
│ └── JwtSettings.cs # JWT configuration settings
│
├── Mappers/ # Map domain models to DTOs
│ ├── OrderMapper.cs
│ ├── PlateListingMapper.cs
│ ├── PlateMapper.cs
│ └── PlateOwnershipMapper.cs
│
├── Models/ # Domain models & enums
│ ├── Enums/ # Enum definitions
│ │ ├── OrderEnums.cs
│ │ └── PlateEnums.cs
│ ├── Order.cs
│ ├── Plate.cs
│ ├── PlateBid.cs
│ ├── PlateListing.cs
│ ├── PlateOwnershipRecord.cs
│ └── User.cs
│
├── Properties/ # Project properties
│ └── launchSettings.json # Debug & launch settings
│
├── Services/ # Application services (business logic) with their interfaces and implementation
│ ├── OrderService/ # Order-related business logic
│ │ ├── IOrderService.cs 
│ │ └── OrderService.cs 
│ ├── PlateListingService/ # Plate listing logic
│ | ├── IPlateListingService.cs
│ │ └── PlateListingService.cs 
│ ├── PlateOwnershipService/ # Ownership-related logic
| | ├── IPlateOwnershipService.cs 
│ │ └── PlateOwnershipService.cs 
│ ├── PlateService/ # Plate management logic
| | ├──  IPlateService.cs 
│ │ └── PlateService.cs 
│ └── UserService/ # User-related logic (registration, auth)
|   ├── IUserService.cs 
│   └── UserService.cs 
│
├── Tests/ # Unit tests
│ ├── UserTests/
│ │ ├── UserControllerTests.cs
│ │ ├── UserServiceTests.cs
│ │ └── Tests.csproj
│
├── .gitignore 
├── AppDb.db 
├── AppDb.db-shm 
├── AppDb.db-wal 
├── Program.cs 
├── SmartPlate.csproj 
├── SmartPlate.http 
└── SmartPlate.sln
```
### Explanation
- **Controllers:** Handle API endpoints and HTTP requests.
- **DTOs:** Encapsulate data to transfer between client and server.
- **Data:** EF Core DbContext and entity configurations for database mapping.
- **Helpers:** Utility classes like JWT config or custom JSON converters.
- **Mappers:** Convert between domain models and DTOs for clean separation.
- **Models:** Core domain entities and enums representing the database schema.
- **Properties:** Project-specific settings, including launch configs.
- **Services:** Business logic layer separated by domain (User, Plate, Orders, etc.).
- **Tests:** Unit and integration tests for controllers and services.
- **Database files:** SQLite files for development/testing.
- **Program.cs & project files:** Entry point and project/solution configuration.

##  Tech Stack

| Area        | Technology             |
|-------------|------------------------|
| Backend     | C#, ASP.NET Core 9     |
| Database    | SQLite                 |
| ORM         | Entity Framework Core  |
| Auth        | JWT Authentication     |
| Validation  | FluentValidation       |
| Testing     | xUnit                  |
| IDE         | VS Code                |

---

###  Planned / Next Steps
**Future modules**
-PlateBid

**Database & Infrastructure**
- Migrate from SQLite → SQL Server (Azure SQL or Developer Edition)  
- Full enterprise-level database capabilities with T-SQL, stored procedures, and server-side pagination  

**Microservices Architecture (Phase 2)**
- Split modules into separate APIs
- Each service has its own database  
- Optionally add **YARP API Gateway** and service discovery  

**Advanced Features**
- CQRS + MediatR (decoupled commands & queries)  
- SignalR for real-time notifications (order updates, new plates, purchases)  
- Hangfire background jobs (email confirmation, cleanup tasks, scheduled discounts)  
- Serilog logging with Seq / Elastic Stack  

**Testing & CI/CD**
- Remaining unit tests 
- Integration tests (EF Core InMemory)  
- API tests (WebApplicationFactory)  
- CI/CD pipeline via GitHub Actions  


## Installation & Running

### 1. Clone & Enter Directory
```bash
git clone https://github.com/vladafursa/SmartPlate.git
cd SmartPlate




