# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["backend.csproj", "."]
RUN dotnet restore "backend.csproj"

# Copy the remaining source code
COPY . .

# Build the application
WORKDIR "/src/."
RUN dotnet build "backend.csproj" -c Release -o /app/build

# Define a stage for publishing the application
FROM build AS publish
RUN dotnet publish "backend.csproj" -c Release -o /app/publish

# Create the final image using the base image
FROM base AS final
WORKDIR /app

# Copy the published files from the publish stage
COPY --from=publish /app/publish .

# Copy the entrypoint script (assuming it's in the backend directory)
COPY entrypoint.sh .
COPY install-pgvector.sh .

# Set permissions for the entrypoint script
#RUN chmod +x ./entrypoint.sh

# Define the entrypoint script to run during container startup
ENTRYPOINT ["sh", "./entrypoint.sh"]
