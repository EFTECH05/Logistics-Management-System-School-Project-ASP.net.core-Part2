<img width="1357" height="669" alt="image" src="https://github.com/user-attachments/assets/75a3bd5f-4cb7-4d7c-93d5-09d954c5464f" />

🚀 Logistics Management System (GLMS)

An enterprise-level ASP.NET Core MVC application designed to modernize and replace manual logistics workflows for Logistica.

The system centralizes contract management, automates service requests, and integrates real-time currency conversion using external APIs. It also includes a full unit testing suite using xUnit to ensure system reliability and business rule validation.

📌 Project Status

🟡 Currently in Development – Part 2: Core Prototype & Unit Testing

👨‍💻 Developer
Name: Franklin Ngangu Simbi
Project: Enterprise Application Development
System:  Logistics Management System (GLMS)
🏗️ System Overview

The GLMS system replaces:

Spreadsheets 📊
Email-based workflows 📧
Manual contract tracking 📑

With a centralized enterprise web application.

🧱 Architecture

The system follows a Monolithic Architecture with clear separation of concerns:

🔹 Presentation Layer
ASP.NET Core MVC (Razor Views)
Responsive UI dashboards
🔹 Business Logic Layer
Controllers (Contracts, Service Requests, Users)
Business rules enforcement
🔹 Data Access Layer
Entity Framework Core
SQL Server Database
🧩 Core Modules
👤 Client Management
Stores client information
Tracks regions and contact details
📄 Contract Management
Handles agreements between clients and logistics provider
Status tracking: Active, Draft, On Hold, Expired
📦 Service Requests
Linked to contracts
Tracks operational requests
Stores cost in USD and ZAR
📌 Business Rules
✔ Only Active contracts can create service requests
✔ Expired or On Hold contracts are restricted
✔ All requests require valid descriptions and cost values
💱 External API Integration
Real-time USD → ZAR currency conversion
Implemented using HttpClient
Stores both original and converted values
🧪 Unit Testing (xUnit)

Testing framework used:

xUnit
EF Core InMemory Database
✔ Covered Tests:
Currency conversion validation
Service request creation logic
Contract validation rules
File validation security checks
📊 Results:
Tests: 4
Passed: 4
Failed: 0
⚡ Technologies Used
ASP.NET Core MVC (.NET 8 / .NET 10)
Entity Framework Core
SQL Server
xUnit Testing Framework
C# Async/Await
HttpClient API Integration
📈 Key Features
🔐 Secure MVC architecture
📦 Service request automation
📑 Contract lifecycle management
💱 Live currency conversion (USD → ZAR)
🧪 Automated unit testing
⚡ Async performance optimization
🧠 Software Engineering Principles Applied
Separation of Concerns (SoC)
Test-Driven Development (TDD)
Clean Architecture (Layered Design)
SOLID principles (Controller + Service structure)
Data validation & business rule enforcement
🚀 Future Improvements
Microservices migration
Azure deployment (App Service + SQL DB)
Role-based authentication upgrade (JWT / Identity)
API-first architecture
Mobile app integration
