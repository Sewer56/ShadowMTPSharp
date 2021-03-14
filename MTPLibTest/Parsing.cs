using System.Linq;
using MTPLib;
using Xunit;

namespace MTPLibTest
{
    public class Parsing
    {
        [Fact]
        public void WithVariableExtraProperties()
        {
            var package = MotionPackage.FromMtp(Assets.Assets.BkChaos());
            Assert.Equal(3, package.Entries.Length);

            Assert.NotEqual(default, package.Entries.FirstOrDefault(x => x.FileName == "chaos_attack"));
            Assert.NotEqual(default, package.Entries.FirstOrDefault(x => x.FileName == "chaos_rot"));
            Assert.NotEqual(default, package.Entries.FirstOrDefault(x => x.FileName == "chaos_water"));
        }

        [Fact]
        public void WithSingleEntry()
        {
            var package = MotionPackage.FromMtp(Assets.Assets.BkLarva());
            Assert.Single(package.Entries);
            Assert.NotEqual(default, package.Entries.FirstOrDefault(x => x.FileName == "bklarva_move"));
        }
    }
}
