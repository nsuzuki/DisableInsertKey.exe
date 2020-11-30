using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DisableInsertKey
{
    public class DataProperty
    {
        // 実装は外部から隠蔽(privateにしておく)
        private static bool InsertKeyStatus; // staticで変数を宣言

        // 変数の取得・変更用のプロパティ
        public bool IsDisabled
        {
            set { InsertKeyStatus = value; }
            get { return InsertKeyStatus; }
        }
    }


    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        //DisableInsertKey.Properties.Settings.
        //public isDisabled = DisableInsertKey.Properties.

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        public static void Main()
        {

            DataProperty p = new DataProperty();
            p.IsDisabled = true;
            //Console.WriteLine(p.IsDisabled);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // フォームの表示を抑制するためコメントアウト
            //Application.Run(new Form1());

            // フォームを表示せずに実行する
            new Form1();

            _hookID = SetHook(_proc);

            Application.Run();

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

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);


        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {

                DataProperty p = new DataProperty();

                int vkCode = Marshal.ReadInt32(lParam);
                // デバッグ用
                // Console.WriteLine((Keys)vkCode);
                // Console.WriteLine(p.IsDisabled);

                // Insertキーが押されたときは入力を破棄
                if (vkCode == 45 && p.IsDisabled) return new IntPtr(1);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

}
