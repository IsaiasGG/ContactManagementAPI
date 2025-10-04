using Serilog.Core;
using Serilog.Events;
using System.Reflection;

namespace IF.ContactManagement.Presentation.WebAPI.LogProvider
{
    public class SerilogThreadIdEnricher : ILogEventEnricher
    {
        private readonly string version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ThreadId",
                Environment.CurrentManagedThreadId.ToString().PadLeft(3, '0')));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("severity", this.LevelToSeverity(logEvent.Level)));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("message", logEvent.RenderMessage()));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("version", this.version));
            logEvent.RemovePropertyIfPresent("ActionId");
            logEvent.RemovePropertyIfPresent("ActionName");
            logEvent.RemovePropertyIfPresent("RequestId");
            logEvent.RemovePropertyIfPresent("RequestPath");
            logEvent.RemovePropertyIfPresent("ConnectionId");
            logEvent.RemovePropertyIfPresent("CorrelationId");
        }


        private string LevelToSeverity(LogEventLevel level)
        {
            return level switch
            {
                LogEventLevel.Verbose => "DEFAULT",
                LogEventLevel.Debug => "DEBUG",
                LogEventLevel.Information => "INFO",
                LogEventLevel.Warning => "WARNING",
                LogEventLevel.Error => "ERROR",
                LogEventLevel.Fatal => "CRITICAL",
                _ => "DEFAULT",
            };
        }
    }
}
