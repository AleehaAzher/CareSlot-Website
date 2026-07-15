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

<img width="975" height="630" alt="image" src="https://github.com/user-attachments/assets/dfe0c7a3-e241-4520-bb08-213446187a06" />
<img width="975" height="437" alt="image" src="https://github.com/user-attachments/assets/acbbcd62-f064-42d2-acc9-0327e1fa9ed3" />
<img width="975" height="434" alt="image" src="https://github.com/user-attachments/assets/4a9734f1-4900-4c8b-bcfe-d9520930b13f" />
<img width="975" height="437" alt="image" src="https://github.com/user-attachments/assets/101525e2-49ad-4159-8cff-aa449a6980e1" />
<img width="975" height="436" alt="image" src="https://github.com/user-attachments/assets/a4ef8406-cda6-491d-b260-fe6c2e470ab8" />
<img width="975" height="431" alt="image" src="https://github.com/user-attachments/assets/6726df62-5ffe-49ac-a583-09e3efc3d1df" />
<img width="975" height="436" alt="image" src="https://github.com/user-attachments/assets/3f0d65d0-badd-43c8-9a97-d3b3b2a91440" />
<img width="975" height="438" alt="image" src="https://github.com/user-attachments/assets/48f3293f-5638-486e-a75f-8fdd01c01724" />
<img width="1892" height="898" alt="image" src="https://github.com/user-attachments/assets/f0bc2909-c2cd-4c53-a9d0-fa2e8cfadea4" />
<img width="1913" height="902" alt="image" src="https://github.com/user-attachments/assets/2c521afa-d269-485c-b1ec-ac71c921b823" />

Potential enhancements for the next phase include:

- Email and SMS appointment reminders
- Real-time notifications for doctors and patients
- Online payment integration
- Advanced analytics dashboards for admins
- Cloud deployment and containerization

## Author

Name: Aleeha Azher" 
