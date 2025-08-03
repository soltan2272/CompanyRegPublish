#!/bin/sh

# Install .NET 8 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100

# Restore dependencies
dotnet restore

# Build and publish
dotnet publish -c Release -o output