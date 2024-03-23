namespace Dariosoft.Framework.Cryptography
{
    public class PasswordEncoder
    {
        public static string Encode(string username, string password)
            => Hash(username, password);

        public static bool Verify(string username, string encodedPassword, string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(encodedPassword) || string.IsNullOrWhiteSpace(plainPassword))
                return false;

            return Hash(username, plainPassword) == encodedPassword;
        }

        private static string Hash(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return "";

            username = string.Join("", username.Reverse());

            password = $"[{username}|{password}]";

            return Hashing.SHA512(password)!;
        }
    }
}
