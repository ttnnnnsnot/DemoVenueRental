using Serilog;

namespace DemoVenueRental.Global
{
    public static class LoggerService
    {
        private static readonly Serilog.ILogger _logger = Log.Logger;

        public static void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public static void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public static void LogError(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }
    }
}
