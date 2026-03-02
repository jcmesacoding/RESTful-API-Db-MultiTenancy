# 🚀 RESTful API - Multi-Tenant Architecture (.NET 8)

API REST desarrollada en **ASP.NET Core 8** implementando:

- 🔐 Autenticación con JWT
- 🛡️ Autorización por Roles
- 🏢 Arquitectura Multi-Tenant
- 🧱 Clean Architecture (Domain / Infrastructure / Api)
- 🔒 Encriptación de contraseñas con BCrypt
- 🗄️ Entity Framework Core
- 🧩 Migraciones
- 🔄 Cambio de contraseña seguro

---

## 🏗️ Arquitectura del Proyecto

El proyecto está estructurado siguiendo principios de **Clean Architecture**:

---

## 🔐 Autenticación y Seguridad

- JWT Token basado en Claims
- Roles (Admin / User)
- TenantId como Claim
- BCrypt para hash de contraseñas
- Endpoints protegidos con `[Authorize]`

---

## 🧪 Endpoints Principales

### 🔑 Registro

Body:

```json
{
  "username": "admin",
  "password": "1234",
  "role": "Admin",
  "tenantId": "Tenant1"
}
```
### 🔐 Login
POST /Auth/Login
{
  "token": "JWT_TOKEN"
}
### 👤 Profile (Protegido)
GET /Auth/Profile
Authorization: Bearer TU_TOKEN

### 🔁 Cambio de contraseña
POST /Auth/CambioDeClave


## 🗄️ Base de Datos

Entity Framework Core

Migraciones habilitadas

Soporte multi-tenant mediante campo TenantId

## 🧰 Tecnologías Utilizadas

.NET 8

ASP.NET Core Web API

Entity Framework Core

SQL Server

JWT Authentication

BCrypt.Net

Clean Architecture

## Link de Explicacion de Youtube: [https://youtu.be/x7pG0mEPkls]
