# Health Checks for ASP.NET Applications

This repository contains a set of extension methods for adding health checks to your ASP.NET Core application. Health checks are essential for monitoring the health and status of various components and services in your application.

## Getting Started

1. Clone or download this repository to your local development environment.
2. Include the `HealthCheckExtensions.cs` file in your ASP.NET Core project.

To set up a custom health check endpoint, add the following code in your `Program.cs`:

```csharp
app.UseCustomHealthCheckMapping();
```

This code maps a custom health check endpoint to the `/health` route. It provides the following status codes:

- Healthy: 200 OK
- Degraded: 200 OK
- Unhealthy: 503 Service Unavailable

## Available Health Checks

Add health checks for various components using the provided extension methods in your `Program.cs`:

### SQL Database Health Check

```csharp
services.AddSqlDatabaseHealthChecks(
    connectionString,
    name: "sql-database",  // optional
    tags: new[] { "database" }  // optional
);
```

### MongoDB Health Check

```csharp
services.AddMongoDatabaseHealthChecks(
    connectionString,
    tags: new[] { "database" }  // optional
);
```

### Redis Health Check

```csharp
services.AddRedisHealthChecks(
    connectionString,
    name: "redis-cache",  // optional
    tags: new[] { "cache" }  // optional
);
```

## Health Check Response Format

The `/health` endpoint returns a JSON response with detailed status information for each registered health check:

```json
{
    "status": "Healthy",
    "results": {
        "mongodb": {
            "status": "Healthy",
            "description": "MongoDB health check",
            "data": {}
        },
        "sql-database": {
            "status": "Healthy",
            "description": "SQL Server health check",
            "data": {}
        }
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
