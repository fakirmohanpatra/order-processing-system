# Order Processing System

A production-grade backend system built using ASP.NET Core, showcasing event-driven architecture, CQRS, async processing, observability, and cloud-native practices.

## Tech Stack
- **ASP.NET Core** (.NET 8)
- **RabbitMQ** (Event-driven messaging)
- **PostgreSQL** (Primary database)
- **Redis** (Caching & Idempotency)
- **CQRS + MediatR** (Command/Query separation)
- **Docker & Docker Compose** (Containerization & orchestration)
- **Nginx** (API Gateway & Load Balancer)
- **OpenTelemetry + Jaeger** (Distributed tracing)
- **Prometheus + Grafana** (Metrics & visualization)
- **Rate Limiting & Retry Logic** (Resilience patterns)

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│ Browser / Postman (Client)                                      │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ Nginx (Port 8080) - Reverse Proxy                               │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ API Gateway (Port 5004)                                         │
│ Routes: /orders → Order Service | /payments → Payment Service   │
└────────────────┬────────────────────────────────────────────────┘
                 │
        ┌────────┴────────┬──────────────┐
        ▼                 ▼              ▼
    ┌────────┐      ┌──────────┐  ┌─────────────┐
    │ Order  │      │ Payment  │  │Notification │
    │Service │      │ Service  │  │  Service    │
    │(5001)  │      │(5002)    │  │  (5003)     │
    └────────┘      └──────────┘  └─────────────┘

Data & Messaging:
  PostgreSQL (5432) | Redis (6379) | RabbitMQ (5672)

Observability:
  Jaeger (6831) | Prometheus (9090) | Grafana (3000)
```

## Services

1. **Order Service** (Port 5001) - Core order management
2. **Payment Service** (Port 5002) - Payment processing
3. **Notification Service** (Port 5003) - Email/SMS notifications
4. **API Gateway** (Port 5004) - Request routing
5. **Nginx** (Port 8080) - Public entry point

## Key Features

- ✅ **CQRS** - Read/Write separation with MediatR
- ✅ **Event-Driven** - RabbitMQ for async communication
- ✅ **Domain-Driven Design** - Clean domain entities and value objects
- ✅ **Idempotent APIs** - Redis-backed request deduplication
- ✅ **Distributed Tracing** - OpenTelemetry + Jaeger
- ✅ **Metrics & Monitoring** - Prometheus + Grafana dashboards
- ✅ **Resilience** - Retry with exponential backoff, circuit breaker patterns
- ✅ **Container-Ready** - Docker Compose with full infrastructure

---

## Getting Started

### Prerequisites

Ensure you have installed:
- **Docker** & **Docker Compose** (v2.0+)
- **.NET 8 SDK** (optional, for local development)
- **Git**

### Step 1: Clone the Repository

```bash
git clone <repo-url>
cd order-processing-system
```

### Step 2: Start Infrastructure & Services

Start all dependent services with a single command:

```bash
docker-compose up -d
```

This will spin up:
- ✅ PostgreSQL (orderdb on 5432)
- ✅ Redis (6379)
- ✅ RabbitMQ (5672, 15672)
- ✅ Order Service (5001)
- ✅ Payment Service (5002)
- ✅ Notification Service (5003)
- ✅ API Gateway (5004)
- ✅ Nginx (8080)
- ✅ Jaeger Tracing (6831, 6831/udp, 16686)
- ✅ Prometheus (9090)
- ✅ Grafana (3000)

**Monitor startup:**

```bash
docker-compose logs -f
```

Wait for all services to be healthy (~30-45 seconds).

### Step 3: Verify Services Are Running

```bash
# Check all containers
docker-compose ps

# Expected output:
# NAME              STATUS              PORTS
# postgres          Up (healthy)        5432
# redis             Up                  6379
# rabbitmq          Up                  5672, 15672
# order-service     Up                  5001
# payment-service   Up                  5002
# notification-service Up               5003
# api-gateway       Up                  5004
# nginx             Up                  8080
# jaeger            Up                  6831, 16686
# prometheus        Up                  9090
# grafana           Up                  3000
```

---

## Accessing Services & Dashboards

### API Entry Points

| Service | URL | Purpose |
|---------|-----|---------|
| **Public Entry** | `http://localhost:8080` | All traffic through Nginx |
| **Order Service** | `http://localhost:5001` | Direct (Swagger at `/swagger`) |
| **Payment Service** | `http://localhost:5002` | Direct (Swagger at `/swagger`) |
| **Notification Service** | `http://localhost:5003` | Direct (Swagger at `/swagger`) |

### Observability Dashboards

| Tool | URL | Credentials |
|------|-----|-------------|
| **Jaeger (Traces)** | `http://localhost:16686` | None |
| **Prometheus (Metrics)** | `http://localhost:9090` | None |
| **Grafana (Dashboards)** | `http://localhost:3000` | admin / admin |
| **RabbitMQ Management** | `http://localhost:15672` | ops_user / ops_password |

### Database Access (Optional)

Connect to PostgreSQL:

```bash
psql -h localhost -U ops_user -d orderdb
# Password: ops_password
```

Redis CLI:

```bash
redis-cli -h localhost
```

---

## Testing the System

### 1. Create an Order

```bash
curl -X POST http://localhost:8080/orders \
  -H "Content-Type: application/json" \
  -d '{
    "productName": "Laptop",
    "quantity": 1,
    "price": 999.99,
    "customerId": "CUST-001"
  }'
```

**Expected Response:**

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "status": "Created",
  "createdAt": "2026-02-25T12:34:56Z"
}
```

### 2. Get Order Details

```bash
curl http://localhost:8080/orders/{order-id}
```

### 3. Check Event Propagation

1. **RabbitMQ Dashboard**: Visit `http://localhost:15672` to see message queues
2. **Jaeger Traces**: Visit `http://localhost:16686` → Search for "order-service" to see distributed traces
3. **Prometheus**: Visit `http://localhost:9090` → Query `http_request_duration_seconds`

### 4. View Grafana Dashboards

1. Open `http://localhost:3000`
2. Login: `admin` / `admin`
3. Navigate to **Dashboards** → Look for available dashboards
4. Explore metrics from recent API calls

---

## Workflow Example: Complete Order Flow

```
┌─────────────────────────────────────────────────────────────┐
│ 1. POST /api/orders (Create Order)                          │
│    Client → Nginx → Gateway → Order Service                │
└─────────────────────┬───────────────────────────────────────┘
                      │
        ✅ Order created in PostgreSQL
        ✅ OrderCreated event published to RabbitMQ
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│ 2. RabbitMQ: OrderCreated Event                             │
│    Payment Service Consumer receives event                  │
└─────────────────────┬───────────────────────────────────────┘
                      │
        ✅ Payment processed (async)
        ✅ PaymentProcessed event published
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│ 3. RabbitMQ: PaymentProcessed Event                         │
│    Notification Service Consumer receives event             │
└─────────────────────┬───────────────────────────────────────┘
                      │
        ✅ Notification sent (email/SMS)
        ✅ Event logged to Jaeger/Prometheus
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│ 4. Observability Chain Complete                             │
│    Trace visible in Jaeger, metrics in Prometheus/Grafana  │
└─────────────────────────────────────────────────────────────┘
```

---

## Debugging & Troubleshooting

### View Service Logs

```bash
# Order Service
docker-compose logs -f order-service

# All services
docker-compose logs -f

# Specific service output with timestamps
docker-compose logs --timestamps payment-service
```

### Health Checks

```bash
# Order Service health
curl http://localhost:5001/health

# Payment Service health
curl http://localhost:5002/health

# Notification Service health
curl http://localhost:5003/health
```

### Common Issues

**"Connection refused" errors:**

```bash
# Services starting up, wait 1-2 minutes for full initialization
docker-compose ps

# Check specific service logs
docker-compose logs order-service
```

**Database not ready:**

```bash
# Wait for postgres to be healthy
docker-compose logs postgres

# Manually check connection
psql -h localhost -U ops_user -d orderdb
```

**RabbitMQ not accessible:**

```bash
# Check RabbitMQ status
docker-compose logs rabbitmq

# Verify management UI
curl http://localhost:15672/api/overview -u ops_user:ops_password
```

---

## Stopping & Cleaning Up

```bash
# Stop all services (keeps data)
docker-compose stop

# Stop and remove containers (keeps volumes)
docker-compose down

# Remove everything including data
docker-compose down -v
```

---

## Project Structure

```
├── OrderService/                 # Order microservice (CQRS)
│   ├── OrderService.API/        # Controllers & endpoints
│   ├── OrderService.Application/ # Commands, queries, handlers
│   ├── OrderService.Domain/      # Entities, value objects, events
│   └── OrderService.Infrastructure/ # DB, repositories, messaging
├── PaymentService/              # Payment microservice
├── NotificationService/         # Notification microservice
├── ApiGateway/                  # Central routing
├── docker-compose.yml           # Infrastructure orchestration
└── docker/                       # Docker configs (Nginx, Prometheus, etc)
```

---

## Next Steps

1. ✅ All services running
2. 📊 Monitor activity in Grafana/Prometheus
3. 🔍 Trace requests in Jaeger
4. 🐰 View events in RabbitMQ Management
5. 🧪 Run integration tests (coming soon)
6. 📈 Add custom Grafana dashboards

---

## Architecture Documentation

See [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed system design.

---

**Last Updated:** February 2026  
**Status:** Active Development
