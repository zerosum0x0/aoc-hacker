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

/// Simple Form to allow memory reads and writes to Age of Conan player values.
/// Includes runspeed and Z-Axis modification, along with a no fall damage option.
/// Keyboard hook is taken from an update to an MSDN article by Stephen Troub (http://blogs.msdn.com/toub/archive/2006/05/03/589423.aspx) 
/// The ProcessMemoryReader is largely based on an article that describes altering process memory for freecell (http://www.codeproject.com/KB/trace/freecellreader.aspx).
/// Offsets were found using a memory editor, with hints on good search values found on Google.

namespace AoCHacker
{
    public partial class Form1 : Form
    {
        static IntPtr StaticAddress = (IntPtr)0x013B49B4; //Ptr to static address for player values
        static int runspeedOffset = 0x150;
        static int zAxisOffset = 0x30;
        static int noFallOffset = 0x184;
        static int noGravOffset = 0x48;
        static int gravDMA = 0x0062eb0c;
        static int jumpDMA = 0x0062cd93;
        static IntPtr playerDMA = GetDMA(); //Start of player values

        static bool comboState = false;
        static bool over40 = false;
        static bool gravBool = false;
        //static Thread oThread = new Thread(new ThreadStart(autoCombo));

        //These are needed to perform a low-level keyboard hook
        private const int WH_KEYBOARD_LL = 13; 
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        
        public Form1()
        {
            _hookID = SetHook(_proc);
            InitializeComponent();//Note that the keyboard hook is released in the Dispose method
        }

        private void GetSpeedButton_Click(object sender, EventArgs e)
        {
            //[plo] Removed the threading and constant updates. Not really necessary
            SpeedTextBox.Text = getRunspeed().ToString();
        }

        private void MaxRunButton_Click(object sender, EventArgs e)
        {
            setRunSpeed(11);
        }

        private void NormalRunButton_Click(object sender, EventArgs e)
        {
            setRunSpeed(5);
        }

        private void maxStealthButton_Click(object sender, EventArgs e)
        {
            setRunSpeed(7);
        }


        private static float getRunspeed()
        {
            playerDMA = GetDMA();
            ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
            pReader.OpenProcess();

            int readBytes;
            byte[] buffer;

            buffer = pReader.ReadProcessMemory((IntPtr)((int)playerDMA + runspeedOffset), 4, out readBytes);
            pReader.CloseHandle();
            return (BitConverter.ToSingle(buffer, 0));
        }

        private static void setRunSpeed(float runSpeed)
        {
            playerDMA = GetDMA();
            ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
            pReader.OpenProcess();

            int writtenBytes;
            byte[] buffer = BitConverter.GetBytes(runSpeed);
            pReader.WriteProcessMemory((IntPtr)((int)playerDMA + runspeedOffset), buffer, out writtenBytes);
            pReader.CloseHandle();
        }

        private static void setZAxis(float zAxisDiff)
        {
            ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
            pReader.OpenProcess();

            int writtenBytes, readBytes;

            byte[] axisBuffer = pReader.ReadProcessMemory((IntPtr)((int)playerDMA + zAxisOffset), 4, out readBytes);
            float zAxis = BitConverter.ToSingle(axisBuffer, 0);
            byte[] buffer = BitConverter.GetBytes(zAxis + zAxisDiff);
            pReader.WriteProcessMemory((IntPtr)((int)playerDMA + zAxisOffset), buffer, out writtenBytes);
            pReader.CloseHandle();
        }

        private static IntPtr GetDMA()
        {
            ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
            pReader.OpenProcess();

            int readBytes;
            byte[] buffer;

            // Get the memory location pointed to by the static address
            buffer = pReader.ReadProcessMemory(StaticAddress, 4, out readBytes);
            IntPtr DMAAddress = (IntPtr)(
                buffer[0] +
                256 * buffer[1] +
                256 * 256 * buffer[2] +
                256 * 256 * 256 * buffer[3]);
            pReader.CloseHandle();
            return (DMAAddress);

        }

        private static ProcessMemoryReaderLib.ProcessMemoryReader GetAoCProcess()
        {
            ProcessMemoryReaderLib.ProcessMemoryReader procReader
                   = new ProcessMemoryReaderLib.ProcessMemoryReader();

            System.Diagnostics.Process[] myProcesses
                           = System.Diagnostics.Process.GetProcessesByName("ageofconan");

            // take first instance of AoC we find

            if (myProcesses.Length == 0)
            {
                MessageBox.Show("Age of Conan not found");
            }
            procReader.ReadProcess = myProcesses[0];
            return (procReader);

        }


        private void zAxisPlusButton_Click(object sender, EventArgs e)
        {
            setZAxis(10.0f);
        }

        private void zAxisDown_Click(object sender, EventArgs e)
        {
            setZAxis(-10.0f);
        }

        private void noFallDMGCheck_CheckedChanged(object sender, EventArgs e)
        {
            noFallToggle(noFallDMGCheck.Checked);
        }

        private static void noFallToggle(bool isChecked)
        {
            ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
            pReader.OpenProcess();

            int writtenBytes;
            byte[] buffer = new byte[4];
            if (isChecked)
                buffer = BitConverter.GetBytes(0);
            else
                buffer = BitConverter.GetBytes(1);
            pReader.WriteProcessMemory((IntPtr)((int)playerDMA + noFallOffset), buffer, out writtenBytes);
            pReader.CloseHandle();
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
                int vkCode = Marshal.ReadInt32(lParam);
                if (Keys.PageUp == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    setZAxis(1.0f);
                }
                if (Keys.PageDown == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    setZAxis(-1.0f);
                }
                if (Keys.Home == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    setRunSpeed(getRunspeed() + 1.0f);
                }
                if (Keys.Insert == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    setRunSpeed(getRunspeed() - 1.0f);
                }
                if (Keys.End == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    noFallToggle(true);
                }
                if (Keys.NumLock == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    comboState = !comboState;
                    Color pixelColorlvl = getPixel(723, 917);
                    if ((pixelColorlvl.G > 10 && pixelColorlvl.G < 25) && (pixelColorlvl.R > 10 && pixelColorlvl.R < 25) && (pixelColorlvl.B > 10 && pixelColorlvl.B < 25))
                        over40 = true;

                    Thread m_NewThread = new Thread(new ThreadStart(autoCombo));

                    m_NewThread.IsBackground = true; // this prevents the extra thread from blocking an application shutdown

                    m_NewThread.Start();
                }
                if (Keys.Delete == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {
                    disableGravity();
                }
                    
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static void disableGravity()
        {
            if (gravBool)
            {
                playerDMA = GetDMA();
                ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
                pReader.OpenProcess();

                int writtenBytes;
                byte[] gravBuffer = { 0xD9, 0x5E, 0x48, 0x83}; // write back proper instructions
                byte[] jumpBuffer = { 0x0f, 0x13, 0x46, 0x48 }; // Jump power instructions           
                pReader.WriteProcessMemory((IntPtr)((int)gravDMA), gravBuffer, out writtenBytes);
                pReader.WriteProcessMemory((IntPtr)((int)jumpDMA), jumpBuffer, out writtenBytes);
                pReader.CloseHandle();
            }
            else
            {
                playerDMA = GetDMA();
                ProcessMemoryReaderLib.ProcessMemoryReader pReader = GetAoCProcess();
                pReader.OpenProcess();

                int writtenBytes;
                byte[] gravBuffer = { 0x90, 0x90, 0x90, 0x83 }; // nop = 0x90
                byte[] jumpBuffer = { 0x90, 0x90, 0x90, 0x90 };
                byte[] fallBuffer = new byte[4];
                fallBuffer = BitConverter.GetBytes(0); //Set to 0 for true zero grav. 
                pReader.WriteProcessMemory((IntPtr)((int)gravDMA), gravBuffer, out writtenBytes);
                pReader.WriteProcessMemory((IntPtr)((int)jumpDMA), jumpBuffer, out writtenBytes);
                pReader.WriteProcessMemory((IntPtr)((int)playerDMA + noGravOffset), fallBuffer, out writtenBytes);
                pReader.CloseHandle();
            }
            gravBool = !gravBool;
        }

        private static void autoCombo()
        {
            //Determine if this is for a char over 40 with the extra attack buttons by looking for the extra border            
            if (over40)
            {
                while (comboState)
                {
                    Color pixelColorBQ = getPixel(605, 967);//79,5,3                
                    Color pixelColorBE = getPixel(713, 967);//89,5,3
                    Color pixelColorB1 = getPixel(605, 917);//77,5,3 -> 112, 38, 15
                    Color pixelColorB2 = getPixel(660, 917);//87,5,3 -> 139,69,22
                    Color pixelColorB3 = getPixel(713, 917);//84,5,3

                    if ((pixelColorBQ.G > 30 && pixelColorBQ.G < 85) && (pixelColorBQ.R > 100 && pixelColorBQ.R < 175) && (pixelColorBQ.B > 10 && pixelColorBQ.B < 35))
                    {
                        KeyInput.sendK(0x10); //DI_KEY for Q
                    }
                    else if ((pixelColorBE.G > 30 && pixelColorBE.G < 85) && (pixelColorBE.R > 100 && pixelColorBE.R < 175) && (pixelColorBE.B > 10 && pixelColorBE.B < 35))
                    {
                        KeyInput.sendK(0x12); //DI_KEY for numpad E
                    }
                    else if ((pixelColorB1.G > 30 && pixelColorB1.G < 85) && (pixelColorB1.R > 100 && pixelColorB1.R < 175) && (pixelColorB1.B > 10 && pixelColorB1.B < 35))
                    {
                        KeyInput.sendK(0x02); //DI_KEY for numpad 1
                    }
                    else if ((pixelColorB2.G > 30 && pixelColorB2.G < 85) && (pixelColorB2.R > 100 && pixelColorB2.R < 175) && (pixelColorB2.B > 10 && pixelColorB2.B < 35))
                    {
                        KeyInput.sendK(0x03); //DI_KEY for numpad 2
                    }
                    else if ((pixelColorB3.G > 30 && pixelColorB3.G < 85) && (pixelColorB3.R > 100 && pixelColorB3.R < 175) && (pixelColorB3.B > 10 && pixelColorB3.B < 35))
                    {
                        KeyInput.sendK(0x04); //DI_KEY for numpad 3
                    }
                    Thread.Sleep(400);
                }
            }
            else
            {
                while (comboState)
                {
                    Color pixelColorB1 = getPixel(605, 967);//73,4,3 -> 118 -> 45, 18
                    Color pixelColorB2 = getPixel(660, 967);//96,6,3 -> 145,62,22
                    Color pixelColorB3 = getPixel(713, 967);// 86,5,3 -> 147, 69, 25

                    if ((pixelColorB1.G > 30 && pixelColorB1.G < 85) && (pixelColorB1.R > 100 && pixelColorB1.R < 175) && (pixelColorB1.B > 10 && pixelColorB1.B < 35))
                    {
                        KeyInput.sendK(0x02); //DI_KEY for numpad 1
                    }
                    else if ((pixelColorB2.G > 30 && pixelColorB2.G < 85) && (pixelColorB2.R > 100 && pixelColorB2.R < 175) && (pixelColorB2.B > 10 && pixelColorB2.B < 35))
                    {
                        KeyInput.sendK(0x03); //DI_KEY for numpad 2
                    }
                    else if ((pixelColorB3.G > 30 && pixelColorB3.G < 85) && (pixelColorB3.R > 100 && pixelColorB3.R < 175) && (pixelColorB3.B > 10 && pixelColorB3.B < 35))
                    {
                        KeyInput.sendK(0x04); //DI_KEY for numpad 3
                    }
                    Thread.Sleep(400);
                }
                }
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

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        private void GetPixelButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            String pixc = "";
            while (i < 20)
            {
                Color pixelColor = getPixel(605, 967);
                if (pixelColor.G > 20)
                {
                    KeyInput.sendK(0x02);
                    pixc = pixc + pixelColor.G.ToString() + " : ";
                }
                i++;
                Thread.Sleep(500);
            }
            MessageBox.Show(pixc);

        }

        private static Color getPixel(int x, int y)
        {
            Bitmap screenShotBMP = new Bitmap(
                1,
                1,
                PixelFormat.Format32bppArgb);

            Graphics screenShotGraphics = Graphics.FromImage(
                screenShotBMP);

            screenShotGraphics.CopyFromScreen(
                x,
                y,
                0, 0, screenShotBMP.Size,
                CopyPixelOperation.SourceCopy);

            Color pixelColor = screenShotBMP.GetPixel(0, 0);
            screenShotGraphics.Dispose();
            return (pixelColor);
            
        }

     
            
        

    }
}