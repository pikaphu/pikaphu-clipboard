using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikaPhuClipboard
{
    static class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            _hookID = SetHook(_proc);

            //uiForm = new Form1();
            Application.Run(new Form1());  //Application.Run();

            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        static Keys key1, key2;

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                Keys inputKey = (Keys)vkCode;

                Console.WriteLine(inputKey.ToString());

                DetectComboKey(inputKey);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        static void DetectComboKey(Keys curKey)
        {
            
            if (curKey == Keys.LControlKey)
            {
                key1 = curKey;
                key2 = Keys.None;
            }

            if (curKey != key1)
            {
                key2 = curKey;
            }

            if (key1 != key2 && key2 != Keys.None)
            {
                // combo keys
                if (key1 == Keys.LControlKey)
                {
                    if (key2 == Keys.C)
                    {
                        Console.WriteLine("Copy!");                        
                        ClearComboKey();

                        UpdateUI();
                    }
                    else if (key2 == Keys.V)
                    {
                        Console.WriteLine("Paste!");
                        ClearComboKey();

                        ClearClip();
                    }
                }
            }
        }

        static void UpdateUI()
        {
            Form1.thisForm.UpdateUI();
        }
        static void ClearClip()
        {
            Form1.thisForm.ClearClip();
        }

        static void ClearComboKey()
        {
            key1 = Keys.None;
            key2 = Keys.None;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
