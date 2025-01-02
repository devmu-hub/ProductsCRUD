FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductsCRUD.WebApi/ProductsCRUD.WebApi.csproj", "src/ProductsCRUD.WebApi/"]
COPY ["ProductsCRUD.WebApi.HTTPModels/ProductsCRUD.WebApi.HTTPModels.csproj", "src/ProductsCRUD.WebApi.HTTPModels/"]
COPY ["ProductsCRUD.Application/ProductsCRUD.Application.csproj", "src/ProductsCRUD.Application/"]
COPY ["ProductsCRUD.Domain/ProductsCRUD.Domain.csproj", "src/ProductsCRUD.Domain/"]
COPY ["ProductsCRUD.Data.EntityFrameworkCore/ProductsCRUD.Data.EntityFrameworkCore.csproj", "src/ProductsCRUD.Data.EntityFrameworkCore/"]
RUN dotnet restore "src/ProductsCRUD.WebApi/ProductsCRUD.WebApi.csproj"

COPY . .
WORKDIR "/src/ProductsCRUD.WebApi"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsCRUD.WebApi.dll"]


