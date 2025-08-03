# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CompanyApi/CompanyApi.csproj", "CompanyApi/"]
COPY ["CompanyDataLayer/CompanyDataLayer.csproj", "CompanyDataLayer/"]
COPY ["CompanyRepositoryLayer/CompanyRepositoryLayer.csproj", "CompanyRepositoryLayer/"]
COPY ["CompanyServiceLayer/CompanyServiceLayer.csproj", "CompanyServiceLayer/"]
RUN dotnet restore "CompanyApi/CompanyApi.csproj"

COPY . .
WORKDIR "/src/CompanyApi"
RUN dotnet publish "CompanyApi.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CompanyApi.dll"]