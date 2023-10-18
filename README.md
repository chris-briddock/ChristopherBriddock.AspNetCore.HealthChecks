# Health Checks for ASP.NET Applications

This repository contains a set of extension methods for adding health checks to your ASP.NET Core application. Health checks are essential for monitoring the health and status of various components and services in your application.

## Getting Started

1. Clone or download this repository to your local development environment.
2. Include the `HealthCheckExtensions.cs` file in your ASP.NET Core project.

To set up a custom health check endpoint, add the following code in your `Program.cs`:

    app.UseCustomHealthCheckMapping();

This code maps a custom health check endpoint to the `/health` route. It provides status codes for healthy, degraded, and unhealthy states.

To add health checks for various components such as SQL databases, MongoDB, Elasticsearch, Redis, and RabbitMQ, you can use the provided extension methods in your `Program.cs`:

* SQL Database Health Check:

    services.AddSqlDatabaseHealthChecks(connectionString);

* MongoDB Health Check:

    services.AddMongoDatabaseHealthChecks(connectionString);

* Elasticsearch Health Check:

    services.AddElasticSearchHealthChecks(elasticSearchUri);

* Redis Health Check:

    services.AddRedisHealthChecks(connectionString);

* RabbitMQ Health Check:

    services.AddRabbitMQHealthChecks();

Replace `connectionString` and `elasticSearchUri` with the appropriate connection strings or URIs for your specific services.

Your ASP.NET application is now equipped with health checks for various components, and the custom health check endpoint is available at `/health`.

## Usage

Once you have added the health checks to your application, you can monitor the health of different components and services by making HTTP GET requests to the `/health` endpoint. The response will indicate the health status of each component.

For further details on customizing health checks or extending the functionality, refer to the official documentation of ASP.NET Core Health Checks.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
