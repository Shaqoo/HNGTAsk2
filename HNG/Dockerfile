# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything into the container
COPY . ./

# Restore dependencies for the solution
RUN dotnet restore HNGTAsk2.sln

# Publish the app to a release folder
RUN dotnet publish HNGTAsk2.sln -c Release -o out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /app/out ./

# Tell ASP.NET to listen on all interfaces and port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "HNG.dll"]
