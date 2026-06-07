using System;
using KeyLoggerClone.Formatters;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Raises a C# event whenever a key is processed, allowing components (like the UI) 
    /// to listen and react to formatted logging output in real-time.
    /// </summary>
    public class EventLogger : BaseLogger
    {
        /// <summary>
        /// Triggered when a formatted key is logged.
        /// </summary>
        public event Action<string> KeyLogged;

        public EventLogger(IKeyFormatter formatter) : base(formatter)
        {
        }

        protected override void Write(string formattedKey)
        {
            // Propagate the formatted text via events
            KeyLogged?.Invoke(formattedKey);
        }

        public override void Flush()
        {
            // Event loggers do not buffer, so flush is a no-op
        }
    }
}
