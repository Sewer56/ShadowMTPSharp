using Reloaded.Memory;

namespace MTPLib.Structs
{
    /// <summary>
    /// The header of a .MTN file.
    /// This is the reduced header, not the full one.
    /// </summary>
    public struct MotionHeader : IEndianReversible
    {
        private byte    _unknown0;
        public  byte    IsBigEndian;
        private short   _unknown1;
        private int     _unknown2;
        public  int     FileSize;

        /// <summary>
        /// Obtains a pointer to the header, reversing the endian if necessary.
        /// </summary>
        public static unsafe MotionHeader FromPointer(void* pointer)
        {
            var header = *(MotionHeader*) pointer;
            if (header.IsBigEndian == 1)
                header.SwapEndian();

            return header;
        }

        public void SwapEndian()
        {
            Endian.Reverse(ref _unknown0);
            Endian.Reverse(ref _unknown1);
            Endian.Reverse(ref _unknown2);
            Endian.Reverse(ref IsBigEndian);
            Endian.Reverse(ref FileSize);
        }
    }
}