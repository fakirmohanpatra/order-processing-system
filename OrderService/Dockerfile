FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore to leverage layer caching
COPY OrderService.API/OrderService.API.csproj OrderService.API/
RUN dotnet restore OrderService.API/OrderService.API.csproj

# Copy everything and publish
COPY . .
WORKDIR /src/OrderService.API
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
