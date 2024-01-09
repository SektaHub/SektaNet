# Use the ASP.NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building and publishing
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["backend.csproj", "./"]
RUN dotnet restore "backend.csproj"

# Copy the remaining source code
COPY . .
RUN ls -la # For debugging: this will list files present in the image

# Install Entity Framework tools
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Apply Entity Framework Core Migrations
#RUN dotnet ef database update --project . --startup-project .

# Define a stage for publishing the application
FROM build AS publish
RUN dotnet publish "backend.csproj" -c Release -o /app/publish

# Create the final image using the base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define the entrypoint script to run during container startup
ENTRYPOINT ["dotnet", "backend.dll"]