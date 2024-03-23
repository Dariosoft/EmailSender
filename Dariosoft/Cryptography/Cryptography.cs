using System.Text;
using G = System.Security.Cryptography;

namespace Dariosoft.Framework.Cryptography
{
    public class Cryptography
    {
        #region Constructors
        public Cryptography(byte[] key)
        {
            _key = key;
        }

        public Cryptography(string key)
            : this(Encoding.UTF8.GetBytes(key))
        {
        }

        public Cryptography()
            :this("Dariosoft.Framework.Cryptography")
        {
        }
        #endregion

        #region Readonly Fields
        readonly byte[] _key;
        readonly byte[] _salt = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };
        #endregion

        #region Private Methods
        private byte[] ComputeHashKey(byte[] key)
        {
            using (var md5Hash = G.MD5.Create())
            {
                var keyData = md5Hash.ComputeHash(key);
                keyData = keyData.Reverse().ToArray();
                keyData = md5Hash.ComputeHash(keyData);
                md5Hash.Clear();
                return keyData;
            }
        }

        private G.Rfc2898DeriveBytes GetRFC(byte[] key)
        {
            using (var md5Hash = G.MD5.Create())
            {
                var rsaSalt = md5Hash.ComputeHash(_salt);
                md5Hash.Clear();
                return new G.Rfc2898DeriveBytes(key, rsaSalt, 1);
            }

        }
        #endregion

        #region Encrypt
        public byte[] Encrypt(byte[] data, byte[] key)
        {

            byte[]? resultArray = null;

            using (var rfc = GetRFC(ComputeHashKey(key)))
            {
                using (var aes = G.Aes.Create())
                {
                    aes.Key = rfc.GetBytes(32);
                    aes.IV = rfc.GetBytes(16);
                    using (var transform = aes.CreateEncryptor())
                    {
                        resultArray = transform.TransformFinalBlock(data, 0, data.Length);
                    }
                    aes.Clear();
                }
            }

            return resultArray;
        }

        public byte[] Encrypt(byte[] data, string key)
            => Encrypt(data, Encoding.UTF8.GetBytes(key));

        public byte[] Encrypt(byte[] data)
            => Encrypt(data, _key);

        public byte[] Encrypt(string data, string key)
            => Encrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(key));

        public byte[] Encrypt(string data, byte[] key)
            => Encrypt(Encoding.UTF8.GetBytes(data), key);

        public byte[] Encrypt(string data)
            => Encrypt(data, _key);
        #endregion

        #region Decrypt
        public byte[] Decrypt(byte[] data, byte[] key)
        {
            byte[]? resultArray = null;
            using (var rfc = GetRFC(ComputeHashKey(key)))
            {
                using (var aes = G.Aes.Create())
                {
                    aes.Key = rfc.GetBytes(32);
                    aes.IV = rfc.GetBytes(16);
                    using (var transform = aes.CreateDecryptor())
                    {
                        resultArray = transform.TransformFinalBlock(data, 0, data.Length);
                    }
                    aes.Clear();
                }
            }

            return resultArray;
        }

        public byte[] Decrypt(byte[] data, string key)
            => Decrypt(data, Encoding.UTF8.GetBytes(key));

        public byte[] Decrypt(byte[] data)
            => Decrypt(data, _key);

        public string Decrypt(string data, string key)
            => Encoding.UTF8.GetString(Decrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(key)));

        public string Decrypt(string data, byte[] key)
            => Encoding.UTF8.GetString(Decrypt(Encoding.UTF8.GetBytes(data), key));

        public string Decrypt(string data)
            => Decrypt(data, _key);
        #endregion
    }
}
