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
    public class SendInputClass
    {

        //C# signature for "SendInput()"
        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        static extern uint SendInput(
            uint nInputs,
            INPUT[] pInputs,
            int cbSize);

        //C# signature for "GetMessageExtraInfo()"
        [DllImport("user32.dll", EntryPoint = "GetMessageExtraInfo", SetLastError = true)]
        static extern IntPtr GetMessageExtraInfo();

        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }

        [Flags()]
        private enum MOUSEEVENTF
        {
            MOVE = 0x0001,  // mouse move 
            LEFTDOWN = 0x0002,  // left button down
            LEFTUP = 0x0004,  // left button up
            RIGHTDOWN = 0x0008,  // right button down
            RIGHTUP = 0x0010,  // right button up
            MIDDLEDOWN = 0x0020,  // middle button down
            MIDDLEUP = 0x0040,  // middle button up
            XDOWN = 0x0080,  // x button down 
            XUP = 0x0100,  // x button down
            WHEEL = 0x0800,  // wheel button rolled
            VIRTUALDESK = 0x4000,  // map to entire virtual desktop
            ABSOLUTE = 0x8000,  // absolute move
        }

        [Flags()]
        private enum KEYEVENTF
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        }


        // This function simulates a simple mouseclick at the current cursor position.
        public static uint Click(int x, int y)
        {
            INPUT input_move = new INPUT();
            input_move.mi.dx = (int)Math.Round((double)(x * (65535 / Screen.PrimaryScreen.Bounds.Width)), 0);
            input_move.mi.dy = (int)Math.Round((double)(y * (65535 / Screen.PrimaryScreen.Bounds.Height)), 0);
            input_move.mi.mouseData = 0;
            input_move.mi.dwFlags = (int)MOUSEEVENTF.MOVE | (int)MOUSEEVENTF.ABSOLUTE;

            INPUT input_down = input_move;
            input_down.mi.dwFlags = (int)MOUSEEVENTF.LEFTDOWN;

            INPUT input_up = input_down;
            input_up.mi.dwFlags = (int)MOUSEEVENTF.LEFTUP;

            INPUT[] input = { input_move, input_down, input_up };

            return SendInput(3, input, Marshal.SizeOf(input_down));
        }

        public static uint sendKey(ushort scanCode)
        {
            INPUT input_key = new INPUT();
            input_key.type = 1; //Set it to Keyboard type
            input_key.ki.wScan = (short)0x02;
            input_key.ki.time = 0;
            input_key.ki.dwFlags = (int)KEYEVENTF.SCANCODE;

            INPUT[] input = { input_key };

            return SendInput(1, input, Marshal.SizeOf(input_key));
        }
    }   

}
