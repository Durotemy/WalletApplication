# Wallet API — learning project

## Why this project exists
I'm relatively new to .NET and building this to grow into an **intermediate .NET role**.
It follows on from `~/Desktop/travelApp` (controllers → services → EF Core/Postgres, JWT, BCrypt),
which never had to deal with concurrency, idempotency, background work, proper error handling or tests.
A wallet/ledger API forces all of those, because money has to be correct.

## How to help me
- **Teach, don't just do.** Explain the fix and why; let me write the code unless I ask you to do it.
- Relate new concepts back to travelApp where it helps.
- Point out what an interviewer would ask about the topic.

## Constraints
- **No Docker** (disk space). Use my local Postgres, in-memory caching, a local `wallet_test` DB for tests.
- **Monolith**, single project. Possibly a modular monolith (`Features/`) later, once the layout feels messy.

## Stack & layout
- .NET 10, ASP.NET Core controllers, EF Core + Npgsql (Postgres)
- Namespace root: `WalletApi` (project was renamed from `learning4`)
- Folders: `Controllers/`, `Services/`, `Entities/`, `Dtos/`, `Enums/`, `Data/`
- Planned folders: `Data/Configurations/` (IEntityTypeConfiguration), `Common/` (Result<T>),
  `Validators/`, `Middleware/`, `BackgroundJobs/`
- Rules: controllers stay thin (no business logic); never return entities from the API, map to response DTOs;
  money is `decimal`; timestamps are UTC.

## The plan
| Phase | What you build | What you learn | Interview question it prepares you for |
|---|---|---|---|
| 1. Ledger | Balance worked out from debit/credit rows | Domain modelling, `decimal` for money, EF Core configuration, migrations, relationships | "Why not `double` for money?" "How do you configure EF without attributes?" |
| 2. Transfers | Move money between wallets | Database transactions, concurrency (two transfers at the same moment, Postgres `xmin`), `DbUpdateConcurrencyException` | "What's a race condition and how would you prevent one?" (the overbooking bug in travelApp) |
| 3. Idempotency | `Idempotency-Key` header | Middleware, the request pipeline, why a retried request mustn't charge someone twice | "What happens if the client retries after a timeout?" |
| 4. Errors and validation | FluentValidation, a `Result<T>` type, `ProblemDetails` | Clean error handling, global exception handling (`IExceptionHandler`), separating validation from business rules | "How do you handle errors across an API?" |
| 5. Payment provider | A fake external API plus a webhook | `IHttpClientFactory`, Polly retries and timeouts, webhook security | "How do you call an unreliable external service?" |
| 6. Outbox | Background notifications | `BackgroundService`, eventual consistency, reliable messaging | "How do you make sure an event is sent when the database save succeeds?" |
| 7. Real-time | Live balance updates | SignalR | Common in fintech roles |
| 8. Performance | Statements and transaction history | Pagination, `AsNoTracking`, projections, indexes, caching (HybridCache) | "Your endpoint is slow. What do you check?" |
| 9. Testing | Unit and integration tests | xUnit, `WebApplicationFactory`, testing against a real database (Respawn + `wallet_test`), testing concurrency | "How do you test your API?" (almost always asked) |
| 10. Production | Logging, health checks, CI, deployment | Serilog, rate limiting, GitHub Actions, deploying | "How do you know your app is healthy in production?" |

## Current status
**Phase 1 in progress.** Working: `POST api/v1/wallets` (WalletService → AppDbContext).
Next up:
- Add `ConnectionStrings:TravelDb` (or rename) to appsettings and create the first migration
- New wallets start at 0 — don't accept `balance` from the client
- Set `userId` when creating a wallet; `UserRequest` should take `Password`, not `PasswordHash`
- Design `LedgerEntry` before writing any transfer code
- Add `Data/Configurations/` (unique email index, max lengths, decimal precision)

_Update this section as phases are completed._
