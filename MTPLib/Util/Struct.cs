using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Modified Source - Originally from Reloaded.Memory - 7.1.0
// https://github.com/Reloaded-Project/Reloaded.Memory/tree/7.1.0

namespace MTPLib.Util
{
    /// <summary>
    /// Struct is a general utility class providing functions which provides various functions for working with structures; such
    /// as reading/writing to/from memory of structures.
    /// </summary>
    public static unsafe class Struct
    {
  
        /// <summary>
        /// Returns the size of a specific primitive or struct type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSize<T>() where T : unmanaged
        {
            return Unsafe.SizeOf<T>();
        }

        /// <summary>
        /// Creates a byte array from specified structure or class type with explicit StructLayout attribute.
        /// </summary>
        /// <param name="item">The item to convert into a byte array.</param>
        /// <param name="buffer">Buffer inside which the result is to be stored.</param>
        /// <returns>Original span sliced to contain only the bytes of the struct.</returns>
        public static Span<byte> GetBytes<T>(ref T item, Span<byte> buffer) where T : unmanaged
        {
            MemoryMarshal.Write(buffer, ref item);
            return buffer.Slice(0, sizeof(T));
        }
    }
}
