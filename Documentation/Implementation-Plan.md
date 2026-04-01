# Stargate ACTS — Implementation ToDo

---

## Phase 1 — Critical Fixes
- [x] **F1 — Eliminate SQL injection** — Replace all Dapper raw SQL with EF Core LINQ queries in GetPeople, GetPersonByName, GetAstronautDutiesByName, and CreateAstronautDuty handlers
- [x] **F1 — Remove StargateContext.Connection** — Delete the `IDbConnection Connection` property; remove `using System.Data` and Dapper dependency from the project
- [ ] **F2 — Fix wrong MediatR dispatch** — Change `AstronautDutyController.GetAstronautDutiesByName` to send `GetAstronautDutiesByName` instead of `GetPersonByName`
- [ ] **F12 — Null guard in GetAstronautDutiesByNameHandler** — Return 404 with message if person not found
- [ ] **F13 — Null guard in CreateAstronautDutyHandler** — Return 400 with message if person lookup fails
- [ ] Verify: `GET /AstronautDuty/{name}` returns both person and duties
- [ ] Verify: passing `' OR 1=1 --` as a name returns 404, not a data dump

---

## Phase 2 — Business Rule
- [x] **F5 — Fix CareerEndDate on retirement** — Change new-AstronautDetail path to `AddDays(-1)` matching the update path
- [ ] **F9 — Scope duplicate duty check** — Add `z.PersonId == person.Id` filter in CreateAstronautDutyPreProcessor
- [ ] **F10 — Unique constraint on Person.Name** — Add `builder.HasIndex(x => x.Name).IsUnique()` in PersonConfiguration
- [ ] **Register CreatePersonPreProcessor** — Add `cfg.AddRequestPreProcessor<CreatePersonPreProcessor>()` in Program.cs
- [ ] **Enable seed data** — Uncomment SeedData call in StargateContext; use fixed UTC dates instead of DateTime.Now
- [ ] Generate new EF migration: `dotnet ef migrations add EnforceBusinessRules --output-dir Business/Migrations`
- [ ] Run migration: `dotnet ef database update`
- [ ] Verify: creating a duplicate person returns 400
- [ ] Verify: creating a duty for Person A with same title/date as Person B succeeds
- [ ] Verify: retiring a person on their first duty sets CareerEndDate = StartDate − 1

---

## Phase 3 — Defensive Coding & Error Handling

- [ ] **F4 — Global exception middleware** — Create `Middleware/ExceptionHandlingMiddleware.cs`; register in Program.cs before MapControllers
- [ ] **Remove per-action try-catch** — Strip try-catch from all controller actions (middleware handles it now)
- [ ] **F6 — Input validation on CreatePerson** — Reject empty/whitespace names in PreProcessor; trim input
- [ ] **F6 — Input validation on CreateAstronautDuty** — Reject empty Name, Rank, DutyTitle; reject default DutyStartDate
- [ ] **F8 — Descriptive error messages** — Replace all `"Bad Request"` strings with specific failure descriptions
- [x] **F11 — Add CORS** — Configure `AddCors` / `UseCors` in Program.cs with `http://localhost:4200`
- [ ] **F7 — Fix access modifier** — Change `public readonly` to `private readonly` in GetPeopleHandler
- [ ] **Clean up Program.cs** — Remove redundant using directives flagged by Qodana
- [ ] Verify: POST with empty body returns 400 with clear message, not 500
- [ ] Verify: unhandled exception returns JSON envelope, not stack trace

---

## Phase 4 — Process Logging

- [ ] **Create ProcessLog entity** — Id, Message, LogLevel, Timestamp, StackTrace (nullable), Source
- [ ] **Add ProcessLog DbSet** to StargateContext
- [ ] **Create ILogService interface** — `LogSuccess(message, source)`, `LogException(exception, source)`
- [ ] **Create LogService implementation** — Writes to ProcessLog table via StargateContext
- [ ] **Register ILogService** as scoped in Program.cs
- [ ] **Inject into handlers** — Log success at end of each handler; log exceptions in middleware
- [ ] Generate migration: `dotnet ef migrations add AddProcessLogging --output-dir Business/Migrations`
- [ ] Run migration: `dotnet ef database update`
- [ ] Verify: successful POST creates a log row with LogLevel = "Information"
- [ ] Verify: failed request creates a log row with LogLevel = "Error" and populated StackTrace

---

## Phase 5 — Unit Tests
- [ ] **Set up test project** — `dotnet new xunit -n StargateAPI.Tests`; add project reference; install InMemory provider
- [ ] **CreateAstronautDutyHandler tests** (highest priority — most complex handler):
    - [ ] First duty for a person creates AstronautDetail
    - [ ] New duty closes previous duty's end date (StartDate − 1)
    - [ ] RETIRED duty sets CareerEndDate = StartDate − 1
    - [ ] Handler updates AstronautDetail rank and title
    - [ ] Person not found returns failure response
- [ ] **CreateAstronautDutyPreProcessor tests:**
    - [ ] Rejects missing person
    - [ ] Rejects duplicate duty (same person + title + date)
    - [ ] Allows same duty title + date for different people
    - [ ] Rejects empty name, rank, title, default date
- [ ] **CreatePersonPreProcessor tests:**
    - [ ] Rejects duplicate name
    - [ ] Rejects empty/whitespace name
    - [ ] Trims whitespace from name
- [ ] **CreatePersonHandler tests:**
    - [ ] Creates person and returns ID
- [ ] **GetAstronautDutiesByNameHandler tests:**
    - [ ] Returns person + duties sorted by date descending
    - [ ] Returns 404 for unknown name
    - [ ] Returns empty duty list for non-astronaut
- [ ] **GetPersonByNameHandler tests:**
    - [ ] Returns person with astronaut detail
    - [ ] Returns 404 for unknown name
- [ ] **GetPeopleHandler tests:**
    - [ ] Returns all people with detail
    - [ ] Returns empty list when no people exist
- [ ] Run coverage: `dotnet test --collect:"XPlat Code Coverage"` — confirm >50%

---

## Phase 6 — Frontend
- [ ] **Scaffold project** — `ng new stargate-ui --style=scss --routing=true --ssr=false`
- [ ] **Create models** — person-astronaut.model.ts, astronaut-duty.model.ts, api-responses.model.ts
- [ ] **Create StargateService** — All 5 API calls with HttpClient
- [ ] **App shell** — Header with nav links (Roster, Add Person, Add Duty); responsive layout
- [ ] **Shared components:**
    - [ ] Status badge (Active / Retired / No Assignment)
    - [ ] Toast notification service (success / error)
    - [ ] Loading spinner / skeleton rows
- [ ] **Personnel Roster page:**
    - [ ] Table with Name, Rank, Current Duty, Career Start, Status columns
    - [ ] Row click navigates to duty history
    - [ ] Search/filter bar
    - [ ] Loading, empty, and error states
- [ ] **Person Detail / Duty History page:**
    - [ ] Person summary card
    - [ ] Duty history timeline/table sorted newest-first
    - [ ] Active vs Retired vs Non-Astronaut display variants
    - [ ] Loading and not-found states
- [ ] **Add Person page:**
    - [ ] Name input with validation
    - [ ] Duplicate error handling
    - [ ] Success confirmation with navigation options
    - [ ] Submitting state (locked input, spinner)
- [ ] **Add Astronaut Duty page:**
    - [ ] Person name (typeahead from roster), Rank, Duty Title, Start Date fields
    - [ ] Field-level validation errors
    - [ ] API error handling (person not found, duplicate duty)
    - [ ] Success confirmation with link to duty history
    - [ ] Submitting state
- [ ] **Routing** — `/` → Roster, `/duties/:name` → Duty History, `/add-person`, `/add-duty`
- [ ] Verify: full round trip — create person → assign duty → view duty history

---

## Phase 7 — If time allows

- [ ] **Qodana clean** — Run static analysis; resolve any remaining warnings
- [ ] **HasMaxLength on all string properties** — Person.Name (200), Rank, DutyTitle, CurrentRank, CurrentDutyTitle
- [ ] **Navigation property initializers** — `= null!` on all required nav properties to silence compiler
- [ ] **DTO mapping** — Ensure no EF entities leak to API response surface; create AstronautDutyDto if needed
- [ ] **Review Swagger** — Confirm all 5 endpoints show correct request/response schemas
- [ ] **README update** — Document setup steps, prerequisites, how to run API + frontend
- [ ] **Changelog** — Document every fix with what changed and why
- [ ] **Final regression** — Run all unit tests, hit every endpoint manually, test every UI state
- [ ] **Build clean** — `dotnet build --no-incremental` with zero warnings; `ng build --configuration production` with zero errors