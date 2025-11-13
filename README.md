# SmartPlate – Personalized Number Plate Marketplace  
### Modular Monolith Version (Initial Release)

SmartPlate is a **clean, modular ASP.NET Core 9 Web API** that simulates the buying and selling of personalized number plates.  
This version is intentionally built as a **modular monolith** to allow fast development, easy maintenance, and effortless future scaling into microservices.

---

##  Features

###  Authentication & Users
- User registration & login  
- JWT-based authentication  
- Role-based authorization (**Admin**, **User**)  

###  Plates Management
- CRUD operations  
- Search & filtering  
- Ownership tracking  

###  Orders & Transactions
- Create purchase orders  
- Transfer ownership between users  

###  Architecture & Quality
- Modular structure (Users, Plates, Orders modules)  
- EF Core 9 + SQL Server (SQLite fallback for development)  
- FluentValidation for request validation  
- Serilog for structured logging  
- Swagger

---

##  Tech Stack

| Area        | Technology             |
|-------------|------------------------|
| Backend     | C#, ASP.NET Core 9     |
| Database    | SQL Server / SQLite    |
| ORM         | Entity Framework Core  |
| Auth        | JWT Authentication     |
| Validation  | FluentValidation       |
| Logging     | Serilog                |
| Testing     | xUnit                  |
| IDE         | VS Code                |

---

##  Architecture Overview

SmartPlate follows a **modular folder-by-feature structure** inside one monolithic Web API project.

```text
SmartPlate/
 ├── Controllers/
 │    ├── UserController.cs
 │    ├── PlateController.cs
 │    ├── OrderController.cs
 ├── Services/
 │    ├── UserService/
 │    ├── PlateService/
 │    ├── OrderService/
 ├── Data/
 │    ├── UserDbContext.cs
 │    ├── PlateDbContext.cs
 │    ├── OrderDbContext.cs
 ├── Models/
 │    ├── User.cs
 │    ├── Plate.cs
 │    ├── Order.cs
 ├── DTOs/
 └── Tests/
```

## Installation & Running

### 1. Clone & Enter Directory
```bash
git clone https://github.com/vladafursa/SmartPlate.git
cd SmartPlate


