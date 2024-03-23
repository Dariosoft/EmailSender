using System.Text;

namespace Dariosoft.Framework.Cryptography
{

    public class Hashing
    {
        #region MD5
        public static string MD5(byte[] input)
        {
            var result = string.Empty;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                result = HexStringHelper.ToHexString(md5.ComputeHash(input));
            }

            return result;
        }

        public static string MD5(string plainText)
            => MD5(Encoding.UTF8.GetBytes(plainText));
        #endregion

        #region SHA1
        public static string SHA1(byte[] input)
        {
            var result = string.Empty;
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                result = HexStringHelper.ToHexString(sha1.ComputeHash(input));
            }

            return result;
        }

        public static string SHA1(string plainText)
            => SHA1(Encoding.UTF8.GetBytes(plainText));
        #endregion

        #region SHA256
        public static string SHA256(byte[] input)
        {
            var result = string.Empty;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                result = HexStringHelper.ToHexString(sha256.ComputeHash(input));
            }

            return result;
        }

        public static string SHA256(string plainText)
            => SHA256(Encoding.UTF8.GetBytes(plainText));
        #endregion

        #region SHA512
        public static string SHA512(byte[] input)
        {
            var result = string.Empty;
            using (var sha512 = System.Security.Cryptography.SHA512.Create())
            {
                result = HexStringHelper.ToHexString(sha512.ComputeHash(input));
            }

            return result;
        }

        public static string SHA512(string plainText)
            => SHA256(Encoding.UTF8.GetBytes(plainText));
        #endregion
    }
}
