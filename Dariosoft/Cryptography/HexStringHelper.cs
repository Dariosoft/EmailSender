namespace Dariosoft.Framework.Cryptography
{
    public static class HexStringHelper
    {
        public static string ToHexString(byte[] input)
        {
            if (input == null)
                return string.Empty;
            else if (input.Length < 1)
                return "";
            else
                return BitConverter.ToString(input).Replace("-", "");
        }

        public static byte[] FromHexString(string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString))
                return Array.Empty<byte>();

            hexString = hexString.StartsWith("0x") ? hexString.Substring(2) : hexString;

            if (string.IsNullOrWhiteSpace(hexString))
                return Array.Empty<byte>();

            return Enumerable.Range(0, hexString.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
