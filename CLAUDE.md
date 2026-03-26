# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Run the application
```bash
dotnet run --project src/AppHost
```
This starts the .NET Aspire orchestration host, which launches the Aspire dashboard, the Web API, and the frontend dev server.

### Build
```bash
dotnet build
```

### Run all tests
```bash
dotnet test
```

### Run a specific test project
```bash
dotnet test tests/Domain.UnitTests
dotnet test tests/Application.UnitTests
dotnet test tests/Application.FunctionalTests
dotnet test tests/Infrastructure.IntegrationTests
```

### Frontend (ClientApp)
```bash
cd src/Web/ClientApp
npm install
npm start          # Dev server
npm run build      # Production build
npm run lint       # ESLint
npm run generate-api  # Regenerate API client from OpenAPI spec
```

### Scaffold new CQRS handlers (Clean Architecture templates)
```bash
# Command returning int
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int

# Query
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm

# Install template if needed
dotnet new install Clean.Architecture.Solution.Template::10.8.0
```

## Architecture

This is a **Clean Architecture** solution using **.NET Aspire** for orchestration.

### Layer overview

```
Domain → Application → Infrastructure
                    ↘ Web (API + React SPA)
AppHost (orchestrates all services via Aspire)
ServiceDefaults (shared OpenTelemetry, health checks, resilience)
```

- **Domain** (`src/Domain/`) — Entities (`TodoList`, `TodoItem`), value objects (`Colour`), domain events, no external dependencies.
- **Application** (`src/Application/`) — CQRS via MediatR. Handlers organised by feature (TodoLists, TodoItems, WeatherForecasts). MediatR pipeline behaviors handle validation (FluentValidation), authorization, logging, performance monitoring, and exception handling in that order.
- **Infrastructure** (`src/Infrastructure/`) — EF Core with SQLite. Two SaveChanges interceptors: `AuditableEntityInterceptor` (auto-stamps Created/LastModified) and `DispatchDomainEventsInterceptor` (dispatches domain events after save).
- **Web** (`src/Web/`) — Minimal API endpoints discovered automatically via `IEndpointGroup`. Hosts the React SPA. OpenAPI via Scalar.
- **AppHost** (`src/AppHost/`) — Aspire distributed application host; wires up the SQLite database, web API, and frontend JS service.

### Request flow

```
HTTP → Minimal API endpoint → MediatR pipeline (validation → authz → logging → perf) → Handler → EF Core → Interceptors → DB
                                                                                                              ↓
                                                                                                    Domain events dispatched
```

### Frontend

React 19 + Vite SPA located at `src/Web/ClientApp/`. API client is generated from the OpenAPI spec via NSwag (`npm run generate-api`). Uses react-router-dom v7 with protected routes and an `AuthContext` for auth state.

### Service name constants

Defined in `src/Shared/Services.cs`: `webfrontend`, `webapi`, `SumployableDb`.

### Testing strategy

| Project | Scope |
|---|---|
| `Domain.UnitTests` | Value objects, entities |
| `Application.UnitTests` | Handlers, behaviors |
| `Application.FunctionalTests` | Feature-level, uses `TestAppHost` |
| `Infrastructure.IntegrationTests` | Database / EF Core |
| `Web.AcceptanceTests` | BDD (Reqnroll + Playwright) |

All tests use NUnit + Shouldly assertions. Mocking with Moq.

### Central build configuration

- `Directory.Build.props` — target framework, nullable, implicit usings, warning levels
- `Directory.Packages.props` — all NuGet versions (central package management; do not set versions in individual `.csproj` files)
