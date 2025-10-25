# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies first to leverage Docker layer caching.
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application's source code.
COPY . ./

# Publish the application, creating a release build.
RUN dotnet publish -c Release -o out

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Set the environment variable for the URLs the app will listen on.
ENV ASPNETCORE_URLS=http://+:8080
# Expose the port the app will run on.
EXPOSE 8080

# Define the entry point for the container. This command starts your application.
ENTRYPOINT ["dotnet", "HNG.dll"]
