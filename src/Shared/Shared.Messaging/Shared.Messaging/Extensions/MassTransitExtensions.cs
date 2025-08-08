using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitExtensions(this IServiceCollection services, params Assembly[] assemblies)
        {
            return services;
        }
    }
}
