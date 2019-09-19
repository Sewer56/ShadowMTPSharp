using System.IO;

namespace MTPLibTest.Assets
{
    public static class Assets
    {
        public static byte[] BkChaos() => File.ReadAllBytes("Assets/BKCHAOS.MTP");
        public static byte[] BkLarva() => File.ReadAllBytes("Assets/BKLARVA.MTP");
    }
}
