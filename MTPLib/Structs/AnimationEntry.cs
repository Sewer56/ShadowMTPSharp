using Reloaded.Memory;

namespace MTPLib.Structs
{
    /// <summary>
    /// An individual file entry in the <see cref="MotionPackage"/> archive.
    /// </summary>
    public unsafe struct AnimationEntry : IEndianReversible
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

        public void SwapEndian()
        {
            Endian.Reverse(ref FileNamePtr);
            Endian.Reverse(ref FileDataPtr);
            Endian.Reverse(ref PropertyTuplePtr);
        }
    }
}