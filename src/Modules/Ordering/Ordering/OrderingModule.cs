using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering
{
    public static class OrderingModule
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services, repositories, etc. for the Ordering module here
            // Example: services.AddScoped<IOrderingService, OrderingService>();
            return services;
        }


        public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
