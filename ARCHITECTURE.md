# Order Processing System – Architecture

## High-level Request Flow

```
Browser
   │
   ▼
Nginx (8080)
   │
   ▼
API Gateway (5004)
   │
   ├─ /orders/ → Order Service (5001)
   ├─ /payments/ → Payment Service (5002)
   └─ /notifications/ → Notification Service (5003)
```

## Explanation (Step by Step)

### 1) Browser (Client)

* User, frontend app, Postman, or curl.
* Sends requests to a **single public entry point**: `http://localhost:8080`.

### 2) Nginx (Port 8080)

* Acts as a **reverse proxy / traffic cop**.
* Receives all incoming HTTP traffic.
* Forwards requests to the **API Gateway**.
* Contains **no business logic**.

### 3) API Gateway (Port 5004 → internal 80)

* Central routing and orchestration layer.
* Inspects request paths and forwards them to the correct backend service.
* Example routing:

  * `/orders/*` → Order Service
  * `/payments/*` → Payment Service
  * `/notifications/*` → Notification Service

### 4) Docker Internal Network

* Services communicate using **Docker service names**, not `localhost`.
* Examples:

  * `http://order-service:80`
  * `http://payment-service:80`
  * `http://notification-service:80`

### 5) Microservices

* **Order Service**: Order-related business logic and persistence.
* **Payment Service**: Payment processing.
* **Notification Service**: Emails, messages, notifications.
* Each service:

  * Runs in its own container
  * Is independently deployable
  * Has a single responsibility

## Why This Architecture

* **Single Entry Point**: Only Nginx + Gateway are exposed.
* **Security**: Internal services are hidden from the outside world.
* **Scalability**: Services can scale independently.
* **Maintainability**: Clear separation of concerns.
* **Production-grade**: Matches real-world microservice systems.

## Notes

* Nginx should forward **all traffic** to the API Gateway.
* API Gateway should own **service-level routing**.
* Avoid duplicating routing logic in both Nginx and services.
