# .NET 8 Application

This is a sample application built using .NET 8. It follows a layered architecture pattern with the Unit of Work pattern for efficient database management.

## Prerequisites

- .NET 8 SDK
- A code editor like visual studio, or any IDE that supports .NET development
- A SQL Server
- Docker for containerized deployment

## Getting Started

### Clone the Repository

```bash:no-line-numbers
git clone https://github.com/devmu-hub/ProductsCRUD
```

### Building the Application

This will restore dependencies and build the project.

```bash:no-line-numbers
dotnet build
```

### Running the Application

navigate to YourFolderPath\ProductsCRUD.WebApi

```bash:no-line-numbers
dotnet run
```

The application should now be running locally, typically accessible at:

```bash:no-line-numbers
http://localhost:5144/swagger/index.html
```

### Running Tests

You can run unit and integration tests using:

Navigate to YourFolderPath\ProductsCRUD.Tests

```bash:no-line-numbers
dotnet test
```

### Environments
- appsettings.Development for development
- appsettings for production


### Docker Deployment

- Navigate to the cloned folder
- Use this commands to build the images and start the services.

```bash:no-line-numbers
docker-compose build
docker-compose up
```
