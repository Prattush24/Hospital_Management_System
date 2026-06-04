# Hospital Management System API

A RESTful Hospital Management System built using **ASP.NET Core 8 Web API** and **SQL Server**, designed to manage patients, doctors, appointments, and analytical reports through a clean layered architecture.

## Features

### Patient Management

* Register new patients
* Retrieve patient details by ID
* View all active patients
* Update patient information
* Soft delete (deactivate) patients

### Doctor Management

* Add new doctors
* Retrieve doctors by specialization
* View available doctors
* Duplicate phone number validation

### Appointment Management

* Book appointments
* Cancel appointments
* View upcoming appointments
* View appointments by doctor
* View appointments by patient

### Reporting Module

* Appointment report with patient and doctor details
* Doctor appointment count report
* Revenue by specialization report
* Upcoming appointments within the next 7 days

---

## Technology Stack

| Technology           | Usage                        |
| -------------------- | ---------------------------- |
| ASP.NET Core 8       | REST API Framework           |
| C#                   | Backend Development          |
| SQL Server           | Database                     |
| Stored Procedures    | Data Access & Business Rules |
| Swagger UI           | API Documentation & Testing  |
| Dependency Injection | Service Registration         |
| Middleware           | Exception Handling & Logging |

---

## Architecture

The application follows a clean layered architecture:

```text
Client / Swagger
        │
        ▼
Controllers
        │
        ▼
Services
        │
        ▼
Repositories
        │
        ▼
SQL Server (Stored Procedures)
```

### Design Patterns Used

* Repository Pattern
* Service Layer Pattern
* Dependency Injection
* DTO Pattern
* Middleware Pattern

---

## Project Structure

```text
HospitalManagementSystem
│
├── Controllers
│   ├── PatientController
│   ├── DoctorController
│   ├── AppointmentController
│   └── ReportController
│
├── Services
│   ├── Interfaces
│   └── Implementations
│
├── Repositories
│   ├── Interfaces
│   └── Implementations
│
├── Models
│   ├── Entities
│   └── DTOs
│
├── Middleware
│   ├── GlobalExceptionMiddleware
│   └── RequestLoggingMiddleware
│
├── SQLScripts
│   └── Stored Procedures
│
└── Program.cs
```

---

## Database Modules

### Patients

* PatientId
* FullName
* DateOfBirth
* Gender
* PhoneNumber
* Email
* IsActive

### Doctors

* DoctorId
* FullName
* Specialization
* PhoneNumber
* ConsultationFee
* IsAvailable

### Appointments

* AppointmentId
* PatientId
* DoctorId
* AppointmentDate
* Status
* CancelledAt

---

## Business Validations

### Patient Rules

* Name, DOB, Gender and Phone Number are mandatory
* Date of Birth cannot be in the future
* Phone Number must be unique
* Email must be unique
* Only Male, Female or Other allowed

### Doctor Rules

* All fields are mandatory
* Consultation fee cannot be negative
* Phone Number must be unique
* Doctor availability defaults to true

### Appointment Rules

* Patient must exist and be active
* Doctor must exist and be available
* Appointment date must be in the future
* Cannot cancel an already cancelled appointment
* Status values:

  * Scheduled
  * Completed
  * Cancelled

---

## Middleware

### Global Exception Middleware

* Centralized exception handling
* Consistent JSON error responses
* SQL THROW error mapping
* Prevents raw exception exposure

### Request Logging Middleware

* Logs request method and path
* Tracks response status codes
* Measures execution time
* Useful for monitoring and debugging

---

## Steps to Run the Application

### Prerequisites

- .NET 8 SDK
- SQL Server
- SQL Server Management Studio (SSMS)
- Visual Studio 2022 or VS Code

### Setup

1. Clone the repository.

```bash
git clone <repository-url>
cd HospitalManagementSystem
```

2. Create the database in SQL Server.

3. Execute all SQL scripts:
   - Tables
   - Stored Procedures
   - Sample Data (optional)

4. Update the connection string in `appsettings.json`.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

5. Restore NuGet packages.

```bash
dotnet restore
```

6. Build the project.

```bash
dotnet build
```

7. Run the application.

```bash
dotnet run
```

8. Open Swagger UI in your browser.

```text
https://localhost:{port}/swagger
```

9. Test the APIs using Swagger.

### Execution Order

1. Register Patient
2. Add Doctor
3. Book Appointment
4. View/Cancel Appointment
5. Generate Reports



## API Documentation

Swagger UI is enabled for testing and exploring all endpoints.

After running the application:

```text
https://localhost:{port}/swagger
```

---

## Key Highlights

* ASP.NET Core 8 REST API
* SQL Server Database
* 16+ Stored Procedures
* Repository & Service Layer Architecture
* Global Exception Handling
* Request Logging Middleware
* Swagger Documentation
* DTO-Based API Design
* Custom SQL Error Handling
* Production-Oriented Project Structure

---

## Future Enhancements

* JWT Authentication & Authorization
* Role-Based Access Control (Admin/Doctor/Patient)
* Pagination & Filtering
* Email Notifications
* Appointment Conflict Prevention
* Unit Testing & Integration Testing
* Docker Containerization

---

## Author

Developed as a backend learning and portfolio project to demonstrate REST API development, SQL Server stored procedures, layered architecture, middleware implementation, and enterprise application design using ASP.NET Core 8.

