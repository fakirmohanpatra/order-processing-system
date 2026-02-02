# Order Processing System – Master Checklist

Author: Fakir Mohan Patra  
Focus: Event-Driven Backend Architecture (.NET)

---

## Phase 0 – Repository & Planning
- [x] Create local git repository
- [x] Connect to remote GitHub repository
- [x] Define monorepo structure
- [x] Add .gitignore
- [x] Create documentation folder
- [ ] Finalize architecture decisions (CQRS, async, messaging)

---

## Phase 1 – Infrastructure (Docker Only)
- [ ] Docker Compose for PostgreSQL
- [ ] Docker Compose for RabbitMQ
- [ ] Docker Compose for Redis
- [ ] Single Docker network for all services
- [ ] Environment variables management
- [ ] Health checks for infra services

---

## Phase 2 – Order Service (Core API)
- [ ] ASP.NET Core Web API setup
- [ ] Swagger & health endpoints
- [ ] PostgreSQL DbContext
- [ ] Connection pooling configuration
- [ ] Async DB access
- [ ] Order entity & schema
- [ ] Order status lifecycle

---

## Phase 3 – CQRS & MediatR
- [ ] Introduce MediatR
- [ ] CreateOrderCommand
- [ ] Command handler
- [ ] GetOrderQuery
- [ ] Query handler
- [ ] Separate read/write models (logical CQRS)

---

## Phase 4 – Messaging (RabbitMQ)
- [ ] Define domain events
- [ ] Publish OrderCreated event
- [ ] RabbitMQ exchange & queue setup
- [ ] Consumer setup
- [ ] Message idempotency handling
- [ ] Dead-letter queue configuration

---

## Phase 5 – Background Services
- [ ] Payment worker service
- [ ] Retry with exponential backoff
- [ ] Payment success/failure events
- [ ] Notification worker
- [ ] Graceful shutdown handling

---

## Phase 6 – Third-Party API Integration
- [ ] Exchange rate API integration
- [ ] Handle rate limiting (429)
- [ ] Retry & backoff strategy
- [ ] Timeout handling
- [ ] Circuit breaker discussion

---

## Phase 7 – Caching & Idempotency
- [ ] Redis setup
- [ ] Idempotency keys
- [ ] Cache frequently accessed reads
- [ ] Cache invalidation strategy

---

## Phase 8 – API Gateway & Security
- [ ] Nginx reverse proxy
- [ ] Load balancing
- [ ] SSL termination
- [ ] Rate limiting at edge
- [ ] Request correlation IDs

---

## Phase 9 – Observability
- [ ] Structured logging
- [ ] OpenTelemetry integration
- [ ] Distributed tracing
- [ ] Prometheus metrics
- [ ] Grafana dashboards
- [ ] Application Insights (optional)

---

## Phase 10 – Failure Scenarios
- [ ] RabbitMQ downtime
- [ ] Database timeout
- [ ] Duplicate message processing
- [ ] Payment failures
- [ ] Retry storm prevention
- [ ] Eventual consistency validation

---

## Phase 11 – Kubernetes (Optional / Advanced)
- [ ] Containerize services
- [ ] Kubernetes manifests
- [ ] ConfigMaps & Secrets
- [ ] Liveness & readiness probes
- [ ] Horizontal scaling
- [ ] Rolling deployments

---

## Final
- [ ] Architecture documentation
- [ ] Design decisions documented
- [ ] Resume bullet points updated
- [ ] Interview-ready explanations prepared
