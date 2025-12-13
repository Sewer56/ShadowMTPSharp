using System;
using System.Runtime.InteropServices;

namespace MTPLib.Util
{
    public static class Struct
    {
        public static byte[] GetBytes<T>(ref T str) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(str, ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return arr;
        }
    }

    public static class StructArray
    {
        public static byte[] GetBytes<T>(T[] arr) where T : struct
        {
            if (arr == null || arr.Length == 0) return Array.Empty<byte>();
            int size = Marshal.SizeOf(typeof(T));
            int total = size * arr.Length;
            var result = new byte[total];
            IntPtr ptr = Marshal.AllocHGlobal(total);
            try
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    IntPtr dest = IntPtr.Add(ptr, i * size);
                    Marshal.StructureToPtr(arr[i], dest, false);
                }

                Marshal.Copy(ptr, result, 0, total);
            }
            finally
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    IntPtr dest = IntPtr.Add(ptr, i * size);
                    Marshal.DestroyStructure(dest, typeof(T));
                }

                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
    }
}
