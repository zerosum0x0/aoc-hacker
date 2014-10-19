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
    public class NativeWIN32
    {
        const uint INPUT_KEYBOARD = 1;
        const int KEY_EXTENDED = 0x0001;
        const uint KEY_UP = 0x0002;
        const uint KEY_SCANCODE = 0x0004;
        //const uint KEY_SCANCODE = 0x0008;


        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBOARD_INPUT
        {
            public uint type;
            public ushort vk;
            public ushort scanCode;
            public uint flags;
            public uint time;
            public uint extrainfo;
            public uint padding1;
            public uint padding2;
        }

        [DllImport("User32.dll")]
        private static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] KEYBOARD_INPUT[] input, int structSize);

        void press(int scanCode)
        {
            sendKey(scanCode, true);
        }

        void release(int scanCode)
        {
            sendKey(scanCode, false);
        }

        public void sendKey(int scanCode, bool press)
        {
            KEYBOARD_INPUT[] input = new KEYBOARD_INPUT[1];
            input[0] = new KEYBOARD_INPUT();
            input[0].type = INPUT_KEYBOARD;
            input[0].flags = 0x0008;/* KEY_SCANCODE;*/

            if ((scanCode & 0xFF00) == 0xE000)
            { // extended key?
                input[0].flags |= KEY_EXTENDED;
            }

            if (press)
            { // press?
                input[0].scanCode = (ushort)(scanCode /*& 0xFF*/);
            }
            else
            { // release?
                input[0].scanCode = (ushort)scanCode;
                input[0].flags |= KEY_UP;
            }

            uint result = SendInput(1, input, Marshal.SizeOf(input[0]));

            if (result != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }
    }
    /*{
        public const ushort KEYEVENTF_KEYUP = 0x0002;

        public enum VK : ushort
        {
            SHIFT = 0x10,
            CONTROL = 0x11,
            MENU = 0x12,
            ESCAPE = 0x1B,
            BACK = 0x08,
            TAB = 0x09,
            RETURN = 0x0D,
            PRIOR = 0x21,
            NEXT = 0x22,
            END = 0x23,
            HOME = 0x24,
            LEFT = 0x25,
            UP = 0x26,
            RIGHT = 0x27,
            DOWN = 0x28,
            SELECT = 0x29,
            PRINT = 0x2A,
            EXECUTE = 0x2B,
            SNAPSHOT = 0x2C,
            INSERT = 0x2D,
            DELETE = 0x2E,
            HELP = 0x2F,
            NUMPAD0 = 0x60,
            NUMPAD1 = 0x61,
            NUMPAD2 = 0x62,
            NUMPAD3 = 0x63,
            NUMPAD4 = 0x64,
            NUMPAD5 = 0x65,
            NUMPAD6 = 0x66,
            NUMPAD7 = 0x67,
            NUMPAD8 = 0x68,
            NUMPAD9 = 0x69,
            MULTIPLY = 0x6A,
            ADD = 0x6B,
            SEPARATOR = 0x6C,
            SUBTRACT = 0x6D,
            DECIMAL = 0x6E,
            DIVIDE = 0x6F,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            OEM_1 = 0xBA,   // ',:' for US
            OEM_PLUS = 0xBB,   // '+' any country
            OEM_COMMA = 0xBC,   // ',' any country
            OEM_MINUS = 0xBD,   // '-' any country
            OEM_PERIOD = 0xBE,   // '.' any country
            OEM_2 = 0xBF,   // '/?' for US
            OEM_3 = 0xC0,   // '`~' for US
            MEDIA_NEXT_TRACK = 0xB0,
            MEDIA_PREV_TRACK = 0xB1,
            MEDIA_STOP = 0xB2,
            MEDIA_PLAY_PAUSE = 0xB3,
            LWIN = 0x5B,
            RWIN = 0x5C
        }


        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public long time;
            public uint dwExtraInfo;
        };

        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public uint type;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
        };


        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

    }*/
}
