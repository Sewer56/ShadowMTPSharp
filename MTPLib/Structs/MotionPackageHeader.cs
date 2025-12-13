using Reloaded.Memory;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

namespace MTPLib.Structs
{
    public struct MotionPackageHeader : ICanReverseEndian
    {
        /// <summary>
        /// Always 1
        /// </summary>
        public short Enabler;

        /// <summary>
        /// Number of file entries.
        /// </summary>
        public short NumberOfFiles;

        /// <summary>
        /// Might be flag to declare endian of file.
        /// Not handled until I'm sure.
        /// </summary>
        public short Enabler2;

        private short _pad1;
        private int _unknown1;
        private int _unknown2;

        /// <summary>
        /// Offset of the first file entry.
        /// </summary>
        public int EntryOffset;

        /// <summary>
        /// Retrieves a <see cref="MotionPackageHeader"/> from a <see cref="MotionPackage"/>.
        /// Note: Returns as default/little endian. Call <see cref="SwapEndian"/> before writing to file.
        /// </summary>
        public static MotionPackageHeader FromPackage(MotionPackage package)
        {
            var header = new MotionPackageHeader();
            header.Enabler = 1;
            header.Enabler2 = 1;
            header.NumberOfFiles = (short) package.Entries.Length;
            header.EntryOffset = 20;
            return header;
        }

        public void ReverseEndian()
        {
            Enabler = Endian.Reverse(Enabler);
            NumberOfFiles = Endian.Reverse(NumberOfFiles);
            Enabler2 = Endian.Reverse(Enabler2);
            _pad1 = Endian.Reverse(_pad1);
            _unknown1 = Endian.Reverse(_unknown1);
            _unknown2 = Endian.Reverse(_unknown2);
            EntryOffset = Endian.Reverse(EntryOffset);
        }
    }
}