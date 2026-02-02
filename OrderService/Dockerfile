# -----------------------------
# Stage 1: Build
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only csproj first to restore dependencies (caching)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Publish the app in Release mode to /app folder
RUN dotnet publish -c Release -o /app

# -----------------------------
# Stage 2: Runtime
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published app from build stage
COPY --from=build /app .

# Expose default ASP.NET port
EXPOSE 80

# Entry point for the container
ENTRYPOINT ["dotnet", "OrderService.dll"]
