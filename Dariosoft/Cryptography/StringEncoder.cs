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

        public static bool TryDecrypt(string data, string key, out string result)
        {
            try
            {
                result = Decrypt(data, key);
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
        public static string Encrypt(string data, string key)
        {
            if (string.IsNullOrWhiteSpace(data))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return string.IsNullOrEmpty(data) ? data : HexStringHelper.ToHexString(_encoder.Encrypt(data, key));
        }

        public static bool TryEncrypt(string data, string key, out string result)
        {
            try
            {
                result = Encrypt(data, key);
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
