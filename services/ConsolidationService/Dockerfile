# ============================
# 1. Build Stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "./services/ConsolidationService/ConsolidationService.csproj"

RUN dotnet build "./services/ConsolidationService/ConsolidationService.csproj" -c Release -o /app/build

RUN dotnet publish "./services/ConsolidationService/ConsolidationService.csproj" -c Release -o /app/publish

# ============================
# 2. Runtime Stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "ConsolidationService.dll"]
