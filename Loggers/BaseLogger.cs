using KeyLoggerClone.Core;
using KeyLoggerClone.Formatters;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Abstract class providing formatter integration and basic thread-safety lock mechanisms.
    /// Uses the Template Method Pattern.
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        protected readonly IKeyFormatter Formatter;
        protected readonly object LockObj = new object();

        protected BaseLogger(IKeyFormatter formatter)
        {
            Formatter = formatter;
        }

        public virtual void Log(KeyInterceptedEventArgs e)
        {
            string formatted = Formatter.Format(e);
            if (!string.IsNullOrEmpty(formatted))
            {
                lock (LockObj)
                {
                    Write(formatted);
                }
            }
        }

        /// <summary>
        /// Concrete write mechanism to be implemented by child classes.
        /// </summary>
        protected abstract void Write(string formattedKey);

        /// <summary>
        /// Flushes buffers if applicable.
        /// </summary>
        public abstract void Flush();
    }
}
