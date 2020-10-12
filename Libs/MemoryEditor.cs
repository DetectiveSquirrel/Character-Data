// Decompiled with JetBrains decompiler
// Type: Typing_Buttons.Utils.MemoryEditor
// Assembly: Typing Buttons, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F0A1A831-F1A8-4E0C-8D57-E89E4C077FC4
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Z Typing Buttons\Typing Buttons.dll

using System;
using System.Runtime.InteropServices;

namespace CharacterData.Libs
{
    internal class MemoryEditor
    {
        private readonly int _handle;

        public MemoryEditor(int pid)
        {
            _handle = (int) OpenProcess(56U, 1, (uint) pid);
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] byte[] buffer,
            uint size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
            int lpNumberOfBytesWritten);

        public int MultiLevelPointerReader(int address, int[] offsets)
        {
            var length = offsets.Length;
            var num1 = address;
            var buffer = new byte[4];
            var num2 = 1;
            var num3 = length - num2;
            for (var index = 0; index <= num3; ++index)
            {
                var lpNumberOfBytesRead = IntPtr.Zero;
                ReadProcessMemory((IntPtr) _handle, (IntPtr) num1, buffer, 4U, out lpNumberOfBytesRead);
                num1 = BitConverter.ToInt32(buffer, 0) + offsets[index];
            }

            return num1;
        }

        public byte[] ReadByteArray(int address, int size)
        {
            var num1 = 0;
            var buffer = new byte[size - 1 + 1];
            var lpNumberOfBytesRead = (IntPtr) num1;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, (uint) size, out lpNumberOfBytesRead);
            var num2 = (int) lpNumberOfBytesRead;
            return buffer;
        }

        public byte[] ReadByteArray(long address, int size)
        {
            var num1 = 0;
            var buffer = new byte[size - 1 + 1];
            var lpNumberOfBytesRead = (IntPtr) num1;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, (uint) size, out lpNumberOfBytesRead);
            var num2 = (int) lpNumberOfBytesRead;
            return buffer;
        }

        public double ReadDouble(int address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[8];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 8U, out lpNumberOfBytesRead);
            return (int) Math.Round(BitConverter.ToDouble(buffer, 0));
        }

        public double ReadDouble(long address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[8];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 8U, out lpNumberOfBytesRead);
            return (int) Math.Round(BitConverter.ToDouble(buffer, 0));
        }

        public float ReadFloat(int address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[4];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 4U, out lpNumberOfBytesRead);
            return (int) Math.Round(BitConverter.ToSingle(buffer, 0));
        }

        public float ReadFloat(long address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[4];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 4U, out lpNumberOfBytesRead);
            return (int) Math.Round(BitConverter.ToSingle(buffer, 0));
        }

        public int ReadInteger(int address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[4];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 4U, out lpNumberOfBytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        public int ReadInteger(long address)
        {
            var zero = IntPtr.Zero;
            var buffer = new byte[4];
            var lpNumberOfBytesRead = zero;
            ReadProcessMemory((IntPtr) _handle, (IntPtr) address, buffer, 4U, out lpNumberOfBytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        public bool WriteByteArray(int address, byte[] bArray)
        {
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bArray, (uint) bArray.Length, 0);
        }

        public bool WriteByteArray(long address, byte[] bArray)
        {
            return WriteProcessMemory((IntPtr)_handle, (IntPtr)address, bArray, (uint)bArray.Length, 0);
        }

        public bool WriteByteArray(long address, byte[] bArray, int arraySize)
        {
            return WriteProcessMemory((IntPtr)_handle, (IntPtr)address, bArray, (uint)arraySize, 0);
        }

        public bool WriteDouble(int address, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 8U, 0);
        }

        public bool WriteDouble(float address, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) (long) address, bytes, 8U, 0);
        }

        public bool WriteDouble(long address, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 8U, 0);
        }

        public bool WriteFloat(int address, float value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 4U, 0);
        }

        public bool WriteFloat(long address, float value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 4U, 0);
        }

        public bool WriteInteger(int address, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 4U, 0);
        }

        internal bool WriteInteger(long address, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return WriteProcessMemory((IntPtr) _handle, (IntPtr) address, bytes, 4U, 0);
        }
    }
}