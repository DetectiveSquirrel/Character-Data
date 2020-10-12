// Decompiled with JetBrains decompiler
// Type: Typing_Buttons.Utils.Keyboard
// Assembly: Typing Buttons, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F0A1A831-F1A8-4E0C-8D57-E89E4C077FC4
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Z Typing Buttons\Typing Buttons.dll

using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Typing_Buttons.Utils
{
    public static class Keyboard
    {
        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;
        private const int ACTION_DELAY = 1;

        [DllImport("user32.dll")]
        private static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void KeyDown(Keys key)
        {
            var num = (int) keybd_event((byte) key, 0, 1, 0);
        }

        public static void KeyUp(Keys key)
        {
            var num = (int) keybd_event((byte) key, 0, 3, 0);
        }

        public static void KeyPress(Keys key)
        {
            KeyDown(key);
            Thread.Sleep(1);
            KeyUp(key);
        }

        [DllImport("USER32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        public static bool IsKeyDown(int nVirtKey)
        {
            return GetKeyState(nVirtKey) < 0;
        }
    }
}