using LeadGen.Core.Events.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LeadGen.Core.Endpoints
{
    public static class EndpointBuilderExtensions
    {
        public static IEndpointRouteBuilder MapConfigDebugEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/config", async context =>
            {
                var config = context.RequestServices.GetService<IConfiguration>();
                if (config is IConfigurationRoot cr)
                    await context.Response.WriteAsync(cr.GetDebugView());
            });

            return endpoints;
        }
    }
}
