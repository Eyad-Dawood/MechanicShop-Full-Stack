# MechanicShop Technical Documentation

## 1) Project Purpose and Scope

### What this project is
- `MechanicShop` is a .NET 9 backend for shop operations.
- It manages:
- customers and vehicles
- repair task catalog with parts/labor estimates
- work order scheduling and lifecycle
- invoice issuance, settlement, and PDF export
- authentication/authorization for manager and labor users
- schedule and dashboard stats endpoints

### What this project is not
- No frontend project in this repository.
- `AppHost` only launches the API process.
- No message broker/event bus integration.
- No implemented backup/restore workflow in code.

### Scope boundaries in current code
- Domain rules are implemented in entities and handlers.
- SQL Server persistence via EF Core and ASP.NET Identity.
- Versioned API routes (v1).
- Notification transport is simulated (log-based email/SMS).

## 2) Architecture and Layer Responsibilities

### Projects
- `MechanicShop.Api`
- host, controllers, middleware, OpenAPI.
- `MechanicShop.Application`
- MediatR handlers, validators, pipelines, DTO mapping.
- `MechanicShop.Domain`
- entities, enums, errors, invariants.
- `MechanicShop.Infrastructure`
- EF Core, identity services, policies, background jobs, PDF, notifier.
- `MechanicShop.Contracts`
- API contracts (request/response models).
- `MechanicShop.ServiceDefaults`
- Aspire defaults (health, telemetry hooks, service discovery).
- `MechanicShop.AppHost`
- Aspire app that starts `mechanicshop-api`.

### Layering style
- API -> MediatR command/query -> domain + db context.
- Infrastructure provides implementations behind application interfaces.
- Domain contains mutation rules and state transitions.

### Architectural characteristics
- CQRS-style request handlers.
- `Result<T>`-driven error handling.
- EF Core Fluent API per entity.
- Domain events dispatched through DbContext save.
- Hybrid cache for selected queries.

## 3) Startup/Runtime Flow

### 3.1 Boot flow (`MechanicShop.Api/Program.cs`)
- Create builder.
- Register services:
- `AddPresentation(configuration)`
- `AddApplication()`
- `AddInfrastructure(configuration, AppSettings)`
- Configure Serilog from config.
- Add Aspire defaults (`AddServiceDefaults`).
- Build app.
- Development only:
- map OpenAPI + Swagger UI
- `InitialiseDatabaseAsync()`
- enable WebAssembly debugging middleware
- Non-development:
- use HSTS
- map controllers + default endpoints
- use forwarded headers
- apply core middleware pipeline (`UseCoreMiddlewares`)
- run application

### 3.2 DI and cross-cutting setup
- Presentation registration includes:
- ProblemDetails customization
- API versioning and API explorer
- OpenAPI transformers (version info + bearer scheme)
- global exception handler
- CORS from `AppSettings`
- rate limiter
- output cache
- hybrid cache defaults
- `IUser` (`CurrentUser`)
- SignalR service registration

- Infrastructure registration includes:
- SQL Server `AppDbContext`
- `AuditableEntityInterceptor`
- JWT bearer auth
- ASP.NET Identity Core (`AppUser`, roles)
- authorization policies and custom requirement handler
- `IIdentityService`, `ITokenProvider`, `IWorkOrderPolicy`, `IInvoicePdfGenerator`, `INotificationService`, `IWorkOrderNotifier`
- background job: `OverdueBookingCleanupService`

- Application registration includes:
- validators from application assembly
- MediatR handlers
- behaviors: validation, performance, unhandled exception, caching

### 3.3 Logging
- Serilog enabled and configured via `appsettings.json`.
- Sinks: Console and Seq (`http://ops.seq:5341`).
- Request logging middleware enabled.
- Long-running requests (>500ms) logged in `PerformanceBehaviour`.

### 3.4 Database initialization, migrations, seeding
- In Development, startup runs `InitialiseDatabaseAsync`.
- `ApplicationDbContextInitialiser` calls:
- `Database.EnsureCreatedAsync()`
- seed routine (`TrySeedAsync`)
- Important:
- this uses `EnsureCreated`, not migration-based startup
- no EF migrations folder is present

### 3.5 Seed data
- Roles:
- `Manager`, `Labor`
- Seeded users (password equals email):
- `pm@localhost` (Manager)
- `john.labor@localhost`
- `peter.labor@localhost`
- `kevin.labor@localhost`
- `suzan.labor@localhost`
- Seeded domain data:
- employees mapped to seeded user IDs
- customers + vehicles
- repair tasks + parts
- generated scheduled work orders for next month
- two in-progress work orders around current UTC time

### 3.6 Background cleanup lifecycle
- Hosted service runs every `OverdueBookingCleanupFrequencyMinutes`.
- Selects scheduled work orders older than configured threshold.
- Calls domain `Cancel()` and saves changes.
- Logs both successes and failures.

### 3.7 Middleware order (`UseCoreMiddlewares`)
- exception handler
- status code pages
- HTTPS redirection
- Serilog request logging
- CORS
- rate limiter
- authentication
- authorization
- output cache

## 4) Core Domain Model and Enums

### Entities
- `WorkOrder`
- schedule window, labor, spot, state, repair tasks, optional invoice.
- `Customer`
- customer profile with vehicle collection.
- `Vehicle`
- make/model/year/license plate.
- `RepairTask`
- service task with labor cost, estimated duration, parts.
- `Part`
- name, unit cost, quantity.
- `Invoice`
- line items, tax/discount, payment status.
- `RefreshToken`
- token string, user ID, expiry.
- `Employee`
- employee identity and role.
- `AppUser`
- ASP.NET Identity user.

### Enums
- `Role`: `Labor`, `Manager`
- `WorkOrderState`: `Scheduled`, `InProgress`, `Completed`, `Cancelled`
- `Spot`: `A`, `B`, `C`, `D`
- `RepairDurationInMinutes`: 15 to 180 (15-minute increments)
- `InvoiceStatus`: `Unpaid`, `Paid`, `Refunded`

### Key business invariants in domain
- WorkOrder:
- valid IDs required
- at least one repair task required
- `EndAtUtc > StartAtUtc`
- spot must be valid enum
- transition rules:
- Scheduled -> InProgress
- InProgress -> Completed
- any non-completed -> Cancelled
- non-editable states block timing/labor/task mutations

- Customer:
- name required
- phone regex `^\+?\d{7,15}$`
- email required and format-checked

- Vehicle:
- year between min vehicle year and current UTC year
- make/model/license required

- RepairTask:
- name required
- positive labor cost
- valid duration enum

- Invoice:
- requires non-empty line items
- discount cannot be negative or greater than subtotal
- payment allowed only when status is `Unpaid`

## 5) DbContext Configuration, Indexes, Defaults, Delete Behaviors

### 5.1 AppDbContext behavior
- Inherits `IdentityDbContext<AppUser>`.
- Exposes business DbSets through `IAppDbContext`.
- `OnModelCreating` applies configuration classes from assembly.
- `SaveChangesAsync` publishes domain events before base save.

### 5.2 Auditing defaults
- `AuditableEntityInterceptor` writes:
- `CreatedAtUtc`, `CreatedBy` on add
- `LastModifiedUtc`, `LastModifiedBy` on add/modify
- also updates owned auditable entities when changed

### 5.3 Configuration summary by entity
- `Customer`
- table: `Customers`
- non-clustered PK
- clustered index on `Name`
- non-clustered index on `PhoneNumber`
- `Name` required (150)
- `PhoneNumber` required (20)
- `Email` max (150)

- `Vehicle`
- non-clustered PK
- `Id` value generated never
- `Make` required (100)
- `Model` required (100)
- `Year` required
- `LicensePlate` required

- `Employee`
- table: `Employees`
- non-clustered PK
- clustered index on `FirstName`
- index on `LastName`
- `Role` stored as string

- `RepairTask`
- non-clustered PK
- `Id` value generated never
- `Name` required (100)
- `EstimatedDurationInMins` stored as string
- `LaborCost` decimal(18,2)
- parts relation delete behavior: `Cascade`

- `Part`
- non-clustered PK
- `Id` value generated never
- `Name` required (100)
- `Cost` decimal(18,2)
- `Quantity` required

- `WorkOrder`
- non-clustered PK
- required `LaborId`
- FK to `VehicleId`
- one-to-one invoice FK on invoice side with `DeleteBehavior.Restrict`
- many-to-many with repair tasks via `WorkOrderRepairTasks`
- state and spot stored as strings
- indexes:
- `LaborId`
- `VehicleId`
- `State`
- composite (`StartAtUtc`, `EndAtUtc`)
- `Tax` and `Discount` precision(18,2)
- `Total`, `TotalLaborCost`, `TotalPartsCost` ignored by EF mapping

- `Invoice`
- table: `Invoices`
- non-clustered PK
- `Id` value generated never
- `DiscountAmount`, `TaxAmount` precision(18,2)
- status stored as string
- owns many `LineItems` in `InvoiceLineItems`
- owned PK: (`InvoiceId`, `LineNumber`)

- `RefreshToken`
- table: `RefreshTokens`
- non-clustered PK
- unique index on `Token`
- `Token` max length 200
- `UserId` required
- `ExpiresOnUtc` required

### 5.4 Defaults and constraints notes
- Most defaults are application-level, not SQL default constraints.
- No optimistic concurrency token configuration is present.
- No global query filters are configured.

## 6) Repository + UnitOfWork Patterns and Query Conventions

### 6.1 Pattern used in this codebase
- No explicit repository classes are implemented.
- No `IUnitOfWork` type exists.
- `IAppDbContext` + `SaveChangesAsync` acts as unit-of-work boundary.
- Handlers query DbSets directly with EF LINQ.

### 6.2 Query conventions
- Read handlers frequently use `AsNoTracking`.
- Complex reads use explicit `Include`/`ThenInclude`.
- Pagination/sorting/filtering is handler-side.
- `PaginatedList<T>` standardizes paging metadata.
- Caching is opt-in via `ICachedQuery`.
- Mutation handlers invalidate cache tags.

### 6.3 Hybrid cache behavior
- `CachingBehavior` checks for `ICachedQuery`.
- Reads from cache key first.
- On cache miss: executes handler and caches successful `Result` responses.
- Uses tags for grouped invalidation.

## 7) Service-by-Service Responsibilities and Key Rules

### 7.1 API controllers
- `WorkOrdersController`
- list/detail, create, relocate, assign labor, update state, update tasks, delete, daily schedule.
- `CustomersController`
- customer CRUD with vehicle child operations.
- `RepairTasksController`
- repair-task CRUD with part child operations.
- `InvoicesController`
- issue, query, PDF download, settle payment.
- `IdentityController`
- token issue, refresh, current user claims.
- `DashboardController`
- daily work order stats.
- `LaborsController`
- labor listing.
- `SettingsController`
- operating-hours exposure.

### 7.2 Application handlers (business behavior)
- Work order creation:
- validates all repair task IDs exist
- calculates end time from sum of repair durations
- enforces operating hours and min duration
- checks spot overlap, labor overlap, vehicle overlap

- Work order relocation:
- preserves duration
- checks spot/labor/vehicle overlap in target slot
- updates timing and spot

- Work order state update:
- disallows transitions before scheduled start time
- uses domain transition map
- emits completion event when completed

- Work order repair-task update:
- requires at least one task
- rebuilds task list
- recalculates end time and revalidates constraints

- Work order delete:
- allowed only when state is `Scheduled`

- Customer create/update/remove:
- email uniqueness enforcement
- vehicle upsert behavior on update
- deletion blocked if customer has any related work orders

- Repair task create/update/remove:
- duplicate name check on create
- part upsert behavior on update
- deletion blocked when referenced by work orders

- Invoice issue/settle:
- issue allowed only for completed work order
- line items generated per repair task (labor + parts)
- tax from `ShopConstants.TaxRate`
- settlement marks invoice paid with paid timestamp

### 7.3 Infrastructure services
- `WorkOrderPolicy`
- reusable conflict and operating-hour checks.
- `IdentityService`
- authentication and role/policy checks.
- `TokenProvider`
- JWT creation and refresh-token persistence/rotation.
- `InvoicePdfGenerator`
- QuestPDF invoice document generation.
- `NotificationService`
- simulated (logged) email and SMS notifications.
- `SignalRWorkOrderNotifier`
- broadcasts `WorkOrdersChanged` to hub clients.
- `OverdueBookingCleanupService`
- periodic cancellation of overdue scheduled orders.

## 8) Authentication and Session Model

### Auth configuration
- JWT bearer is default auth scheme.
- token validation uses `JwtSettings` (`Secret`, `Issuer`, `Audience`).
- `ClockSkew = 0`.

### Authorization model
- `ManagerOnly` policy -> Manager role required.
- `SelfScopedWorkOrderAccess` policy -> labor assigned to route work order OR manager.
- Work-order state endpoint uses role + policy combination.

### Token/session lifecycle
- Login endpoint authenticates via `UserManager`.
- On login:
- creates JWT access token with subject/email/roles
- deletes existing refresh tokens for user
- inserts one new refresh token expiring in 7 days

- Refresh endpoint:
- validates expired access token signature and claims
- resolves user ID
- validates refresh token existence and expiry
- issues new access+refresh pair (rotation by replacing stored token)

### Session characteristics
- Effectively one active refresh token per user.
- Stateless access token + stateful refresh token table.

## 9) Backup/Restore Design and Config Files

### Current state in repository
- No backup job/service is implemented.
- No restore workflow or restore endpoint exists.
- No backup-specific config section exists in appsettings files.

### Current config files relevant to persistence
- `MechanicShop.Api/appsettings.json`
- `ConnectionStrings:DefaultConnection`
- `AppSettings:DistributedCachSqlConnectionString`
- `MechanicShop.AppHost/appsettings.json`
- logging only, no backup configuration

### Operational implication
- Backup/restore is external to this codebase today.
- Data durability depends on SQL Server/environment operations.
- This is a production-readiness gap and should be planned explicitly.

## 10) Error Handling and Validation Strategy

### Result/error pattern
- Domain/application operations return `Result` objects.
- Errors include type (`Validation`, `Conflict`, `NotFound`, etc.) and codes.

### Validation stack
- FluentValidation via MediatR `ValidationBehavior`.
- Domain factories/methods enforce invariant validation.

### HTTP error mapping
- `ProblemExtensions` maps errors to HTTP:
- validation -> 400
- conflict -> 409
- not found -> 404
- unauthorized -> 403
- fallback -> 500

- Global exception handler returns ProblemDetails for unhandled exceptions.
- ProblemDetails includes request instance and trace identifier extension.

### Logging in failure paths
- Handlers log warning/error context for business rule failures.
- Unhandled exceptions logged by MediatR behavior.
- Background jobs catch/log runtime exceptions.

## 11) How to Run Locally

### Prerequisites
- .NET SDK 9.0
- reachable SQL Server instance
- local HTTPS dev certificate setup

### Setup
1. Configure connection string in `MechanicShop.Api/appsettings.json`.
2. Configure JWT settings (`Secret`, `Issuer`, `Audience`).
3. Ensure allowed CORS origins match your client host(s).

### Run API directly
1. `cd MechanicShop/MechanicShop.Api`
2. `dotnet restore`
3. `dotnet run`
4. In Development, DB is ensured and seeded automatically.

### Run with Aspire AppHost
1. `cd MechanicShop/MechanicShop.AppHost`
2. `dotnet restore`
3. `dotnet run`

### Development seed credentials
- Manager:
- `pm@localhost` / `pm@localhost`
- Labor examples:
- `john.labor@localhost` / `john.labor@localhost`
- `peter.labor@localhost` / `peter.labor@localhost`

### Route/version notes
- Versioned APIs: `/api/v{version}/...`
- Version-neutral endpoints:
- `/identity/*`
- `/api/settings/operating-hours`

## 12) Practical Risks and Prioritized Improvements

### P0 (fix first)
- SignalR hub route is never mapped.
- `WorkOrderHub` exists, but no `MapHub<WorkOrderHub>` call.

- Route value casing mismatch in authorization requirement.
- Policy handler reads `"WorkOrderId"`; controller route token is `workOrderId`.

- `GetDailyScheduleQuery.CacheKey` uses `LaborId!.Value` while labor filter is optional.
- Null laborId can trigger runtime failure.

### P1 (next)
- Replace `EnsureCreated` with migrations-based lifecycle.
- Add backup/restore design and automation.
- Remove duplicated DI registrations for `IAppDbContext` and `ApplicationDbContextInitialiser`.
- Fix event ordering where domain events are added after `SaveChangesAsync`.

### P2 (quality)
- Add tests for:
- scheduling overlap rules
- state transitions
- refresh-token edge cases
- policy authorization behavior
- cache invalidation behavior

- Clean consistency issues:
- typos in names (`Qurey`, `Cach`)
- duplicated authorize attribute on assign-labor endpoint
- register or remove unused `RequestLogContextMiddleware`

### Security note
- Seed users use email as password.
- This should stay development-only and never be used in production.

## 13) Maintenance Notes
- This documentation is intentionally onboarding/maintenance focused.
- It is not a file-by-file reference.
- Keep this README aligned with code behavior when runtime logic changes.
