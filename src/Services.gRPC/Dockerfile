FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src/Services.gRPC/*.csproj ./src/Services.gRPC/
COPY src/Domain/*.csproj ./src/Domain/
COPY src/Application/*.csproj ./src/Application/
COPY src/Infrastructure/*.csproj ./src/Infrastructure/
RUN dotnet restore src/Services.gRPC/ConcesionariaAutosToyota.Trade.Services.gRPC.csproj
COPY src/ ./src/
RUN dotnet publish src/Services.gRPC/ConcesionariaAutosToyota.Trade.Services.gRPC.csproj \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8081
COPY --from=build /app/publish .
EXPOSE 8081
ENTRYPOINT ["dotnet", "ConcesionariaAutosToyota.Trade.Services.gRPC.dll"]
