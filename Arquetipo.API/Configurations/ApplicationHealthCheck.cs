using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Arquetipo.API.Configurations
{
    public static class ApplicationHealthCheck
    {
        public static IApplicationBuilder AddApplicationHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        Duration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            return app;
        }
        private class HealthCheck
        {
            public string Status { get; set; }
            public string Component { get; set; }
            public string Description { get; set; }
        }
        private class HealthCheckResponse
        {
            public string Status { get; set; }
            public IEnumerable<HealthCheck> Checks { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}
