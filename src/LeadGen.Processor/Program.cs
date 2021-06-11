using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace LeadGen.Processor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string instrumentationKey = config["ApplicationInsights:InstrumentationKey"];

            CreateHostBuilder(args, instrumentationKey).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string instrumentationKey) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(
                        builder =>
                        {
                            builder.AddApplicationInsights(instrumentationKey);
                            builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                            builder.AddConsole();
                        });
                });
    }
}