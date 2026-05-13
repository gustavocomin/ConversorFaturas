# ConversorFaturas

> A .NET 9 console tool that reads CSV credit-card statements and account
> statements exported by Brazilian banks, normalizes them into a shared
> domain model, and emits a consolidated Excel workbook plus a CSV ready to
> import into other personal-finance tools.

---

## Engineering Focus

- Financial data normalization
- Parser architecture
- Event-driven extensibility
- Clean Architecture
- Multi-bank ingestion pipelines
- AI-ready financial infrastructure

## What it does

Brazilian banks export statements in incompatible CSV layouts (different
columns, separators, decimal conventions, header rows, encodings). This tool
removes the manual reconciliation step:

1. Point it at a folder containing the raw `.csv` files exported from your
   bank's app or web portal.
2. Each file is routed to a bank-specific parser based on its filename
   (`inter*.csv` → Inter parser, `nubank*.csv` → Nubank parser, etc.).
3. Every parsed row is mapped into the unified [`Fatura`](Domain/Faturas/Fatura.cs)
   or [`Extrato`](Domain/Extratos/Extrato.cs) model.
4. The aggregated set is written to a `Convertido/` subfolder as:
   - `Fatura Convertida.xlsx` — Excel sheet with item, value, date, bank.
   - `Fatura Convertida Mobilis.csv` — CSV in the format expected by the
     **Mobilis** finance app (`Data;Descrição;Valor;Banco;Categoria`).

The same flow exists for account statements (`Extratos`), which expects the
raw files inside an `Extratos/` subfolder.

---

## Pipeline

```
 raw .csv files in <folder>
            │
            ▼
 ┌────────────────────────────┐
 │  AplicConversorFaturas     │   filename → dispatch
 │  AplicConversorExtratos    │   (Strategy-by-convention)
 └─────────────┬──────────────┘
               │
   ┌───────────┼───────────┐
   ▼           ▼           ▼
 Inter      Nubank      Itaú*
 parser     parser     (stub — routes
                        to Inter parser)
   │           │           │
   └───────────┼───────────┘
               ▼
   ┌────────────────────────────┐
   │  Fatura / Extrato model    │
   │  (Banco, Data, Descricao,  │
   │   Valor, Categoria, Tipo)  │
   └─────────────┬──────────────┘
                 │
        ┌────────┴────────┐
        ▼                 ▼
   ┌─────────┐       ┌─────────┐
   │  EPPlus │       │   CSV   │
   │  .xlsx  │       │ Mobilis │
   └─────────┘       └─────────┘
```

`*` Itaú is recognized by filename but currently dispatches to the Inter
parser. A dedicated Itaú adapter has not been written yet.

---

## Tech stack

| Layer                | Choice                                            |
|----------------------|---------------------------------------------------|
| Runtime              | .NET 9 (console app, `OutputType=Exe`)            |
| Language             | C# 12 with nullable + implicit usings enabled     |
| Excel output         | EPPlus 7.7.0 (`LicenseContext.NonCommercial`)     |
| PDF parsing          | PdfPig 0.1.10 *(referenced, not wired in yet)*    |
| Persistence (planned)| EF Core 9 + Npgsql (PostgreSQL provider)          |
| DI                   | `Microsoft.Extensions.DependencyInjection` 9      |
| Logging              | `Microsoft.Extensions.Logging.Console` / `.Debug` |

There is no web framework — input/output goes through `Console.ReadLine` and
the filesystem.

---

## Folder structure

```
ConversorFaturas/
├── Program.cs                 ── entry point: prompts for a path, runs the
│                                 conversor, prints the output location
│
├── Aplicacao/                 ── application services (use cases)
│   ├── Conversor/
│   │   ├── Faturas/           ── credit-card statement pipeline
│   │   │   ├── AplicConversorFaturas.cs    (orchestrator + Excel/CSV writer)
│   │   │   ├── Inter/         ── Inter-specific row parser
│   │   │   └── Nubank/        ── Nubank-specific row parser
│   │   └── Extratos/          ── account statement pipeline (same shape)
│   │       ├── Inter/
│   │       └── Nubank/
│   └── Dto/                   ── view-model objects used while building
│                                 the Excel report (cells, totalizers)
│
├── Domain/
│   ├── Faturas/Fatura.cs      ── unified credit-card transaction model
│   ├── Extratos/Extrato.cs    ── unified account-movement model
│   ├── Common/                ── IRepBase<T>, Identificador base classes
│   │                            (defined but not yet implemented)
│   ├── Config/Db/IDbService.cs── DB connection contract (planned)
│   ├── Enums/TipoPlanilha.cs  ── sheet-type discriminator
│   └── Exceptions/            ── domain-specific exception types
│
├── Common/Functions.cs        ── shared helpers: date parsing, decimal
│                                 parsing, Excel formatting, totalizers,
│                                 category aggregation
│
├── Ioc/
│   ├── Services.cs            ── root DI registration
│   └── Application/ApplicationService.cs
│                              ── registers the two application services
│
├── Financeiro.csproj
├── Financeiro.sln
└── README.md
```

The `Repository/` folder mentioned in the project description does not exist
yet — see [Roadmap](#roadmap).

---

## Patterns in use

- **Strategy-by-convention** — `AplicConversorFaturas.ConverterCsv` inspects
  the filename and dispatches to the matching bank parser. Adding a new bank
  is a new folder + a new `case` in the dispatcher.
- **DI-driven composition** — `Program` is registered as a singleton and
  resolved from a scoped `IServiceProvider`; the application services are
  scoped (`AddScoped<IAplicConversorFaturas, …>`).
- **Layered/Clean-Architecture-ish** — `Domain` has no dependencies on
  `Aplicacao` or `Ioc`. The split is consistent even though parts of the
  intended architecture (Repository, DbContext) are not implemented yet.
- **DTOs for spreadsheet construction** — `CelulaDto`, `TotalizadorDto`,
  `TotalizadorCategoria`, `TotalizadorAno` keep Excel-layout concerns out
  of the domain model.

---

## How to run

```powershell
dotnet run --project Financeiro.csproj
```

The console will prompt:

```
Digite o caminho da pasta contendo APENAS as faturas em .CSV:
```

Provide an absolute path. The tool will read every `*.csv` file in that
folder, write the converted Excel + Mobilis CSV into a `Convertido/`
subfolder, and exit on `Enter`.

Filename hints used by the dispatcher (case-insensitive):

- contains `inter`  → Inter parser
- contains `nubank` → Nubank parser
- contains `itau`   → currently routed through the Inter parser

The conversor for account statements (`Extratos`) is implemented but the
call is commented out in [Program.cs:39](Program.cs#L39); uncomment it and
place the files under an `Extratos/` subfolder to use it.

---

## What's working

- [x] CSV parsing for **Inter** credit-card statements
      ([AplicConverterFaturaInter.cs](Aplicacao/Conversor/Faturas/Inter/AplicConverterFaturaInter.cs))
- [x] CSV parsing for **Nubank** credit-card statements
      ([AplicConverterFaturaNubank.cs](Aplicacao/Conversor/Faturas/Nubank/AplicConverterFaturaNubank.cs))
- [x] CSV parsing for **Inter** account statements
      ([AplicConverterExtratoInter.cs](Aplicacao/Conversor/Extratos/Inter/AplicConverterExtratoInter.cs))
- [x] CSV parsing for **Nubank** account statements
      ([AplicConverterExtratoNubank.cs](Aplicacao/Conversor/Extratos/Nubank/AplicConverterExtratoNubank.cs))
- [x] Unified `Fatura` / `Extrato` domain model
- [x] Excel `.xlsx` output via EPPlus with auto-fit columns and per-bank
      ordering
- [x] CSV export in the Mobilis import format
- [x] Shared utilities for date parsing, Brazilian-locale decimal parsing,
      categorization, Excel formatting and totalizers
- [x] DI wiring via `Microsoft.Extensions.DependencyInjection`

## What's partial or scaffolded

- ⚠ **Itaú parser** — dispatcher recognizes the filename but currently
  reuses the Inter parser; a real Itaú adapter is not written.
- ⚠ **Persistence** — `IRepBase<T>` and `IDbService` interfaces exist and
  EF Core + Npgsql are referenced in the project, but there is no
  `DbContext`, no migrations and no repository implementation. Nothing is
  persisted across runs today.
- ⚠ **PdfPig** — referenced as a dependency but not yet used. Likely
  intended for reading Itaú-style PDF statements.
- ⚠ **`Extratos` flow** — implemented but disabled in `Program.cs`.

---

## Target architecture (vision)

The current code is the first slice of a larger design. The intended end
state — the direction the existing `Domain`/`Aplicacao`/`Ioc` split is
pointing toward — looks like this:

```
ConversorFaturas/
├── Domain/
│   ├── Entities/        ── Transaction, BankStatement, Account
│   ├── ValueObjects/    ── Money, Period, Category
│   └── Interfaces/      ── IStatementParser, ITransactionRepository,
│                           IAccountRepository
│
├── Aplicacao/
│   └── UseCases/        ── IngestStatement, AggregateTransactions,
│                           DetectAnomalies, QueryTransactions
│
├── Parsers/             ── one adapter per institution, all implementing
│   ├── Itau/              IStatementParser and registered via DI
│   ├── Nubank/            (replaces the current filename-string dispatch)
│   ├── Inter/
│   ├── MercadoPago/
│   ├── BancoDoBrasil/
│   ├── Bradesco/
│   └── C6/
│
├── Repository/          ── EF Core implementations of the domain
│   ├── Context/           repositories, PostgreSQL via Npgsql, with
│   ├── Configurations/    migrations and deduplication
│   └── Repositories/
│
├── Api/                 ── ASP.NET Core REST layer exposing the use cases
│
└── Assistant/           ── AI-powered finance layer:
                            natural-language queries over the transaction
                            history, spending analysis, anomaly detection
                            (OpenAI / Anthropic integration)
```

The current Itaú "stub", the unused `PdfPig` and `Npgsql` references, and
the empty `IRepBase<T>` / `IDbService` interfaces are placeholders for the
boxes above — they exist in code today only as contracts and dependencies,
waiting to be filled in.

---

## Roadmap

### Parsers — supported institutions

- [x] **Inter** — CSV credit-card statements
- [x] **Inter** — CSV account statements
- [x] **Nubank** — CSV credit-card statements
- [x] **Nubank** — CSV account statements
- [ ] **Itaú** — dedicated parser (currently a stub routed to Inter)
- [ ] **Itaú** — OFX support
- [ ] **Itaú** — PDF support (via the already-referenced PdfPig)
- [ ] **Mercado Pago** — CSV export
- [ ] **Banco do Brasil** — CSV / OFX
- [ ] **Bradesco** — CSV / OFX
- [ ] **C6 Bank** — CSV export

### Layers — architectural completion

- [ ] **Repository layer** — `DbContext`, EF Core configurations, and
      PostgreSQL-backed implementations of `IRepBase<T>`
- [ ] **Persistence + deduplication** — keep history across runs instead of
      regenerating from scratch every time
- [ ] **Parser registry** — replace filename-based dispatch with
      `IStatementParser` strategies registered through DI
- [ ] **REST API layer** (ASP.NET Core) — expose use cases to external
      consumers (web UI, mobile, scripts)
- [ ] **Use-case layer split** — break `AplicConversorFaturas` into smaller
      use cases (`IngestStatement`, `AggregateTransactions`, etc.)
- [ ] **Value objects** — `Money`, `Period`, `Category` instead of raw
      `decimal` / `string` fields on `Fatura` and `Extrato`
- [ ] **Logging pipeline** — wire `Microsoft.Extensions.Logging` (already
      referenced) into every layer

### Product features

- [ ] Re-enable the `Extratos` flow in `Program.cs` and unify it with the
      `Faturas` pipeline
- [ ] Configurable output: Excel only, CSV only, or both
- [ ] Rules-based transaction categorization engine
- [ ] AI financial assistant — natural-language queries over the
      transaction history (OpenAI / Anthropic)
- [ ] Spending anomaly detection
- [ ] Monthly / yearly spending reports beyond the current Excel totalizers
- [ ] Unit tests per parser using fixture CSVs

---

## Notes

- The Excel output requires EPPlus's non-commercial license context, which
  is set explicitly in [`AplicConversorFaturas.CriarPlanilha`](Aplicacao/Conversor/Faturas/AplicConversorFaturas.cs#L53).
  For commercial use, an EPPlus license must be configured.
- All in-code identifiers and comments are in Portuguese (`Fatura`,
  `Extrato`, `Banco`, `Categoria`, `Valor`) — this README is the only
  English-language entry point.
