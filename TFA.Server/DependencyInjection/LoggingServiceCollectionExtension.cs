using Serilog;
using Serilog.Filters;

namespace TFA.Server.DependencyInjection
{
    public static class LoggingServiceCollectionExtension
    {
        public static IServiceCollection AddApiLogging(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddLogging(x => x.AddSerilog(new LoggerConfiguration()
                // level
                .MinimumLevel.Debug()
                // properties
                .Enrich.WithProperty("Application", "TFA")
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                // for open search
                .WriteTo.Logger(lc =>
                    lc.Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.OpenSearch(configuration.GetConnectionString("Logs"), "forum-logs-{0:yyyy.MM.dd}"))
                // for console
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()
                ));

            return services;
        }
    }
}
