using System;
using System.Runtime.CompilerServices;

// Modified Source - Originally from Reloaded.Memory - 7.1.0
// https://github.com/Reloaded-Project/Reloaded.Memory/tree/7.1.0

namespace MTPLib.Util
{
    /// <summary>
    /// Utility class for working with struct arrays.
    /// </summary>
    public static unsafe class StructArray
    {
        private const int MaxStackLimit = 1024;

        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        /// <param name="elementCount">The number of array elements present.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>(int elementCount) where T : unmanaged
        {
            return Struct.GetSize<T>() * elementCount;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        public static byte[] GetBytes<T>(T[] items) where T : unmanaged
        {
            int totalSize   = GetSize<T>(items.Length);
            var result = new byte[totalSize];
            GetBytes(items, result.AsSpan());
            return result;
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="items">The item to convert into a byte array.</param>
        /// <param name="buffer">The buffer to which write the bytes to.</param>
        /// <returns>The passed in buffer sliced to include only the bytes obtained.</returns>
        public static Span<byte> GetBytes<T>(T[] items, Span<byte> buffer) where T : unmanaged
        {
            int totalSize  = GetSize<T>(items.Length);
            var resultSpan = buffer.Slice(0, totalSize);

            if (sizeof(T) < MaxStackLimit)
            {
                Span<byte> currentItem = stackalloc byte[sizeof(T)];
                GetBytesInternal(items, currentItem, resultSpan);
            }
            else
            {
                Span<byte> currentItem = new byte[sizeof(T)];
                GetBytesInternal(items, currentItem, resultSpan);
            }

            return resultSpan;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void GetBytesInternal(T[] items, Span<byte> currentItem, Span<byte> span)
            {
                for (int x = 0; x < items.Length; x++)
                {
                    Struct.GetBytes(ref items[x], currentItem);
                    currentItem.CopyTo(span);
                    span = span.Slice(sizeof(T));
                }
            }
        }
    }
}
