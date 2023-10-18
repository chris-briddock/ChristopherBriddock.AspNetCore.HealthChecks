using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSqlDatabaseHealthChecks(this IServiceCollection services, string connectionString)
    {
        // Ensure that the services parameter is not null
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        // Add a SQL Server health check to the IServiceCollection
        services.AddHealthChecks().AddSqlServer(connectionString: connectionString,
                                                healthQuery: "SELECT 1;",
                                                configure: null,
                                                name: null,
                                                failureStatus: HealthStatus.Unhealthy,
                                                tags: null,
                                                timeout: TimeSpan.FromMinutes(1));
        return services;
    }

    /// <summary>
    /// Adds mongo health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which health checks will be added.</param>
    /// <param name="elasticSearchUri">The connection string used to connect to the mongo database</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns> 
    public static IServiceCollection AddMongoDatabaseHealthChecks(this IServiceCollection services, string connectionString) 
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHealthChecks().AddMongoDb(connectionString,
                                              null,
                                              HealthStatus.Unhealthy,
                                              null,
                                              TimeSpan.FromMinutes(10));

        return services;
    }
    /// <summary>
    /// Adds database health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which health checks will be added.</param>
    /// <param name="elasticSearchUri">The URI used to connect to ElasticSearch</param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>    
    public static IServiceCollection AddElasticSearchHealthChecks(this IServiceCollection services,
                                                                  string elasticSearchUri) 
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHealthChecks()
                .AddElasticsearch(elasticSearchUri,
                                  null,
                                  HealthStatus.Unhealthy,
                                  null,
                                  TimeSpan.FromMinutes(10));
        
        return services;
    }

    /// <summary>
    /// Adds redis health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddRedisHealthChecks(this IServiceCollection services, string connectionString ) 
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHealthChecks().AddRedis(connectionString, null, HealthStatus.Unhealthy, null, TimeSpan.FromMinutes(10));
        return services;
    }

    /// <summary>
    /// Adds RabbitMQ health checks to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddRabbitMQHealthChecks(this IServiceCollection services) 
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHealthChecks().AddRabbitMQ(null,
                                               HealthStatus.Unhealthy,
                                               null,
                                               TimeSpan.FromMinutes(10));
        return services;
    }

}
