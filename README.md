# ShopSphere
A production-ready .NET 8 Microservices e-commerce platform demonstrating DDD, CQRS, Vertical Slice &amp; Clean Architecture with Yarp Gateway, Redis, MassTransit, RabbitMQ, and gRPC.

This project covers:

- Microservices Architecture
- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS
- Vertical Slice Architecture
- Repository Pattern
- Dependency Injection
- Domain Events & Integration Events
- REST APIs
- gRPC
- RabbitMQ
- MassTransit
- Redis Distributed Caching
- Cache-Aside Pattern
- API Gateway with YARP
- Rate Limiting
- Entity Framework Core
- SQL Server
- PostgreSQL
- Docker & Docker Compose
- Health Checks
- Global Exception Handling
- Logging
- Validation with FluentValidation
- MediatR
- Mapster
- Refit
- Minimal APIs
- Carter

---

## 🏗️ Solution Architecture

The application is designed around multiple independent microservices.

```text
                         ┌───────────────────┐
                         │     Web Client    │
                         └─────────┬─────────┘
                                   │
                                   ▼
                         ┌───────────────────┐
                         │   YARP Gateway    │
                         │  Reverse Proxy    │
                         └─────────┬─────────┘
                                   │
              ┌────────────────────┼────────────────────┐
              │                    │                    │
              ▼                    ▼                    ▼
       ┌─────────────┐      ┌─────────────┐      ┌─────────────┐
       │   Catalog   │      │   Basket    │      │  Ordering   │
       │ Microservice│      │ Microservice│      │ Microservice│
       └──────┬──────┘      └──────┬──────┘      └──────┬──────┘
              │                    │                    │
              ▼                    ▼                    ▼
       ┌─────────────┐      ┌─────────────┐      ┌─────────────┐
       │ PostgreSQL  │      │    Redis    │      │ SQL Server  │
       │  + Marten   │      │    Cache    │      │ + EF Core   │
       └─────────────┘      └─────────────┘      └─────────────┘
                                   │
                         ┌─────────┴─────────┐
                         │                   │
                         ▼                   ▼
                  ┌─────────────┐     ┌─────────────┐
                  │   Discount  │     │  RabbitMQ   │
                  │   gRPC      │     │  Event Bus  │
                  └─────────────┘     └──────┬──────┘
                                             │
                                             ▼
                                      ┌─────────────┐
                                      │   Ordering  │
                                      │   Consumer  │
                                      └─────────────┘
```

---

# 📦 Microservices

## 1. Catalog Microservice

Responsible for product/catalog management.

### Technologies

- ASP.NET Core 8
- Minimal APIs
- Vertical Slice Architecture
- CQRS
- MediatR
- FluentValidation
- Carter
- PostgreSQL
- Marten
- Docker

### Concepts

- Feature-based organization
- Commands & Queries
- Request validation
- Document database
- Cross-cutting concerns
- Global exception handling
- Health checks

---

## 2. Basket Microservice

Responsible for shopping carts.

### Technologies

- ASP.NET Core 8 Web API
- Redis
- gRPC Client
- RabbitMQ
- MassTransit
- Docker

### Concepts

- CRUD APIs
- Distributed caching
- Cache-Aside Pattern
- Proxy Pattern
- Decorator Pattern
- Synchronous service-to-service communication
- Asynchronous event publishing

### Flow

```text
Client
  │
  ▼
Basket API
  │
  ├──────────────► Redis
  │
  └──────────────► Discount Service
                       │
                       │ gRPC
                       ▼
                  Discount Result

Basket Checkout
  │
  ▼
RabbitMQ
  │
  ▼
Ordering Service
```

---

## 3. Discount Microservice

Responsible for calculating discounts.

### Technologies

- ASP.NET Core 8
- gRPC
- Protocol Buffers
- Entity Framework Core
- SQLite
- Docker

### Concepts

- gRPC server
- Protobuf contracts
- High-performance synchronous communication
- EF Core migrations
- Containerized database

---

## 4. Ordering Microservice

Responsible for processing customer orders.

### Technologies

- ASP.NET Core 8
- Clean Architecture
- DDD
- CQRS
- MediatR
- FluentValidation
- Mapster
- EF Core
- SQL Server
- RabbitMQ
- MassTransit
- Docker

### Concepts

- Entities
- Value Objects
- Aggregates
- Aggregate Roots
- Repositories
- Domain Services
- Domain Events
- Integration Events
- CQRS
- Domain-driven design
- Database migrations

### Checkout Flow

```text
Basket Service
      │
      │ BasketCheckout Event
      ▼
   RabbitMQ
      │
      ▼
Ordering Service
      │
      ├── Validate Event
      ├── Create Order
      ├── Apply Domain Logic
      └── Persist Order
```

---

# 🌐 API Gateway

The project uses **YARP (Yet Another Reverse Proxy)** as an API Gateway.

```text
                 Client
                   │
                   ▼
            ┌──────────────┐
            │ YARP Gateway │
            └───────┬──────┘
                    │
        ┌───────────┼───────────┐
        ▼           ▼           ▼
     Catalog      Basket     Ordering
```

### Concepts

- Reverse Proxy
- Gateway Routing Pattern
- Routes
- Clusters
- Destinations
- Path transforms
- Rate limiting

The gateway provides a single entry point for clients while hiding internal service topology.

---

# 📨 Microservices Communication

Two major communication styles are implemented.

## Synchronous Communication

Used when the calling service needs an immediate response.

```text
Basket Service
      │
      │ gRPC
      ▼
Discount Service
      │
      ▼
Discount Result
```

### Technology

**gRPC + Protocol Buffers**

---

## Asynchronous Communication

Used when services should communicate without waiting for an immediate response.

```text
Basket Service
      │
      │ Publish Event
      ▼
   RabbitMQ
      │
      │ Consume
      ▼
Ordering Service
```

### Technology

- RabbitMQ
- MassTransit
- Publish/Subscribe
- Integration Events

---

# 🧠 Architecture Patterns

## Clean Architecture

The Ordering service follows a dependency-inverted architecture.

```text
┌─────────────────────────────┐
│       Presentation         │
│       Web API              │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│       Application           │
│   CQRS / Use Cases / DTOs   │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│          Domain             │
│ Entities / Value Objects     │
│ Aggregates / Domain Events   │
└──────────────┬──────────────┘
               ▲
               │
┌──────────────┴──────────────┐
│       Infrastructure        │
│ EF Core / SQL / Messaging   │
└─────────────────────────────┘
```

The core business logic remains independent from infrastructure concerns.

---

# 📐 Domain-Driven Design

Important DDD concepts covered in this project:

### Entity

An object identified by its identity.

```text
Order
Customer
Product
```

### Value Object

An object defined by its values rather than identity.

```text
Address
Money
OrderItem
```

### Aggregate

A consistency boundary around related domain objects.

```text
Order
 ├── OrderItem
 ├── ShippingAddress
 └── Payment
```

### Aggregate Root

The main entry point for modifying an aggregate.

```text
Order
  ↓
OrderItem
OrderAddress
Payment
```

External code should interact with the aggregate through its root.

---

# 🔄 CQRS

CQRS separates operations that modify state from operations that read state.

```text
                 Application
                     │
          ┌──────────┴──────────┐
          ▼                     ▼
       Command                Query
          │                     │
          ▼                     ▼
      Command Handler       Query Handler
          │                     │
          ▼                     ▼
     Write Model            Read Model
```

### Commands

Change state.

Examples:

```text
CreateOrder
UpdateProduct
DeleteProduct
CheckoutBasket
```

### Queries

Read state.

Examples:

```text
GetProduct
GetProducts
GetOrder
GetBasket
```

---

# 📢 Domain Events vs Integration Events

### Domain Event

Represents something that happened inside a domain.

```text
OrderCreated
OrderPaid
BasketCheckedOut
```

Usually handled within the same bounded context/application.

### Integration Event

Used to communicate a business event between separate services.

```text
BasketCheckoutIntegrationEvent
```

Example:

```text
Basket Service
      │
      ▼
BasketCheckoutIntegrationEvent
      │
      ▼
RabbitMQ
      │
      ▼
Ordering Service
```

---

# ⚡ Redis & Cache-Aside Pattern

Redis is used as a distributed cache for the Basket service.

### Cache-Aside Flow

```text
             Request
                │
                ▼
             Redis?
             /    \
           Yes     No
           │        │
           ▼        ▼
       Return    Database
        Data        │
                    ▼
                 Redis
                    │
                    ▼
                Return Data
```

Benefits:

- Faster reads
- Reduced database load
- Improved response time
- Distributed caching across application instances

---

# 🐳 Docker

The services and supporting infrastructure are containerized.

Expected containers include:

```text
Catalog API
Basket API
Discount API
Ordering API
YARP Gateway
PostgreSQL
SQL Server
Redis
RabbitMQ
```

Docker Compose is used to orchestrate the multi-container development environment.

---

# 🛡️ Cross-Cutting Concerns

The project also demonstrates common production concerns:

- Structured logging
- Global exception handling
- Validation
- Health checks
- Rate limiting
- Configuration management
- Environment variables
- Dependency Injection

---

# 🛠️ Technology Stack

| Category | Technologies |
|---|---|
| Language | C# 12 |
| Framework | .NET 8 / ASP.NET Core 8 |
| API | Web API / Minimal API |
| Architecture | Clean / Vertical Slice / Microservices |
| Design | DDD / CQRS |
| ORM | Entity Framework Core |
| Databases | SQL Server / PostgreSQL / SQLite |
| Cache | Redis |
| Messaging | RabbitMQ |
| Messaging Abstraction | MassTransit |
| RPC | gRPC |
| Gateway | YARP |
| Validation | FluentValidation |
| Mediator | MediatR |
| Mapping | Mapster |
| Containerization | Docker / Docker Compose |
| API Client | Refit |
| Source Control | Git / GitHub |

---

# 📚 Roadmap

This repository is being developed progressively.

### Phase 1 — Microservices Fundamentals

- [ ] Microservices architecture
- [ ] Service boundaries
- [ ] Independent databases
- [ ] Inter-service communication
- [ ] Docker basics

### Phase 2 — Catalog

- [ ] Minimal APIs
- [ ] Vertical Slice Architecture
- [ ] CQRS
- [ ] MediatR
- [ ] FluentValidation
- [ ] PostgreSQL
- [ ] Marten

### Phase 3 — Basket

- [ ] REST API
- [ ] Redis
- [ ] Cache-Aside
- [ ] Proxy Pattern
- [ ] Decorator Pattern
- [ ] gRPC client
- [ ] RabbitMQ publishing

### Phase 4 — Discount

- [ ] gRPC server
- [ ] Protobuf
- [ ] EF Core
- [ ] SQLite

### Phase 5 — Ordering

- [ ] Clean Architecture
- [ ] DDD
- [ ] Entities
- [ ] Value Objects
- [ ] Aggregates
- [ ] Aggregate Roots
- [ ] CQRS
- [ ] Domain Events
- [ ] Integration Events
- [ ] SQL Server
- [ ] EF Core

### Phase 6 — Messaging

- [ ] RabbitMQ
- [ ] MassTransit
- [ ] Publish/Subscribe
- [ ] Event-driven communication
- [ ] Event consumers

### Phase 7 — API Gateway

- [ ] YARP
- [ ] Reverse Proxy
- [ ] Gateway Routing
- [ ] Rate Limiting

### Phase 8 — Containerization

- [ ] Dockerfiles
- [ ] Docker Compose
- [ ] Multi-container applications
- [ ] Environment variables
- [ ] Service networking

---

# 💡 Key Design Patterns

Patterns explored during the project include:

- Dependency Injection
- Repository
- Proxy
- Decorator
- Cache-Aside
- Gateway Routing
- CQRS
- Publish/Subscribe
- Domain Events
- Integration Events

These patterns complement the design-pattern concepts I have been studying separately.


---

# 🎯  Objectives

- Design a microservices-based application
- Define service boundaries
- Build production-style ASP.NET Core APIs
- Implement synchronous and asynchronous communication
- Use Redis for distributed caching
- Implement RabbitMQ-based event-driven communication
- Apply DDD principles
- Implement CQRS
- Apply Clean Architecture
- Build and run multi-container applications with Docker
- Design API Gateway routing
- Understand distributed-system trade-offs
- Explain architectural decisions during technical interviews

---

# 📖 Course Reference

This project is based on:

**.NET 8 Microservices: DDD, CQRS, Vertical/Clean Architecture**

Instructor: **Mehmet Ozkaya**

The course covers a complete e-commerce example using Catalog, Basket, Discount, Ordering, API Gateway, messaging, caching, databases, and Docker.

This repository contains my own learning implementation, notes, experiments, and architectural understanding.

---

## ⭐ Final Goal

The ultimate goal of this repository is to move from:

```text
.NET Developer
      ↓
Backend Developer
      ↓
Microservices Developer
      ↓
System Design
      ↓
Software Engineer capable of
designing scalable distributed systems
```

> **Learning by building — understanding the architecture, not just the code.**
