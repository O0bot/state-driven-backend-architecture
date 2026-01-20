# ASP.NET Core State-Driven Workflow Architecture

## What this project demonstrates

This project demonstrates a state-driven backend architecture in ASP.NET Core, designed to decouple HTTP requests from business processing.

The focus is not on CRUD or UI, but on understanding:
- the ASP.NET Core hosting model
- dependency injection lifetimes
- background workers
- state-based workflow transitions
- request-independent processing

## Architecture overview

The system is structured around explicit state transitions instead of request-driven execution.

Core components:
- **Domain**: pure state and workflow states
- **Infrastructure**: persistence abstraction (in-memory store)
- **HTTP endpoints**: accept intent and create state only
- **Background workers**: observe state and move the workflow forward

HTTP does not perform validation or processing.
All business progression happens in background services.

## Request vs background processing

HTTP endpoints are intentionally thin.

Responsibilities:
- HTTP:
  - accept requests
  - create workflow state
  - return identifiers
- Background workers:
  - validate uploads
  - advance workflow states
  - retry safely on failure

This avoids long-running HTTP requests, timeouts, and partial failures,
and allows the system to recover safely after restarts.

## Dependency Injection & lifetimes

The application uses ASP.NET Core’s DI container explicitly.

Lifetimes:
- **Singleton**:
  - in-memory upload store (acts as persistence boundary)
- **Scoped**:
  - unit-of-work services (one scope per request or worker iteration)

HTTP requests and background workers do not share scopes.
Workers explicitly create scopes to ensure correct lifetime isolation.

## Why this architecture

This architecture makes workflow execution:
- resilient to crashes
- independent from HTTP lifecycles
- easy to reason about and extend
- suitable for scaling with multiple workers

The goal of this project is to demonstrate architectural understanding
rather than framework usage.

## Personal note

I built this project because I reached a point where implementing features was no longer enough for me.
Despite having professional experience, I wanted a deeper understanding of backend architecture,
system lifecycles, and why certain design decisions exist beyond framework usage.

This project is intentionally challenging and serves as a way to confront gaps in my architectural understanding,
with the goal of becoming a more deliberate and responsible mid-level engineer.