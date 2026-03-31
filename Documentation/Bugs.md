# Bug Tickets — Stargate ACTS

---

## Issue 1 — SQL Injection Vulnerability in Query Handlers

**Severity:** Critical
**Labels:** `bug`, `security`, `backend`

### Description

Raw SQL queries using string interpolation allow SQL injection across multiple handlers.

### Affected Areas

* GetPeople
* GetPersonByName
* GetAstronautDutiesByName
* CreateAstronautDuty

### Steps to Reproduce

1. Call `GET /Person/' OR 1=1 --`
2. Observe unexpected data returned

### Expected Behavior

Input is treated as a literal string and returns 404 or empty result.

### Acceptance Criteria

* Replace all raw SQL with EF Core LINQ
* Remove Dapper dependency
* `' OR 1=1 --` returns no data

---

## Issue 2 — Wrong MediatR Request in AstronautDutyController

**Severity:** High
**Labels:** `bug`, `backend`, `api`

### Description

`GET /AstronautDuty/{name}` dispatches `GetPersonByName` instead of `GetAstronautDutiesByName`.

### Steps to Reproduce

1. Create duties for a person
2. Call endpoint
3. Duties are missing

### Expected Behavior

Returns person + full duty history

### Acceptance Criteria

* Correct MediatR request
* Duties returned with person data

---

## Issue 3 — Missing Null Guard in GetAstronautDutiesByNameHandler

**Severity:** Medium
**Labels:** `bug`, `backend`

### Description

No null check when person is not found → potential crash

### Steps to Reproduce

1. Call endpoint with unknown name

### Expected Behavior

Returns 404 with message

### Acceptance Criteria

* Add null check
* Return 404 with descriptive message

---

## Issue 4 — Missing Null Guard in CreateAstronautDutyHandler

**Severity:** Medium
**Labels:** `bug`, `backend`

### Description

Handler may dereference null person after lookup

### Steps to Reproduce

1. POST duty for non-existent person

### Expected Behavior

Returns 400 with message

### Acceptance Criteria

* Add null guard
* Return safe error response

---

## Issue 5 — Incorrect CareerEndDate on Retirement

**Severity:** High
**Labels:** `bug`, `business-logic`

### Description

CareerEndDate incorrectly set to same day instead of `StartDate - 1`

### Steps to Reproduce

1. Add first duty with `RETIRED`
2. Inspect CareerEndDate

### Expected Behavior

`CareerEndDate = StartDate - 1`

### Acceptance Criteria

* Use `AddDays(-1)` in all paths

---

## Issue 6 — Duplicate Duty Check Not Scoped to Person

**Severity:** Medium
**Labels:** `bug`, `business-logic`

### Description

Duplicate duty validation is global instead of per person

### Steps to Reproduce

1. Add same duty for two different people
2. Second request fails

### Expected Behavior

Allowed for different people

### Acceptance Criteria

* Filter by `PersonId`
* Only block duplicates per person

---

## Issue 7 — Missing Unique Constraint on Person.Name

**Severity:** Medium
**Labels:** `bug`, `database`

### Description

Database does not enforce uniqueness of person names

### Steps to Reproduce

1. Insert duplicate names

### Expected Behavior

Database rejects duplicates

### Acceptance Criteria

* Add unique index on `Person.Name`
* Migration applied

---

## Issue 8 — CreatePersonPreProcessor Not Registered

**Severity:** High
**Labels:** `bug`, `backend`, `validation`

### Description

Validation logic not executed because preprocessor is not registered

### Steps to Reproduce

1. Create duplicate person
2. Validation not triggered

### Expected Behavior

Validation runs before handler

### Acceptance Criteria

* Register preprocessor in Program.cs
* Validation enforced

---

## Issue 9 — Missing Input Validation on POST Endpoints

**Severity:** High
**Labels:** `bug`, `validation`

### Description

Empty/invalid inputs are accepted

### Steps to Reproduce

1. Submit empty name or duty fields

### Expected Behavior

400 with validation errors

### Acceptance Criteria

* Validate name, rank, title, date
* Reject empty/invalid inputs

---

## Issue 10 — Generic "Bad Request" Messages

**Severity:** Medium
**Labels:** `bug`, `ux`, `api`

### Description

Error messages lack detail

### Steps to Reproduce

1. Trigger validation error

### Expected Behavior

Specific error message

### Acceptance Criteria

* Replace generic messages with descriptive ones

---

## Issue 11 — Missing Global Exception Handling Middleware

**Severity:** High
**Labels:** `bug`, `backend`, `api`

### Description

Unhandled exceptions may expose stack traces

### Steps to Reproduce

1. Trigger server exception

### Expected Behavior

Consistent JSON error response

### Acceptance Criteria

* Add middleware
* Remove controller try/catch
* Standard error envelope returned

---

## Issue 12 — Missing CORS Configuration

**Severity:** Medium
**Labels:** `bug`, `frontend`, `infrastructure`

### Description

Frontend cannot call API due to CORS restrictions

### Steps to Reproduce

1. Run Angular app
2. Make API call

### Expected Behavior

Request succeeds

### Acceptance Criteria

* Enable CORS for `http://localhost:4200`

---

## Issue 13 — Incorrect Access Modifier in GetPeopleHandler

**Severity:** Low
**Labels:** `bug`, `code-quality`

### Description

`_context` is `public readonly` instead of `private readonly`

### Acceptance Criteria

* Change to `private readonly`

---

## Issue 14 — API Behavior Mismatch with README (Add vs Update Person)

**Severity:** Medium
**Labels:** `bug`, `requirements`

### Description

README says “add/update person,” but implementation rejects duplicates

### Steps to Reproduce

1. Create same person twice

### Expected Behavior

Either:

* Update existing person, OR
* Clearly reject duplicates (and update README)

### Acceptance Criteria

* Align API behavior with documented requirements

---

## Issue 15 — EF Core Migration Designer Version Mismatch

**Severity:** High
**Labels:** `bug`, `database`, `build`

### Description

Migration was generated with EF Core 7 (`ProductVersion: "7.0.15"`) but the project references EF Core 10 packages (`10.0.4`). The `BuildTargetModel` override in the designer file does not match the EF Core 8 base class signature, producing a compiler error that blocks the build entirely.

### Steps to Reproduce

1. Open project in IDE or run `dotnet build`
2. Compiler error in `Business/Migrations/20240122154939_InitialCreate.Designer.cs`

### Expected Behavior

Project compiles without errors.

### Acceptance Criteria

* Delete `Business/Migrations/` folder
* Regenerate migration: `dotnet ef migrations add InitialCreate --output-dir Business/Migrations`
* Apply migration: `dotnet ef database update`
* Project builds with zero errors