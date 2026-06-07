using System;
using KeyLoggerClone.Core;

namespace KeyLoggerClone.Listeners
{
    /// <summary>
    /// Abstract contract for keyboard listening services.
    /// </summary>
    public interface IKeyboardListener : IDisposable
    {
        /// <summary>
        /// Triggered when a keyboard key is intercepted.
        /// </summary>
        event EventHandler<KeyInterceptedEventArgs> KeyIntercepted;

        /// <summary>
        /// Starts intercepting keyboard inputs.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops intercepting keyboard inputs.
        /// </summary>
        void Stop();

        /// <summary>
        /// Indicates whether the keyboard listener is currently running.
        /// </summary>
        bool IsRunning { get; }
    }
}
