# .NET Core Layered Architecture Project

This repository demonstrates a CRUD application built using a layered architecture pattern in .NET Core, implementing Unit of Work, repository patterns.

## Project Structure

The project is divided into the following layers:

### 1. Presentation Layer

- **Location**: ProductsCRUD.WebApi
- **Purpose**: APIs.
- **Components**: API Controllers - Request/response models - Mapper profiles

### 2. Application Layer

- **Location**: ProductsCRUD.Application
- **Purpose**: Contains business logic and coordinates data flow between the domain and presentation layers.
- **Components**: DTOs - Application services - Validation logic

### 3. Domain Layer

- **Location**: ProductsCRUD.Domain
- **Purpose**: Defines the core entities, interfaces, and domain logic.

### 4. Infrastructure Layer

- **Location**: ProductsCRUD.Data.EntityFrameworkCore
- **Purpose**: Implements persistence and external services.
- **Components**: ApplicationDbContext - Repository implementations - Unit of Work implementation

### 5. Xunit Tests

- **Location**: ProductsCRUD.Tests
- **Purpose**: Implement unit and integration tests

### Folder Structure

```bash:no-line-numbers
ProductsCRUD/
├── ProductsCRUD.WebApi/                     # Presentation layer
├── ProductsCRUD.WebApi.HTTPModels           # Requests/Responses
├── ProductsCRUD.Application/                # Application layer
├── ProductsCRUD.Domain/                     # Domain layer
├── ProductsCRUD.Data.EntityFrameworkCore/   # Infrastructure layer
├── ProductsCRUD.Tests                       # xunit tests
├── Dockerfile
├── docker-compose
├── ProductsCRUD.sln                         # Solution file
```
### Prerequisites

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

### Login Account

You can use this default user account for authentication:

Username: admin
Passowrd: admin

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
