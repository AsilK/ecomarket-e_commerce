# ShopApp

A full-featured e-commerce application built with ASP.NET Core MVC.

## Technologies
- .NET 8 (LTS)
- Entity Framework Core 8.0
- SQLite
- Bootstrap 5
- ASP.NET Core Identity

## Getting Started

Follow these steps to run the project locally.

### Prerequisites
- .NET 8 SDK

### Installation

1.  Clone the repository or download the source.
2.  Open a terminal in the project root.
3.  Initialize the database (SQLite):

    ```bash
    dotnet ef database update --project ShopApp.WebUI --context ApplicationIdentityDbContext
    dotnet ef database update --project ShopApp.DataAccess --startup-project ShopApp.WebUI --context ShopContext
    ```

### Running the Application

Navigate to the WebUI directory and run the project:

```bash
cd ShopApp.WebUI
dotnet run
```

The application will be available at **http://localhost:5000**.

## Configuration
The project uses SQLite by default (`ShopAppIdentity.db` and `ShopAppData.db`). No additional database installation is required.
