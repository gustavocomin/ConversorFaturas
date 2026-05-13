# ConversorFaturas

> Multi-bank financial statement ingestion and aggregation engine built with C# .NET and Clean Architecture.

ConversorFaturas parses bank statements and invoices from multiple Brazilian financial institutions,
normalizes them into a unified financial model, and persists the aggregated data for downstream
analysis — with a planned AI-powered personal finance assistant layer on top.

---

## What it does

Brazilian banks export statements in incompatible, proprietary formats. ConversorFaturas solves
this by implementing a dedicated parser per institution, running them through a shared normalization
pipeline, and producing a clean, unified transaction record regardless of source.

**Supported institutions:**
- Itaú (OFX / PDF statement)
- Nubank (CSV export)
- Banco Inter (CSV export)
- Mercado Pago (CSV export)

**Processing pipeline:**
```
Raw statement file
       │
       ▼
┌─────────────────┐     one per bank
│  Parser layer   │  ←  (Strategy pattern)
│  Itaú / Nubank  │
│  Inter / MercPg │
└────────┬────────┘
         │  normalized transactions
         ▼
┌─────────────────┐
│  Domain layer   │  ← value objects, business rules,
│  (aggregation)  │     deduplication, categorization
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Persistence    │  ← repository pattern, SQL Server
└─────────────────┘
         │
         ▼
    [Planned] AI financial assistant
    — spending analysis, anomaly detection,
      natural language queries over transaction history
```

---

## Architecture

Built with Clean Architecture and DDD principles:

```
ConversorFaturas/
├── Domain/          # Entities, value objects, business rules
│   ├── Entities/    # Transaction, BankStatement, Account
│   └── Interfaces/  # IStatementParser, ITransactionRepository
│
├── Aplicacao/       # Use cases, orchestration
│   └── UseCases/    # IngestStatement, AggregateTransactions
│
├── Financeiro/      # Bank-specific parser implementations
│   ├── Itau/
│   ├── Nubank/
│   ├── Inter/
│   └── MercadoPago/
│
├── Repository/      # Data access (Entity Framework Core)
├── Ioc/             # Dependency injection configuration
├── Common/          # Shared utilities, extensions
└── Program.cs       # Entry point
```

**Key patterns:** Strategy (one parser per bank) · Repository · Clean Architecture · DDD

---

## Tech stack

- **Runtime:** C# .NET 8 / ASP.NET Core
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Architecture:** Clean Architecture, DDD, SOLID
- **Parsing:** Custom per-bank adapters (OFX, CSV, PDF extraction)

---

## Roadmap

- [x] Multi-bank statement parsing (Itaú, Nubank, Inter, Mercado Pago)
- [x] Unified transaction domain model
- [x] Aggregation and persistence pipeline
- [ ] REST API layer for external consumption
- [ ] Transaction categorization engine (rules-based)
- [ ] OpenAI integration — natural language queries over transaction history
- [ ] Spending anomaly detection

---

## Why this project

Most personal finance tools in Brazil rely on open banking APIs that require user OAuth consent
per institution. ConversorFaturas takes a different approach — direct file ingestion — making it
useful for users who want to process historical data in bulk without connecting live bank access.

The parser-per-bank Strategy pattern also means adding a new institution requires only a new
adapter class, with zero changes to the domain or persistence layers.
