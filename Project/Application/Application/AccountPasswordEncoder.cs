using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.Application
{
    public class AccountPasswordEncoder
    {
        private AccountPasswordEncoder() { }

        private static Lazy<AccountPasswordEncoder> _lazy = new(() => new AccountPasswordEncoder());

        public static AccountPasswordEncoder Instnace => _lazy.Value;

        public string Encode(string emailAddress, string plainPassword)
            => Framework.Cryptography.StringEncoder.TryEncrypt(plainText: plainPassword, key: ComputeKey(emailAddress), out var cipheredPassword) ? cipheredPassword : "";

        public string Decode(string emailAddress, string cipheredPassword)
            => Framework.Cryptography.StringEncoder.TryDecrypt(cipherText: cipheredPassword, key: ComputeKey(emailAddress), out var plainPassword) ? plainPassword : "";

        private string ComputeKey(string emailAddress)
            => Framework.Cryptography.Hashing.MD5(emailAddress.ToLower().Trim());
    }
}
