using Serilog;
using Serilog.Core;
using Serilog.Formatting.Display;
using Serilog.Sinks.Grafana.Loki;

namespace SaasFactory.Modules.Core.Logging;

public static class CommonLoggerFactory
{
    
    public static Logger CreateLogger(string moduleName)
    {
        var lokiUrl = new Uri(Environment.GetEnvironmentVariable("LOKI_URL")!);
    
        var textFormatter =
            new MessageTemplateTextFormatter("{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null);

        var logger1 = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(
                lokiUrl.ToString(),
                textFormatter: textFormatter, // ✅ render as plain text
                labels: new[]
                {
                    new LokiLabel { Key = "app", Value = "Saas Factory" },
                    new LokiLabel { Key = "service_name", Value = moduleName },
                    new LokiLabel { Key = "env", Value = "dev" }
                })
            .CreateLogger();
        return logger1;
    }
}