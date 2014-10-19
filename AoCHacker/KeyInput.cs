using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace AoCHacker
{
    class KeyInput
    {
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public UInt32 type;
            //KEYBDINPUT:
            public ushort wVk;
            public ushort wScan;
            public UInt32 dwFlags;
            public UInt32 time;
            public UIntPtr dwExtraInfo;
            //HARDWAREINPUT:
            public UInt32 uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        enum SendInputFlags
        {
            KEYEVENTF_KEYDOWN = 0x0000,
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_UNICODE = 0x0004,
            KEYEVENTF_SCANCODE = 0x0008,
        }

        [DllImport("user32.dll")]
        static extern UInt32 SendInput(UInt32 nInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] pInputs, Int32 cbSize);

        public static void sendK(ushort dxscan)
        {
            INPUT[] InputData = new INPUT[1];

            //KEYBDINPUT:
            InputData[0].type = 1;
            InputData[0].wVk = 0;
            InputData[0].wScan = 0;
            InputData[0].dwFlags = 0;
            InputData[0].time = 0;
            InputData[0].dwExtraInfo = (UIntPtr)0;

            InputData[0].wScan = (ushort)dxscan;
            InputData[0].dwFlags = (uint)SendInputFlags.KEYEVENTF_SCANCODE | (uint)SendInputFlags.KEYEVENTF_KEYDOWN;

            SendInput(1, InputData, Marshal.SizeOf(InputData[0]));

            InputData[0].wScan = (ushort)dxscan;
            InputData[0].dwFlags = (uint)SendInputFlags.KEYEVENTF_SCANCODE | (uint)SendInputFlags.KEYEVENTF_KEYUP;

            SendInput(1, InputData, Marshal.SizeOf(InputData[0]));

        }
    }
}
