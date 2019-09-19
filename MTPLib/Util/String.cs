using System.Text;

namespace MTPLib.Util
{
    /// <summary>
    /// Utility class for managing strings.
    /// </summary>
    public static class String
    {
        /// <summary>
        /// Encodes a string using Windows 1252.
        /// </summary>
        public static Encoding Win1252Encoder { get; private set; }

        static String()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Win1252Encoder = Encoding.GetEncoding(1252);
        }

        /// <summary>
        /// Converts a string to null terminated bytes.
        /// </summary>
        public static byte[] GetNullTerminatedBytes(Encoding encoding, string text)
        {
            byte[] bytes = new byte[text.Length + 1];
            encoding.GetBytes(text, 0, text.Length, bytes, 0);
            bytes[bytes.Length - 1] = 0;
            return bytes;
        }

        /// <summary>
        /// Gets the length of a null terminated string pointer.
        /// </summary>
        public static unsafe int Strlen(byte* stringPtr)
        {
            int length = 0;
            while (stringPtr[length] != 0x00)
                length++;

            return length;
        }
    }
}
