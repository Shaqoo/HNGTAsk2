# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy solution and project files for caching
COPY HNGTAsk2.sln ./
COPY HNG/*.csproj ./HNG/
RUN dotnet restore HNGTAsk2.sln

# Copy the rest of the source code
COPY . ./

# Publish the app to a release folder
RUN dotnet publish HNGTAsk2.sln -c Release -o out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build-env /app/out .

# Tell ASP.NET to listen on all interfaces and port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "HNGTAsk2.dll"]
