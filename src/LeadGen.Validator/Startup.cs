using LeadGen.Core.Endpoints;
using LeadGen.Validator.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using LeadGen.Core.Events;
using LeadGen.Core.Store;
using LeadGen.Core.Middleware;
using System.Text.Json;
using LeadGen.Core.Models;

namespace LeadGen.Validator
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddEventBus();
            services.AddStateStore();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LeadGen.Validator", Version = "v1" });
            });

            services.AddHealthChecks()
                .AddCheck<LiveHealthCheck>(
                    "live-health-check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "live" })
                .AddCheck<ReadyHealthCheck>(
                    "ready-health-check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready" });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                var headers = context.Request.Headers;
                await next(context);
            });
            app.UseServiceExceptionHandler("/");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LeadGen.Validator v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseSubscriptionEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/validatetest", async context =>
                {
                    var foo = await JsonSerializer.DeserializeAsync<Lead>(context.Request.Body);
                    System.Console.WriteLine();
                    
                });

                endpoints.MapControllers();
                endpoints.MapConfigDebugEndpoint();

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