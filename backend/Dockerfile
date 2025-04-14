# 1. Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# 2. Copy the solution file and all project folders
COPY . .

# 3. Restore using the .sln file
WORKDIR /src/backend
RUN dotnet restore TaskSync.sln
#RUN dotnet restore backend/TaskSync.sln

# 4. Publish the main app project (TaskSync.csproj)
WORKDIR /src/backend
RUN dotnet publish TaskSync.csproj -c Release -o /app/publish

# 5. Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskSync.dll"]
