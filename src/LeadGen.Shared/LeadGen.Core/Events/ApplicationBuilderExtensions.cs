using Microsoft.AspNetCore.Builder;

namespace LeadGen.Core.Events
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSubscriptionEvents(this IApplicationBuilder app)
        {
            app.UseCloudEvents();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
