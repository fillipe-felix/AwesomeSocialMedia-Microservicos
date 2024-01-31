using AwesomeSocialMedia.Newsfeed.Application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSocialMedia.Newsfeed.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            
            return services;
        }
        
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGetUserNewsFeedService, GetUserNewsFeedService>();

            return services;
        }
    }
}
