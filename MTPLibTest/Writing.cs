using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MTPLib;
using Xunit;

namespace MTPLibTest
{
    public class Writing
    {
        [Fact]
        public void SameAsOriginalSingleFile()
        {
            var originalFile   = Assets.Assets.BkLarva();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile        = originalParsed.ToMtp();
            var newParsed      = MotionPackage.FromMtp(newFile);

            Assert.Equal(originalParsed, newParsed);
        }

        [Fact]
        public void SameAsOriginal()
        {
            var originalFile = Assets.Assets.BkChaos();
            var originalParsed = MotionPackage.FromMtp(originalFile);

            var newFile = originalParsed.ToMtp();
            var newParsed = MotionPackage.FromMtp(newFile);

            Assert.Equal(originalParsed, newParsed);
        }
    }
}
