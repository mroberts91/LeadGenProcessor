using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LeadGen.Connectors
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceConnectors(this IServiceCollection services)
        {
            services.AddDaprClient();
            services.TryAddTransient<IValidationConnector, ValidationConnector>();
            return services;
        }
    }
}
