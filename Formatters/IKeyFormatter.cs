using KeyLoggerClone.Core;

namespace KeyLoggerClone.Formatters
{
    /// <summary>
    /// Abstract contract for formatting keystroke details.
    /// </summary>
    public interface IKeyFormatter
    {
        /// <summary>
        /// Formats intercepted key arguments into a representable string.
        /// </summary>
        string Format(KeyInterceptedEventArgs e);
    }
}
