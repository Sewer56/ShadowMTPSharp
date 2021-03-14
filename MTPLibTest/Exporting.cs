using System.IO;
using MTPLib;
using Xunit;

namespace MTPLibTest
{
    public class Exporting
    {
        private string BkLarvaDirectory => Path.GetFullPath("BkLarvaExport");
        private string BkChaosDirectory => Path.GetFullPath("BkChaosExport");
        private string SuperShadowDirectory => Path.GetFullPath("SuperShadowExport");
        private string ShadowDirectory => Path.GetFullPath("ShadowExport");

        private string InitializeDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }

        [Fact]
        public void ExportMulti()
        {
            var package = MotionPackage.FromMtp(Assets.Assets.BkChaos());
            var directory = InitializeDirectory(BkChaosDirectory);

            package.ToDirectory(directory);

            var newPackage = MotionPackage.FromDirectory(directory);
            Assert.Equal(package, newPackage);
        }

        [Fact]
        public void ExportSingle()
        {
            var package = MotionPackage.FromMtp(Assets.Assets.BkLarva());
            var directory = InitializeDirectory(BkLarvaDirectory);

            package.ToDirectory(directory);

            var newPackage = MotionPackage.FromDirectory(directory);
            Assert.Equal(package, newPackage);
        }

        [Fact]
        public void ExportMulti_MissingSomeProperties()
        {
            // MTP with multiple MTNs and some MTNs with no associated properties
            var package = MotionPackage.FromMtp(Assets.Assets.SuperShadow());
            var directory = InitializeDirectory(SuperShadowDirectory);

            package.ToDirectory(directory);

            var newPackage = MotionPackage.FromDirectory(directory);
            Assert.Equal(package, newPackage);
        }

        [Fact]
        public void SameAsOriginal_OrderNonAlphabeticalNames_Export()
        {
            var package = MotionPackage.FromMtp(Assets.Assets.Shadow());
            var directory = InitializeDirectory(ShadowDirectory);

            package.ToDirectory(directory);
            var newPackage = MotionPackage.FromDirectory(directory);
            Assert.Equal(package, newPackage);
        }
    }
}
