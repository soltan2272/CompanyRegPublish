# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ONLY project files first (optimizes layer caching)
COPY ["CompanyApi/CompanyApi.csproj", "CompanyApi/"]
COPY ["CompanyDataLayer/CompanyDataLayer.csproj", "CompanyDataLayer/"]
COPY ["CompanyRepositoryLayer/CompanyRepositoryLayer.csproj", "CompanyRepositoryLayer/"]
COPY ["CompanyServiceLayer/CompanyServiceLayer.csproj", "CompanyServiceLayer/"]

# Restore all NuGet packages
RUN dotnet restore "CompanyApi/CompanyApi.csproj"

# Copy everything else
COPY . .

# Publish the API project
RUN dotnet publish "CompanyApi/CompanyApi.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CompanyApi.dll"]  # Your startup DLL