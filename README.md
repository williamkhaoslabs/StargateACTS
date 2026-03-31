# 🚀 Stargate — Astronaut Career Tracking System (ACTS)

ACTS maintains a complete record of all people who have served as Astronauts. When serving as an Astronaut, a person's job (Duty) is tracked by Rank, Title, and Start/End Dates. The master list of People is managed by an external service not controlled by ACTS.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Running the API](#running-the-api)
- [Running the Frontend](#running-the-frontend)
- [Running Tests](#running-tests)
- [CI / CD](#ci--cd)
- [API Reference](#api-reference)
- [Business Rules](#business-rules)
- [Data Model](#data-model)
- [Architecture & Design Patterns](#architecture--design-patterns)
- [Code Review Findings](#code-review-findings)
- [Process Logging](#process-logging)

---

## Prerequisites

| Tool | Version | Install |
|------|---------|---------|
| .NET SDK | 10.0+ | https://dotnet.microsoft.com/download |
| Node.js | 22.x | https://nodejs.org |
| Angular CLI | 21.x | `npm install -g @angular/cli` |
| EF Core CLI | Latest | `dotnet tool install --global dotnet-ef` |
| act (optional) | Latest | `brew install act` |

Verify your environment:

```bash
dotnet --version
node --version
ng version
dotnet ef --version
```

---

## Project Structure

```
StargateACTS/
├── .github/
│   └── workflows/
│       └── ci.yml                  
├── .gitignore
├── StargateACTS.sln
│
├── StargateApi/                     
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Controllers/
│   │   ├── BaseResponse.cs
│   │   ├── ControllerBaseExtensions.cs
│   │   ├── PersonController.cs
│   │   └── AstronautDutyController.cs
│   ├── Middleware/
│   │   └── ExceptionHandlingMiddleware.cs
│   └── Business/
│       ├── Data/
│       │   ├── Person.cs
│       │   ├── AstronautDetail.cs
│       │   ├── AstronautDuty.cs
│       │   └── StargateContext.cs
│       ├── Dtos/
│       │   └── PersonAstronaut.cs
│       ├── Queries/
│       │   ├── GetPeople.cs
│       │   ├── GetPersonByName.cs
│       │   └── GetAstronautDutiesByName.cs
│       ├── Commands/
│       │   ├── CreatePerson.cs
│       │   └── CreateAstronautDuty.cs
│       └── Migrations/
│
├── StargateAPI.Tests/               
│   └── StargateAPI.Tests.csproj
│
└── stargate-ui/                     
    └── src/app/
        ├── app.component.ts
        ├── app.config.ts
        ├── app.routes.ts
        ├── models/
        │   ├── person-astronaut.model.ts
        │   ├── astronaut-duty.model.ts
        │   └── api-responses.model.ts
        ├── services/
        │   └── stargate.service.ts
        └── components/
            ├── people-list/
            ├── person-detail/
            ├── astronaut-duties/
            ├── add-person/
            └── add-duty/
```

---

## Getting Started

### 1. Clone the repository

```bash
git clone git@github.com:williamkhaoslabs/StargateACTS.git
cd StargateACTS
```

### 2. Restore and build .NET

```bash
dotnet restore
dotnet build
```

### 3. Generate the database

```bash
cd StargateApi
dotnet ef database update
```

This applies all migrations and creates `stargate.db` in the API directory.

### 4. Install Angular dependencies

```bash
cd ../stargate-ui
npm install
```

---

## Running the API

```bash
cd StargateApi
dotnet run
```

The API starts on `https://localhost:7015` by default. The port is printed in the terminal output.

Swagger UI is available at:

```
https://localhost:{port}/swagger
```

All 5 endpoints are documented and testable from the Swagger UI.

---

## Running the Frontend

```bash
cd stargate-ui
ng serve
```

The Angular dev server starts at `http://localhost:4200`. The API must be running at the same time for the UI to function.

## Running Tests

### .NET Unit Tests

```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run only the test project
dotnet test StargateAPI.Tests/StargateAPI.Tests.csproj
```

Code coverage target is >50%. Coverage reports are written to `TestResults/` after running with the coverage flag.

### Angular Tests

```bash
cd stargate-ui

# Run once (CI mode)
npx ng test --watch=false

# Run in watch mode (development)
npx ng test
```

Angular 21 uses **Vitest** as the test runner. No browser is required.

### Full local CI run

```bash
act push -j build-and-test
```

This runs the `build-and-test` job from `ci.yml` locally using Docker. Requires [act](https://nektosact.com) to be installed.

---

## CI / CD

GitHub Actions runs on every push to `dev`, `feature/*`, and `bugfix/*` branches, and on pull requests targeting `main` or `dev`.

### Jobs

| Job | Description |
|-----|-------------|
| `build-and-test` | Restores, builds, and tests both .NET and Angular |
| `codeql` | Static security analysis for C# and TypeScript |

### Workflow file

`.github/workflows/ci.yml`

The `build-and-test` job covers:
- `dotnet restore` → `dotnet build` → `dotnet test` with XPlat Code Coverage
- `npm ci` → `npx ng test --watch=false` → `npm run build --configuration production`

CodeQL runs as a separate matrix job and requires GitHub infrastructure — it will not succeed when run locally with `act`. Use `-j build-and-test` for local validation.

---

## Business Rules

Seven rules govern all data operations. All are enforced in the service layer before any database write occurs.

| # | Rule |
|---|------|
| 1 | A Person is uniquely identified by their Name |
| 2 | A Person who has not had an astronaut assignment will not have Astronaut records |
| 3 | A Person will only ever hold one current Astronaut Duty Title, Start Date, and Rank at a time |
| 4 | A Person's Current Duty will not have a Duty End Date |
| 5 | A Person's Previous Duty End Date is set to the day before the New Astronaut Duty Start Date when a new duty is received |
| 6 | A Person is classified as Retired when their Duty Title is `RETIRED` |
| 7 | A Person's Career End Date is one day before the Retired Duty Start Date |

### Implicit Rules

- Adding a duty updates both `AstronautDuty` and `AstronautDetail` in a single transaction — all succeed or all roll back.
- A person's first duty creates a new `AstronautDetail` record. It is not created on person creation.
- `DutyEndDate IS NULL` identifies the current duty. There is always at most one row per person with a null end date.

---

## Data Model

Three EF Core entities with Fluent API configuration. The three-table relationship drives all business logic.

### Person

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, auto-increment |
| Name | string | Required, unique index, max 200 chars |
| AstronautDetail | nav property | 1:0..1 |
| AstronautDuties | nav collection | 1:many |

### AstronautDetail

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, auto-increment |
| PersonId | int | FK → Person.Id |
| CurrentRank | string | Required |
| CurrentDutyTitle | string | Required |
| CareerStartDate | DateTime | Required — set to first duty's start date |
| CareerEndDate | DateTime? | Nullable — set when duty title is RETIRED |

### AstronautDuty

| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK, auto-increment |
| PersonId | int | FK → Person.Id |
| Rank | string | Required |
| DutyTitle | string | Required |
| DutyStartDate | DateTime | Required |
| DutyEndDate | DateTime? | Nullable — NULL means this is the current duty |

---

## Architecture & Design Patterns

### Stack

| Layer | Technology |
|-------|-----------|
| API Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQLite |
| CQRS / Mediator | MediatR 12 with PreProcessor pipeline |
| API Docs | Swagger / Swashbuckle |
| Frontend | Angular 21 (standalone components, Vitest) |

### Patterns

| Pattern | Description |
|---------|-------------|
| **Mediator / CQRS** | Commands and Queries cleanly separated through MediatR. Commands mutate state, Queries read. The architectural backbone of the API. |
| **PreProcessor Pipeline** | Validation runs before handlers via `IRequestPreProcessor`. All POST commands validate input before any handler logic executes. |
| **Global Exception Middleware** | `ExceptionHandlingMiddleware` catches all unhandled exceptions and returns a consistent `BaseResponse` JSON envelope. No stack traces reach the client. |
| **Dependency Injection** | `StargateContext`, `MediatR`, and `ILogService` registered in `Program.cs` with constructor injection throughout. |
| **Thin Controllers** | Controllers only dispatch to MediatR — zero business logic in controller actions. |
| **Fluent Configuration** | EF Core entity configurations are separated from entity classes using `IEntityTypeConfiguration<T>`. |
| **Process Logging** | `ILogService` with a `ProcessLog` entity. Logs successes and exceptions with timestamps, source, and stack traces to the database. |
| **DTOs** | `PersonAstronaut` DTO used for read queries. EF entities do not leak to the API response surface. |

## Code Review Findings

13 flaws were identified and resolved across security, correctness, business logic, and code quality.

### Critical

| ID | Finding | Resolution |
|----|---------|------------|
| F1 | SQL injection in all Dapper queries — user input embedded directly into SQL strings | |
| F2 | `AstronautDutyController` dispatched `GetPersonByName` instead of `GetAstronautDutiesByName` — the duties endpoint never returned duties | |

### High

| ID | Finding | Resolution |
|----|---------|------------|
| F3 | `CreatePerson` PreProcessor rejected existing names; requirement specifies add/update ||
| F4 | POST /AstronautDuty had no exception handling — unhandled errors returned raw stack traces | |
| F5 | New `AstronautDetail` path set `CareerEndDate = DutyStartDate` instead of `AddDays(-1)` on RETIRED | |

### Medium

| ID | Finding | Resolution |
|----|---------|------------|
| F6 | No input validation on POST endpoints — empty names, null fields, invalid dates accepted | |
| F7 | `GetPeopleHandler._context` declared `public readonly` instead of `private readonly` |  |
| F8 | PreProcessors threw generic `BadHttpRequestException("Bad Request")` with no context | |
| F9 | Duplicate duty check was global — not scoped to the person | |
| F10 | No unique constraint on `Person.Name` at the database level | |

### Low

| ID | Finding | Resolution |
|----|---------|------------|
| F11 | No CORS configuration — Angular frontend blocked by same-origin policy | |
| F12 | Null reference in `GetAstronautDutiesByNameHandler` — no null check after person lookup | |
| F13 | Null reference in `CreateAstronautDutyHandler` — person lookup could return null |  |

### Static Analysis

Qodana and JetBrains Rider were run against the original codebase. All 17 findings were resolved:

- 4 compiler warnings — null reference / uninitialized members → null guards and `= null!` initializers
- 5 EF performance warnings — unlimited string lengths → `HasMaxLength()` added to all entity configurations
- 4 redundancies — empty initializers, unused directives → cleaned up
- 3 code quality — unused auto-property accessors → resolved
- 1 migration designer override method mismatch → migrations regenerated from corrected schema

---

## Process Logging

All API operations are logged to the database via `ILogService`.

### ProcessLog Table

| Column | Type | Description |
|--------|------|-------------|
| Id | int | PK, auto-increment |
| Message | string | Human-readable description of the event |
| LogLevel | string | `Information` for success, `Error` for exceptions |
| Timestamp | DateTime | UTC timestamp of the event |
| Source | string | Handler or middleware class name |
| StackTrace | string? | Nullable — populated on exceptions only |

