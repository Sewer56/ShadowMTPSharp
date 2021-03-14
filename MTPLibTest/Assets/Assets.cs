using System.IO;

namespace MTPLibTest.Assets
{
    public static class Assets
    {
        public static byte[] BkChaos() => File.ReadAllBytes("Assets/BKCHAOS.MTP");
        public static byte[] BkLarva() => File.ReadAllBytes("Assets/BKLARVA.MTP");
        public static byte[] Shadow() => File.ReadAllBytes("Assets/SHADOW.MTP");
        public static byte[] SuperShadow() => File.ReadAllBytes("Assets/SUPERSHADOW.MTP");

    }
}
