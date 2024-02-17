# Trucking Company Management System (TCMS)

## Project Overview

TCMS is a comprehensive system designed for managing the day-to-day operations of a trucking company, including shipment tracking, employee management, vehicle maintenance, and payroll systems.

## Prerequisites

- Visual Studio 2022
- .NET 8.0 SDK or later
- SQLite
- Git

## Setup Instructions

### Step 1: Clone the Repository

Clone the TCMS repository to your local machine using the following command in Git:

```sh
git clone https://github.com/ThomasJMichael/TruckingCompanyManagementSystem.git
```

Navigate to the cloned directory:

```sh
cd TCMS
```

### Step 2: Open the Project in Visual Studio 2022

Open Visual Studio 2022 and use the `File > Open > Project/Solution` menu to open the `TruckingCompanyManagementSystem.sln` solution file located in the root of the cloned repository.

### Step 3: Install Dependencies

Visual Studio should automatically restore all NuGet packages required for the project. If it doesn't, you can restore them manually by right-clicking on the solution and selecting `Restore NuGet Packages`.

### Step 4: Configure the Application

Update the `appsettings.json` file in the `TCMS.API` project with your database connection strings and other necessary configuration settings.

### Step 5: Database Setup

Use the Package Manager Console to apply Entity Framework migrations and set up your database schema. You can open the Package Manager Console from `Tools > NuGet Package Manager > Package Manager Console` in Visual Studio.

In the Package Manager Console, select the `TCMS.Data` project as the Default Project and run:

```sh
Update-Database
```

This command applies any pending migrations to the database configured in your `appsettings.json` file.

### Step 6: Build the Solution

Build the solution by using the `Build > Build Solution` menu in Visual Studio or by pressing `Ctrl+Shift+B`.

### Step 7: Run the Application

Start the application by pressing `F5` or by using the `Debug > Start Debugging` menu. This will launch the API and open your default web browser to the Swagger UI, where you can interact with the API endpoints.

## Additional Information

- The Swagger UI is accessible at the `/swagger` endpoint when the application is running.
- The `TCMS.API` project is configured to seed some initial data into the database for testing purposes.
