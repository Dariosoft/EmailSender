using System.Text;

namespace Dariosoft.Framework.Cryptography
{
    public class StringEncoder
    {
        static readonly Cryptography _encoder = new();

        #region Decrypt
        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return string.IsNullOrEmpty(cipherText) 
                ? cipherText 
                : Encoding.UTF8.GetString(_encoder.Decrypt(HexStringHelper.FromHexString(cipherText), key));
        }

        public static bool TryDecrypt(string cipherText, string key, out string result)
        {
            try
            {
                result = Decrypt(cipherText, key);
                return true;
            }
            catch
            {
                result = string.Empty;
                return false;
            }
        }
        #endregion

        #region Encrypt
        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return string.IsNullOrEmpty(plainText) ? plainText : HexStringHelper.ToHexString(_encoder.Encrypt(plainText, key));
        }

        public static bool TryEncrypt(string plainText, string key, out string result)
        {
            try
            {
                result = Encrypt(plainText, key);
                return true;
            }
            catch
            {
                result = string.Empty;
                return false;
            }
        }
        #endregion
    }
}
