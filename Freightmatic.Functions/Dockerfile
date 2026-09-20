#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/base AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Freightmatic.Functions/Freightmatic.Functions.csproj", "Freightmatic.Functions/"]
COPY ["Freightmatic.Application/Freightmatic.Application.csproj", "Freightmatic.Application/"]
COPY ["Freightmatic.Domain/Freightmatic.Domain.csproj", "Freightmatic.Domain/"]
COPY ["Freightmatic.Infrastructure/Freightmatic.Infrastructure.csproj", "Freightmatic.Infrastructure/"]
RUN dotnet restore "./Freightmatic.Functions/Freightmatic.Functions.csproj"
COPY . .
WORKDIR "/src/Freightmatic.Functions"
RUN dotnet build "./Freightmatic.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Freightmatic.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true