# AgroCita

Plataforma web de gestión y seguimiento de asistencia agrícola.

## Backend

API REST desarrollada con ASP.NET Core 10, C#, Entity Framework Core y PostgreSQL.

## Arquitectura

El backend sigue los principios de Clean Architecture:

- **AgroCita.API:** endpoints REST, controladores y autenticación.
- **AgroCita.Application:** casos de uso y lógica de aplicación.
- **AgroCita.Domain:** entidades y reglas del dominio.
- **AgroCita.Infrastructure:** persistencia, PostgreSQL y servicios externos.

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- C#
- Entity Framework Core
- PostgreSQL
- Npgsql
- JWT

## Estructura

El backend se encuentra en `backend-dotnet/`.

La documentación del proyecto y el modelo entidad-relación están en `backend-dotnet/doc/`.
