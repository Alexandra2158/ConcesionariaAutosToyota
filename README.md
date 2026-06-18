# ConcesionariaAutosToyota.Trade — 1er Avance Guía 14

## 🏗️ Arquitectura
Microservicios con .NET 8 + Clean Architecture + CQRS + JWT + gRPC + Event-Driven (RabbitMQ)

## 🚀 Inicio rápido

### Desarrollo local
```bash
# 1. Levantar servicios de infraestructura
docker compose up -d sqlserver rabbitmq

# 2. Ejecutar API REST
cd src/Services.WebApi
dotnet run
# Swagger en: http://localhost:5000/swagger

# 3. Ejecutar servicio gRPC
cd src/Services.gRPC
dotnet run
```

### Docker Compose completo
```bash
docker compose up -d
# API REST: http://localhost:5000/swagger
# RabbitMQ Management: http://localhost:15672 (guest/guest)
```

### Pruebas unitarias
```bash
dotnet test tests/ConcesionariaAutosToyota.Tests/ --verbosity normal
```

### Kubernetes (Minikube)
```bash
# Construir imagen
docker build -f src/Services.WebApi/Dockerfile -t toyota-webapi:latest .

# Aplicar manifiestos
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml
kubectl apply -f k8s/sqlserver-deployment.yaml
kubectl apply -f k8s/rabbitmq-deployment.yaml
kubectl apply -f k8s/webapi-deployment.yaml

# Verificar pods
kubectl get pods
```

## 🔐 Autenticación
```
POST /api/Auth/login
{ "username": "admin", "password": "Admin1234!" }
```
Copia el token y usa: `Authorization: Bearer <token>`

## 📋 Criterios Guía 14
| Criterio | Estado | Evidencia |
|---|---|---|
| Diagrama de arquitectura | ✅ | docs/arquitectura/ |
| API REST + Swagger | ✅ | /swagger |
| gRPC implementado | ✅ | src/Services.gRPC |
| CQRS Commands/Queries | ✅ | src/Application/UseCases |
| Microservicio SQL | ✅ | Infrastructure/Persistence |
| JWT + Roles + Claims | ✅ | AuthController |
| Event-Driven (RabbitMQ) | ✅ | Publisher + Consumer |
| xUnit + Moq (6 tests) | ✅ | tests/ |
| Dockerfile + Docker Compose | ✅ | raíz del proyecto |
| K8s + ConfigMap + Secret | ✅ | k8s/ |
| Pipeline CI/CD | ✅ | .github/workflows/ci.yml |
