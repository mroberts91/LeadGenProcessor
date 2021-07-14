using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using LeadGen.Processor.HealthChecks;
using Dapr;
using Dapr.Client;
using LeadGen.Core.Endpoints;
using System.Reflection;
using LeadGen.Core.Events;
using LeadGen.Core.Events.Leads;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LeadGen.Processor.Queues;
using LeadGen.Processor.Workers;
using LeadGen.Core.Store;

namespace LeadGen.Processor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddStateStore();
            services.AddEventBus();

            services.AddHealthChecks()
                .AddCheck<LiveHealthCheck>(
                    "live-health-check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "live" })
                .AddCheck<ReadyHealthCheck>(
                    "ready-health-check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready" });

            services.AddSingleton<ILeadQueue, LeadProcessingQueue>();

            services.AddHostedService<LeadProcessorWorker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSubscriptionEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready")
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("live")
                });
            });
        }
    }
}