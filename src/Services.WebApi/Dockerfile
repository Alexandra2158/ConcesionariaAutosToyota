# ── Stage 1: Build ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY src/Services.WebApi/*.csproj ./src/Services.WebApi/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/

RUN dotnet restore src/Services.WebApi/ConcesionariaAutosToyota.Trade.Services.WebApi.csproj

# Copiar todo el código fuente y compilar
COPY src/ ./src/
RUN dotnet publish src/Services.WebApi/ConcesionariaAutosToyota.Trade.Services.WebApi.csproj \
    -c Release -o /app/publish 

# ── Stage 2: Runtime ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Variables de entorno para producción
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ConcesionariaAutosToyota.Trade.Services.WebApi.dll"]
