using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Threading;

namespace Controller
{
    class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            var logLevel = LogEventLevel.Information;
#if DEBUG
            logLevel = LogEventLevel.Verbose;
#endif
            if (args.Contains("--verbose"))
            {
                logLevel = LogEventLevel.Verbose;
            }
            else if (args.Contains("--errors-only"))
            {
                logLevel = LogEventLevel.Error;
            }

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Is(logLevel)
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console(theme: AnsiConsoleTheme.Code)
               .CreateLogger();

            Log.Verbose("Reading config.json");

            Configuration.LoadConfig();

            Log.Information("Starting the Controller Server");

            Log.Information("Intializing Dis");

            while (true) Thread.Sleep(10000);
        }
    }
}
