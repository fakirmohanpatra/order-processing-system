# Minimal Purchase Flow - Complete End-to-End

## Architecture Layer Flow

```
┌─────────────────────────────────────────────────────────────────┐
│ 1. API LAYER (OrderService.API)                                │
│    OrderController.CreateOrder()                               │
│    Receives HTTP POST /api/orders with OrderRequest DTO         │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ 2. APPLICATION LAYER (OrderService.Application)                │
│    CreateOrderCommandHandler.HandleAsync()                      │
│    - Converts OrderRequest → CreateOrderCommand                │
│    - Executes business logic (validation)                      │
│    - Creates domain entity: Purchase.Create()                  │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ 3. DOMAIN LAYER (OrderService.Domain)                          │
│    Purchase Entity + PurchaseStatus Enum                        │
│    - Domain logic: validation, state transitions               │
│    - Purchase.Create() factory method                          │
│    - Status: PurchaseStatus.Created                            │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ 4. INFRASTRUCTURE LAYER (OrderService.Infrastructure)          │
│    OrderRepository.AddAsync()                                   │
│    - Receives Purchase entity                                  │
│    - Saves via AppDbContext                                   │
│    - Calls SaveChangesAsync()                                 │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ 5. DATA ACCESS LAYER (Entity Framework Core)                   │
│    AppDbContext.Purchases DbSet                                 │
│    - Maps Purchase entity to Purchases table                   │
│    - SQL INSERT statement executed                             │
└────────────────┬────────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────────┐
│ 6. DATABASE (SQL Server)                                        │
│    OrderServiceDb.dbo.Purchases table                          │
│    - Stores: Id, ProductName, Quantity, UnitPrice, Status     │
└─────────────────────────────────────────────────────────────────┘
```

## Request Flow with Code

### Request:
```http
POST http://localhost:5001/api/orders
Content-Type: application/json

{
  "productName": "Laptop",
  "quantity": 1,
  "price": 999.99,
  "customerId": "CUST-001"
}
```

### Step 1: API Layer
```csharp
// OrderController.cs
[HttpPost]
public async Task<ActionResult<OrderResponse>> CreateOrder(
    [FromBody] OrderRequest orderRequest, 
    CancellationToken cancellationToken)
{
    var command = CreateOrderCommand.FromRequest(orderRequest);
    var response = await _createOrderHandler.HandleAsync(command, cancellationToken);
    return CreatedAtAction(nameof(CreateOrder), new { id = response.Id }, response);
}
```

### Step 2: Convert to Command
```csharp
// CreateOrderCommand.cs
public static CreateOrderCommand FromRequest(OrderRequest request)
{
    return new CreateOrderCommand(
        request.ProductName,      // "Laptop"
        request.Quantity,         // 1
        request.Price,            // 999.99
        request.CustomerId        // "CUST-001"
    );
}
```

### Step 3: Application Handler
```csharp
// CreateOrderCommandHandler.cs
public async Task<OrderResponse> HandleAsync(
    CreateOrderCommand command, 
    CancellationToken cancellationToken)
{
    // Create domain entity using factory method
    var order = Purchase.Create(
        command.ProductName,      // "Laptop"
        command.Quantity,         // 1
        command.Price,            // 999.99
        command.CustomerId        // "CUST-001"
    );
    
    // order.Id = new Guid()
    // order.Status = PurchaseStatus.Created (1)
    // order.CreatedAt = DateTime.UtcNow
    
    // Persist to database via repository
    await _repository.AddAsync(order, cancellationToken);
    
    // Return response DTO
    return new OrderResponse
    {
        Id = order.Id,
        Status = order.Status.ToString(),  // "Created"
        CreatedAt = order.CreatedAt
    };
}
```

### Step 4: Domain Entity Creation
```csharp
// Purchase.cs - Factory Pattern
public static Purchase Create(string productName, int quantity, decimal price, string customerId)
{
    // Validation
    if (string.IsNullOrWhiteSpace(productName))
        throw new ArgumentException("Product name cannot be empty");
    if (quantity <= 0)
        throw new ArgumentException("Quantity must be greater than zero");
    if (price <= 0)
        throw new ArgumentException("Price must be greater than zero");
    
    // Create entity
    return new Purchase
    {
        Id = Guid.NewGuid(),
        ProductName = productName,
        Quantity = quantity,
        UnitPrice = price,
        CustomerId = customerId,
        Status = PurchaseStatus.Created,
        CreatedAt = DateTime.UtcNow
    };
}
```

### Step 5: Infrastructure Persistence
```csharp
// OrderRepository.cs
public async Task AddAsync(Purchase order, CancellationToken cancellationToken)
{
    _context.Purchases.Add(order);  // Add to EF context
    await _context.SaveChangesAsync(cancellationToken);  // INSERT to SQL
}
```

### Step 6: Database Mapping
```csharp
// AppDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Purchase>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.ProductName).IsRequired();
        entity.Property(e => e.CustomerId).IsRequired();
        entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
    });
}
```

### Step 7: SQL Execution
```sql
INSERT INTO [OrderServiceDb].[dbo].[Purchases]
    ([Id], [ProductName], [Quantity], [UnitPrice], [CustomerId], [Status], [CreatedAt])
VALUES
    ('550e8400-e29b-41d4-a716-446655440000', 'Laptop', 1, 999.99, 'CUST-001', 1, '2026-02-13T10:30:00Z')
```

### Response:
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "status": "Created",
  "createdAt": "2026-02-13T10:30:00Z"
}
```

## How to Test

### Option 1: Use REST Client Extension (VS Code)
1. Open `OrderService.API/test-flow.http`
2. Click "Send Request" (Ctrl+Alt+R)
3. See response in side panel

### Option 2: Use cURL
```bash
curl -X POST http://localhost:5001/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "productName": "Laptop",
    "quantity": 1,
    "price": 999.99,
    "customerId": "CUST-001"
  }'
```

### Option 3: Use Swagger UI
1. Run: `dotnet run --project OrderService/OrderService.API`
2. Navigate to: http://localhost:5001/swagger
3. Expand `POST /api/orders`
4. Click "Try it out"
5. Enter JSON body
6. Click "Execute"

## Dependency Injection Chain

```
Program.cs
  ├─ builder.Services.AddApplicationServices()
  │   └─ services.AddScoped<CreateOrderCommandHandler>()
  │       └─ Needs: IOrderRepository (injected by Infrastructure)
  │
  └─ builder.Services.AddInfrastructureServices(configuration)
      ├─ services.AddDbContext<AppDbContext>(options => options.UseSqlServer(...))
      │   └─ Creates DbContext for database access
      └─ services.AddScoped<IOrderRepository, OrderRepository>()
          └─ Registers implementation (can be injected into handlers)
```

## Database Setup

1. **Created automatically** via `dbContext.Database.EnsureCreated()` in Program.cs
2. **Database name**: `OrderServiceDb` (from appsettings.json)
3. **Table**: `Purchases`
4. **Columns**: Id, ProductName, Quantity, UnitPrice, CustomerId, Status, CreatedAt

## Validation Flow

The Purchase factory method validates **before persisting**:
- ProductName: required, non-empty
- Quantity: must be > 0
- Price: must be > 0
- CustomerId: required, non-empty

If validation fails, exception thrown before database access ✓

## Key Architectural Principles ✓

1. **DDD**: Domain logic in Purchase entity (factory pattern)
2. **Dependency Inversion**: Infrastructure depends on Domain interfaces
3. **Separation of Concerns**: Each layer has single responsibility
4. **Testability**: All components are injectable and mockable
5. **No Validation Leakage**: Business rules stay in domain
