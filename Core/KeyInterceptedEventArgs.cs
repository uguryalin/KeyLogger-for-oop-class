using System;

namespace KeyLoggerClone.Core
{
    /// <summary>
    /// Event arguments containing detailed information about the intercepted key event.
    /// </summary>
    public class KeyInterceptedEventArgs : EventArgs
    {
        public uint VkCode { get; }
        public uint ScanCode { get; }
        public string KeyName { get; }
        public string ActiveWindow { get; }
        public DateTime Timestamp { get; }
        
        // Modifier states
        public bool IsShiftPressed { get; }
        public bool IsControlPressed { get; }
        public bool IsAltPressed { get; }
        public bool IsCapsLockOn { get; }

        public KeyInterceptedEventArgs(
            uint vkCode, 
            uint scanCode, 
            string keyName, 
            string activeWindow,
            bool isShift, 
            bool isControl, 
            bool isAlt,
            bool isCapsLock)
        {
            VkCode = vkCode;
            ScanCode = scanCode;
            KeyName = keyName;
            ActiveWindow = activeWindow;
            Timestamp = DateTime.Now;
            IsShiftPressed = isShift;
            IsControlPressed = isControl;
            IsAltPressed = isAlt;
            IsCapsLockOn = isCapsLock;
        }
    }
}
