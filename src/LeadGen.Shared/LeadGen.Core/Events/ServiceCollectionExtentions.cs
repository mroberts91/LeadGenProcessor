using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LeadGen.Core.Events
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            services.AddControllers().AddDapr();
            services.TryAddScoped<DaprClient>();
            services.TryAddTransient<IEventBus, DaprEventBus>();

            return services;
        }
    }
}
