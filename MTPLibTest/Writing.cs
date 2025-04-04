using MTPLib;
using Xunit;

namespace MTPLibTest
{
    public class Writing
    {
        [Fact]
        public void SameAsOriginalSingleFile_InboundMTP()
        {
            var originalFile   = Assets.Assets.BkLarva();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile        = originalParsed.ToMtp();
            var newParsed      = MotionPackage.FromMtp(newFile);

            Assert.Equal(originalParsed, newParsed);
        }

        [Fact]
        public void SameAsOriginal_InboundMTP()
        {
            var originalFile = Assets.Assets.BkChaos();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile = originalParsed.ToMtp();
            var newParsed = MotionPackage.FromMtp(newFile);

            Assert.Equal(originalParsed, newParsed);
        }

        [Fact]
        public void SameAsOriginalSingleFile_Export()
        {
            var originalFile = Assets.Assets.BkLarva();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile = originalParsed.ToMtp();

            Assert.Equal(originalFile, newFile);
        }

        [Fact]
        public void SameAsOriginal_Export()
        {
            var originalFile = Assets.Assets.BkChaos();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile = originalParsed.ToMtp();

            Assert.Equal(originalFile, newFile);
        }

        [Fact]
        public void SameAsOriginal_MixedPropertiesType_Export()
        {
            var originalFile = Assets.Assets.SuperShadow();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile = originalParsed.ToMtp();

            Assert.Equal(originalFile, newFile);
        }
    }
}
