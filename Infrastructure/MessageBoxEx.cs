#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using LcsSaveEditor.Resources;

namespace LcsSaveEditor.Infrastructure
{
    /// <summary>
    /// A more WPF-compatible version of the Win32 <see cref="System.Windows.MessageBox"/>.
    /// </summary>
    /// <remarks>
    /// Unfortunately, the canned MessageBox class cannot be centered on the application window,
    /// so creating this class was a necessary workaround.
    /// Adapted from responses to a question asked at https://stackoverflow.com/q/564710/.
    /// </remarks>
    public class MessageBoxEx
    {
        private static IntPtr _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;

        static MessageBoxEx()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }

        public static MessageBoxResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }

        public static MessageBoxResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }

        public static MessageBoxResult Show(string text, string caption, MessageBoxButton buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }

        public static MessageBoxResult Show(string text, string caption, MessageBoxButton buttons, MessageBoxImage icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public static MessageBoxResult Show(string text, string caption,
            MessageBoxButton buttons, MessageBoxImage icon, MessageBoxResult defResult)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defResult);
        }

        public static MessageBoxResult Show(string text, string caption,
            MessageBoxButton buttons, MessageBoxImage icon, MessageBoxResult defResult, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defResult, options);
        }

        public static MessageBoxResult Show(Window owner, string text)
        {
            if (owner == null) {
                return Show(text);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        public static MessageBoxResult Show(Window owner, string text, string caption)
        {
            if (owner == null) {
                return Show(text, caption);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        public static MessageBoxResult Show(Window owner, string text, string caption, MessageBoxButton buttons)
        {
            if (owner == null) {
                return Show(text, caption, buttons);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static MessageBoxResult Show(Window owner, string text, string caption,
            MessageBoxButton buttons, MessageBoxImage icon)
        {
            if (owner == null) {
                return Show(text, caption, buttons, icon);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static MessageBoxResult Show(Window owner, string text, string caption,
            MessageBoxButton buttons, MessageBoxImage icon, MessageBoxResult defResult)
        {
            if (owner == null) {
                return Show(text, caption, buttons, icon, defResult);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defResult);
        }

        public static MessageBoxResult Show(Window owner, string text, string caption,
            MessageBoxButton buttons, MessageBoxImage icon, MessageBoxResult defResult, MessageBoxOptions options)
        {
            if (owner == null) {
                return Show(text, caption, buttons, icon, defResult, options);
            }

            _owner = new WindowInteropHelper(owner).Handle;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defResult, options);
        }

        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero) {
                string msg = string.Format("{0} ({1}.{2}())",
                    Strings.ExceptionMultipleCallsNotSupported,
                    nameof(MessageBoxEx), nameof(Initialize));
                throw new NotSupportedException();
            }

#pragma warning disable 0618    // GetCurrentThreadId() is marked obsolete, but it still works for what we need it to do.
            if (_owner != null) {
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
#pragma warning restore 0618
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT) Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int) CbtHookAction.HCBT_ACTIVATE) {
                try {
                    CenterWindow(msg.hwnd);
                }
                finally {
                    UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner, ref recParent);

            System.Drawing.Point ptCenter = new System.Drawing.Point(0, 0)
            {
                X = recParent.X + ((recParent.Width - recParent.X) / 2),
                Y = recParent.Y + ((recParent.Height - recParent.Y) / 2)
            };

            System.Drawing.Point ptStart = new System.Drawing.Point(0, 0)
            {
                X = ptCenter.X - (width / 2),
                Y = ptCenter.Y - (height / 2)
            };

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width, height, false);
        }


        /* ===== Win32 imports ===== */

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void TimerProc(IntPtr hWind, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        public const int WH_CALLWNDPROCRET = 12;

        public enum CbtHookAction : int
        {
            HCBT_MOVESIZE,
            HCBT_MINMAX,
            HCBT_QS,
            HCBT_CREATEWND,
            HCBT_DESTROYWND,
            HCBT_ACTIVATE,
            HCBT_CLICKSKIPPED,
            HCBT_KEYSKIPPED,
            HCBT_SYSCOMMAND,
            HCBT_SETFOCUS,
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };
    }
}
