using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Utilities;

namespace MTPLib.Structs
{
    /// <summary>
    /// An individual file entry in the <see cref="MotionPackage"/> archive.
    /// </summary>
    public unsafe struct AnimationEntry : ICanReverseEndian
    {
        public int FileNamePtr;
        public int FileDataPtr;

        /// <summary>
        /// Contains unknown properties. Not all animations have these.
        /// </summary>
        public int PropertyTuplePtr;

        /// <summary>
        /// True if the animation has additional properties, else false.
        /// </summary>
        public bool HasProperties => PropertyTuplePtr != 0;

        public void ReverseEndian()
        {
            FileNamePtr = Endian.Reverse(FileNamePtr);
            FileDataPtr = Endian.Reverse(FileDataPtr);
            PropertyTuplePtr = Endian.Reverse(PropertyTuplePtr);
        }
    }
}