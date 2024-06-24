FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 443

# Copy the project file and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY MilkStore/MilkStore.csproj MilkStore/
# Copy the application code
COPY . .
RUN dotnet restore MilkStore/MilkStore.csproj

# Build the application
WORKDIR /src/MilkStore
RUN dotnet build "MilkStore.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MilkStore.csproj" -c Release -o /app/publish
WORKDIR /app/publish
# Use the base image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Run the application when the container starts

ENTRYPOINT ["dotnet", "MilkStore.dll"]