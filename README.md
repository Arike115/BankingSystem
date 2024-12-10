# BankingSystem
The Banking Transaction Management System is a robust and scalable backend application built using the .NET Core stack. The system is designed to manage essential banking functionalities such as account creation, transactions (credit, debit, and transfers), and transaction history management. It follows modern architectural patterns and best practices to ensure reliability, performance, and security.

The system implements key backend principles such as CQRS, Entity Framework Core for data persistence, JWT Authentication for security, and structured logging for monitoring.
Key Features
Account Management

Create bank accounts with account holder details, balance, and unique account numbers.
Fetch account details using unique account identifiers.

Transaction Management

Perform credit and debit operations.
Implement transfer functionality between accounts.
Update account balances automatically after each transaction.
Maintain transaction history for auditing purposes.
Transaction History

Fetch account transaction history by account ID.
Support pagination for large datasets.
Data Integrity and Validation

Secure API endpoints with JWT (JSON Web Token) authentication.
Use a secure secret key for token generation and validation.
Error Handling and Logging

Implement centralized error handling middleware.
Log system operations and errors using Serilog.
Provide descriptive error messages for debugging.
Scalability and Performance

Optimize database queries using Entity Framework Core.
Support concurrent transactions with proper isolation levels.
Enable caching for frequently accessed data (e.g., using Redis).




Technologies Used
.NET 8: Core framework for building backend APIs.
Entity Framework Core: ORM for database interaction.
SQL Server: Relational database for storing account and transaction data.
JWT Authentication: Secure access to API endpoints.
MediatR: Implements the CQRS pattern for better separation of concerns.
Serilog: Structured logging for monitoring and troubleshooting.
Mapster: Lightweight object-to-object mapping for DTOs.
Dependency Injection: Manages service lifetimes and decouples system components.
Swagger (OpenAPI): API documentation for easy testing and consumption.


Core Architecture
The system follows a clean and modular architecture:

Domain Layer: Contains business entities and core logic (e.g., Account, Transaction).
Application Layer: Contains CQRS handlers, DTOs, and business rules.
Infrastructure Layer: Manages database context, repositories, and external services.
API Layer: Exposes RESTful endpoints for client interaction.






Project Structure
BankingSystem/
│
├── Domain/
│   ├── Entities/
│   │   ├── Account.cs
│   │   ├── Transaction.cs
│   │
│   ├── Enums/
│   │   ├── TransactionStatus.cs
│   │
│   └── Models/
│       ├── BaseResponse.cs
│       ├── GetAllAccountDto.cs
│
├── Application/
│   ├── Commands/
│   │   ├── CreateAccountCommand.cs
│   │   ├── TransactionCommand.cs
│   │
│   ├── Query/
│   │   ├── GetAccountDetailsQuery.cs
│   │   ├── GetAllAccountQuery.cs
│   │
│   └── Handlers/
│       ├── CreateAccountCommandHandler.cs
│       ├── TransactionCommandHandler.cs
│       ├── GetAccountDetailsQueryHandler.cs
│
├── Infrastructure/
│   ├── Persistence/
│   │   ├── BankingDbContext.cs
│   │   ├── Configurations/ (EF configurations)
│   │
│   ├── Services/
│   │   ├── JwtHandler.cs
│
├── API/
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── TransactionController.cs
│   │
│   └── Middleware/
│       ├── ExceptionHandlingMiddleware.cs
│
├── Startup.cs
├── appsettings.json
└── README.md
