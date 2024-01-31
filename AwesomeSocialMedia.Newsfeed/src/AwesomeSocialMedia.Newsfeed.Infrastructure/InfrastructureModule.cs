using AwesomeSocialMedia.Newsfeed.Core.Core.Repositories;
using AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence;
using AwesomeSocialMedia.Newsfeed.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSocialMedia.Newsfeed.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AwesomeSocialMediaCs");

        services
            .AddDb(connectionString)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<NewsFeedDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
    

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserNewsfeedRepository, UserNewsfeedRepository>();

        return services;
    }
}
