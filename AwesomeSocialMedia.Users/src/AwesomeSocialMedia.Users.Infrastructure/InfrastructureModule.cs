using AwesomeSocialMedia.Users.Core.Repositories;
using AwesomeSocialMedia.Users.Infrastructure.EventBus;
using AwesomeSocialMedia.Users.Infrastructure.Persistence;
using AwesomeSocialMedia.Users.Infrastructure.Persistence.Repositories;

using Consul;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AwesomeSocialMedia.Users.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AwesomeSocialMediaCs");

        services
            .AddEventBus()
            .AddDb(connectionString)
            .AddRepositories()
            .AddConsul(configuration);

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        builder.UseConsul();

        return builder;
    }
    
    private static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, RabbitMqService>();

        return services;
    }
    
    private static IServiceCollection AddDb(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
    
    private static IApplicationBuilder UseConsul(this IApplicationBuilder builder)
    {
        var consulClient = builder.ApplicationServices.GetRequiredService<IConsulClient>();

        var logger = builder.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Consul");

        var lifeTime = builder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        var serviceId = Guid.NewGuid();

        var registration = new AgentServiceRegistration
        {
            ID = $"users-{serviceId}",
            Name = "Users",
            Address = "localhost",
            Port = 5109
        };

        logger.LogInformation("Registrando com o Consul.");
        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        lifeTime.ApplicationStopped.Register(() =>
        {
            logger.LogInformation("Desregistrando do Consul.");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        });

        return builder;
    }

    private static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p =>
            new ConsulClient(config =>
            {
                var address = configuration.GetSection("Consul:Host");
                config.Address = new Uri(address.Value);
            }));
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
