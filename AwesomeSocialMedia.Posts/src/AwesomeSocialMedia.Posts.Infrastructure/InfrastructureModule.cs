using AwesomeSocialMedia.Posts.Core.Repositories;
using AwesomeSocialMedia.Posts.Infrastructure.EventBus;
using AwesomeSocialMedia.Posts.Infrastructure.Integration.Services;
using AwesomeSocialMedia.Posts.Infrastructure.Persistence.Repositories;

using Consul;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AwesomeSocialMedia.Posts.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddRepositories()
            .AddEventBus()
            .AddIntegrationServices()
            .AddConsul(configuration);

        services.AddHttpClient<IUserIntegrationService, UserIntegrationService>();
        
        return services;
    }
    
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
    {
        builder.UseConsul();

        return builder;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IPostRepository, PostRepository>();

        return services;
    }

    private static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, RabbitMqService>();

        return services;
    }
    
    private static IApplicationBuilder UseConsul(this IApplicationBuilder builder)
    {
        var consulClient = builder.ApplicationServices.GetRequiredService<IConsulClient>();

        var logger = builder.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("Consul");

        var lifeTime = builder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        var serviceId = Guid.NewGuid();

        // Para pegar a url da api e a porta de forma dinamica deve usar dessa forma abaixo.
        // lifeTime.ApplicationStarted.Register(() =>
        // {
        //     var server = builder.ApplicationServices.GetRequiredService<IServer>();
        //     var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
        //
        //     var address = addresses.ElementAt(1);
        //
        //     var sepSchemeFromUrl = address.Split("//");
        //     var addressAndPort = sepSchemeFromUrl[1].Split(":");
        //     var addressPart = addressAndPort[0];
        //     var portPart = addressAndPort[1];
        //     
        //     var registration = new AgentServiceRegistration
        //     {
        //         ID = $"posts-{serviceId}",
        //         Name = "Posts",
        //         Address = addressPart,
        //         Port = int.Parse(portPart)
        //     };
        // });

        var registration = new AgentServiceRegistration
        {
            ID = $"posts-{serviceId}",
            Name = "Posts",
            Address = "localhost",
            Port = 5144
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

    private static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserIntegrationService, UserIntegrationService>();

        return services;
    }
}
