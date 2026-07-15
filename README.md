"# CareSlot – Healthcare Appointment Management System

A professional healthcare appointment management web application built with ASP.NET Core MVC. The system supports role-based workflows for admins, doctors, and patients, covering registration, appointment scheduling, prescription handling, and invoice-related processes.

## Overview

CareSlot is designed to simplify healthcare appointment management by providing a centralized web platform for clinics and patients. It helps reduce manual coordination by enabling users to manage profiles, book appointments, track appointment status, create prescriptions, and review invoice records through a single application.

## Features

The implementation currently includes:

- User authentication and cookie-based authorization
- Patient registration and profile management
- Doctor management and approval workflow
- Appointment booking, updating, and status tracking
- Appointment history and request-based views for doctors and patients
- Prescription creation and prescription detail management
- Invoice generation for completed appointments
- Dashboard views for different user roles
- SQL Server-backed data persistence through Entity Framework Core
- Responsive Razor-based user interface with custom web assets

## Technologies Used

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- Razor Views
- HTML, CSS, and JavaScript
- Dependency Injection
- Cookie Authentication

## Project Structure

- InternshipFinalProject: Main ASP.NET Core MVC web application containing controllers, views, and configuration.
- InternshipFinalProject_Application: Application-layer services and view models.
- InternshipFinalProject_Core: Core domain models, DTOs, helper classes, and repository interfaces.
- InternshipFinalProject_Infrastructure: EF Core database context, repositories, and migrations.
- wwwroot: Static assets such as CSS, JavaScript, images, and uploads.

## Installation & Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```

2. Open the solution file in Visual Studio 2022 or your preferred .NET IDE.

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Update the SQL Server connection string in the application configuration file:
   ```json
   "ConnectionStrings": {
     "ConnectionString": "Server=YOUR_SERVER_NAME;Database=InternshipProjectDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

5. Apply the existing Entity Framework Core migrations to create the database:
   ```bash
   dotnet ef database update --project InternshipFinalProject_Infrastructure --startup-project InternshipFinalProject
   ```
   If the EF Core CLI is not installed yet, run:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

6. Run the application:
   ```bash
   dotnet run --project InternshipFinalProject
   ```

7. Open the app in your browser using the local URL shown in the terminal.

## Screenshots

### Dashboard
Coming soon.

### Appointment Workflow
Coming soon.

### Prescription & Invoice Views
Coming soon.

## Future Improvements

Potential enhancements for the next phase include:

- Email and SMS appointment reminders
- Real-time notifications for doctors and patients
- Online payment integration
- Advanced analytics dashboards for admins
- Cloud deployment and containerization

## Author

Name: Aleeha Azher" 
