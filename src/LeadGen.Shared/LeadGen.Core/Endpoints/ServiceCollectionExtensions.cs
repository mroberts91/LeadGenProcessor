using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LeadGen.Core.Endpoints
{
    public static class ServiceCollectionExtensions
    {
        //private static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
        //{
        //    var eventTypes = GetAllTypesImplementingOpenGenericType(typeof(ISubscriptionHandler<>), assembly);
        //    IEnumerable<(Type baseType, Type implements)> mappings = eventTypes.Select(t => (t, t.GetInterfaces().First(i => i.Name.Equals("ISubscriptionHandler`1"))));

        //    foreach(var (baseType, implements) in mappings)
        //    {
        //        services.AddTransient(implements, baseType);
        //    }

        //    return services;
        //}

        private static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, Assembly assembly)
        {
            return from x in assembly.GetTypes()
                   from z in x.GetInterfaces()
                   let y = x.BaseType
                   where
                   (y != null && y.IsGenericType &&
                   openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                   (z.IsGenericType &&
                   openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                   select x;
        }
    }
}
