using KeyLoggerClone.Core;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Abstract contract for logging systems.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a single keyboard event.
        /// </summary>
        void Log(KeyInterceptedEventArgs e);

        /// <summary>
        /// Flushes any buffered data to the output.
        /// </summary>
        void Flush();
    }
}
