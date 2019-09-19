using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MTPLib;
using Xunit;

namespace MTPLibTest
{
    public class Exporting
    {
        private string BkLarvaDirectory => Path.GetFullPath("BkLarvaExport");
        private string BkChaosDirectory => Path.GetFullPath("BkChaosExport");

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
    }
}
