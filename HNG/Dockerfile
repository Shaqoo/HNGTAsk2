# Stage 1: Build Environment
# Use the .NET 8 SDK to build the application.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and project files.
# This leverages Docker's layer caching, so 'dotnet restore' is only re-run when projects change.
COPY HNGTAsk2.sln .
COPY HNG/HNG.csproj ./HNG/
RUN dotnet restore HNGTAsk2.sln

# Copy the rest of the source code.
COPY . ./

# Publish the project.
WORKDIR /app/HNG
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime Environment
# Use the smaller ASP.NET runtime image for the final, production-ready container.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set the environment variable for the URLs the app will listen on.
ENV ASPNETCORE_URLS=http://+:8080
# Expose the port the app will run on.
EXPOSE 8080

# Define the entry point for the container. This command starts your application.
ENTRYPOINT ["dotnet", "HNG.dll"]