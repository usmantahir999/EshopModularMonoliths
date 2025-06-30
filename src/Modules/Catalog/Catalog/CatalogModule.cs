

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services,IConfiguration configuration)
        {
            // Register services, repositories, etc. for the Catalog module here
            // Example: services.AddScoped<ICatalogService, CatalogService>();
            //Data - infrastructure services
            var connectionString = configuration.GetConnectionString("Database");
            services.AddDbContext<CatalogDbContext>((sp, options) =>
            {
                options.AddInterceptors(new AuditableEntityInterceptor());
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IDataSeeder, CatalogDataSeeder>();
            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            app.UseMigration<CatalogDbContext>();
            return app;
        }

        
    }
}
