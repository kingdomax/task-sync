# 1. Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app

# 2. Copy .csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# 3. Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# 4. Use ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# 5. Copy the published app from build stage
COPY --from=builder /app/out .

# 6. Expose the HTTP port your app runs on (from launchSettings.json it's 5070, but ASP.NET automatically 8080)
EXPOSE 8080

# 7. Set the entrypoint
ENTRYPOINT ["dotnet", "TaskSync.dll"]