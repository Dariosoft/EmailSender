namespace Dariosoft.Framework.Cryptography
{
    public static class GuidEncoder
    {
        public static Guid Decode(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 40)
                return Guid.Empty;

            //4. Replace 2
            string[] parts = [
                string.Join("", input.Skip(10).Take(5)),
                string.Join("", input.Skip(15).Take(5)),
                string.Join("", input.Skip(20).Take(5)),
                string.Join("", input.Skip(30).Take(5)),
                string.Join("", input.Take(5)),
                string.Join("", input.Skip(5).Take(5)),
                string.Join("", input.Skip(35)),
                string.Join("", input.Skip(25).Take(5)),
            ];

            parts[0] = PatternReplace(parts[0], "kvwqozx", "518acef");
            parts[1] = PatternReplace(parts[1], "rstzk", "234ab");
            parts[2] = PatternReplace(parts[2], "qhilrpn", "6751cde");
            parts[3] = PatternReplace(parts[3], "nmzyso", "123fde");
            parts[4] = PatternReplace(parts[4], "tqvnmo", "6e3fd1");
            parts[5] = PatternReplace(parts[5], "tjrpx", "5ba07");
            parts[6] = PatternReplace(parts[6], "kjxpyu", "0e9acd");
            parts[7] = PatternReplace(parts[7], "iuwqkxz", "042abef");

            var target = string.Join("", parts).ToArray();

            #region Validation
            var ok = target[0] == target[10] &&
                     target[1] == target[3] &&
                     target[5] == target[4] &&
                     target[12] == target[7] &&
                     target[8] == target[26] &&
                     target[13] == target[27] &&
                     target[18] == target[25] &&
                     target[23] == target[24];

            if (!ok) return Guid.Empty;
            #endregion

            target[0] = target[39];
            target[1] = target[38];
            target[5] = target[37];
            target[12] = target[36];
            target[8] = '-';
            target[13] = '-';
            target[18] = '-';
            target[23] = '-';
            target[36] = '\0';
            target[37] = '\0';
            target[38] = '\0';
            target[39] = '\0';

            var s = string.Join("", target.Where(c => c != '\0'));

            return Guid.TryParse(s, out var id) ? id : Guid.Empty;
        }

        public static string Encode(Guid input)
        {
            if (input == Guid.Empty)
                return "0".PadLeft(40, '0');

            //1. Copying the guid without separators into the target array
            var target = new char[40];
            Array.Copy(input.ToString().ToLower().ToCharArray(), target, 36);

            //2. Replace 1

            target[8] = target[26];
            target[13] = target[27];
            target[18] = target[25];
            target[23] = target[24];


            target[36] = target[12];
            target[37] = target[5];
            target[38] = target[1];
            target[39] = target[0];

            //5. Replace 1
            target[0] = target[10];
            target[1] = target[3];
            target[5] = target[4];
            target[12] = target[7];

            //6. Replace 2
            string[] parts = [
                string.Join("", target.Take(5)),
                string.Join("", target.Skip(5).Take(5)),
                string.Join("", target.Skip(10).Take(5)),
                string.Join("", target.Skip(15).Take(5)),
                string.Join("", target.Skip(20).Take(5)),
                string.Join("", target.Skip(25).Take(5)),
                string.Join("", target.Skip(30).Take(5)),
                string.Join("", target.Skip(35))
                ];

            parts[0] = PatternReplace(parts[0], "518acef", "kvwqozx");
            parts[1] = PatternReplace(parts[1], "234ab", "rstzk");
            parts[2] = PatternReplace(parts[2], "6751cde", "qhilrpn");
            parts[3] = PatternReplace(parts[3], "123fde", "nmzyso");
            parts[4] = PatternReplace(parts[4], "6e3fd1", "tqvnmo");
            parts[5] = PatternReplace(parts[5], "5ba07", "tjrpx");
            parts[6] = PatternReplace(parts[6], "0e9acd", "kjxpyu");
            parts[7] = PatternReplace(parts[7], "042abef", "iuwqkxz");

            // 7. Join
            return parts[4] + parts[5] + parts[0] + parts[1] + parts[2] + parts[7] + parts[3] + parts[6];
        }

        private static string PatternReplace(string s, string from, string to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                s = s.Replace(from[i], to[i]);
            }
            return s;
        }
    }
}
