using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace LeadGen.Core.Store
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateStore(this IServiceCollection services)
        {
            services.AddOptions<StateStoreOptions>()
                .BindConfiguration("StateStore");

            services.AddDaprClient();
            services.TryAddTransient<IStateStore, StateStore>();

            return services;
        }
    }
}
