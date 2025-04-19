using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace ChristopherBriddock.AspNetCore.HealthChecks;

public static class HealthCheckExtensions
{
    /// <summary>
    /// Maps a custom health check endpoint to the specified route.
    /// </summary>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> to which the health check mapping is added.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> for further configuration.</returns>
    public static IEndpointRouteBuilder UseCustomHealthCheckMapping(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResultStatusCodes =
           {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
           },
            ResponseWriter = HealthCheckResponseWriter.WriteResponse,
            AllowCachingResponses = true
        });

        return app;
    }

    /// <summary>
    /// Adds database health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which health checks will be added.</param>
    /// <param name="connectionString">The connection string used to connect to the MS SQL database</param>
    /// <param name="name">The name of the health check.</param>
    /// <param name="tags">The tags to apply to the health check.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSqlDatabaseHealthChecks(this IServiceCollection services,
                                                                string connectionString,
                                                                string? name = null,
                                                                IEnumerable<string>? tags = null)
    {
        // Ensure that the services parameter is not null
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        // Add a SQL Server health check to the IServiceCollection
        services.AddHealthChecks().AddSqlServer(connectionString: connectionString,
                                                healthQuery: "SELECT 1;",
                                                configure: null,
                                                name: name,
                                                failureStatus: HealthStatus.Unhealthy,
                                                tags: tags,
                                                timeout: TimeSpan.FromMinutes(1));
        return services;
    }

    /// <summary>
    /// Adds redis health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which health checks will be added.</param>
    /// <param name="connectionString">The connection string used to connect to the redis instance.</param>
    /// <param name="name">The name of the health check.</param>
    /// <param name="tags">The tags to apply to the health check.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddRedisHealthChecks(this IServiceCollection services,
                                                          string connectionString,
                                                          string? name,
                                                          IEnumerable<string>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHealthChecks().AddRedis(connectionString,
                                            name,
                                            HealthStatus.Unhealthy,
                                            tags,
                                            TimeSpan.FromMinutes(10));
        return services;
    }

    /// <summary>
    /// Adds mongo health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which health checks will be added.</param>
    /// <param name="connectionString">The connection string to connect to the mongo database.</param>
    /// <param name="tags">The tags to apply to the health check.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddMongoDatabaseHealthChecks(this IServiceCollection services,
                                                                  string connectionString,
                                                                  IEnumerable<string>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
        
        services.AddHealthChecks()
                .AddMongoDb(
                    clientFactory: sp => new MongoClient(connectionString),
                    name: "mongodb",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: tags,
                    timeout: TimeSpan.FromMinutes(1));
            
        return services;
    }

}
