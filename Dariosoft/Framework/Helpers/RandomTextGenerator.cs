namespace Dariosoft.Framework.Helpers
{
    public enum RandomNumberOption
    {
        Default = 0,

        /// <summary>
        /// Trims leading zeros
        /// </summary>
        SkipZeroFromStart = 1,

        /// <summary>
        /// Trims trailing zeros
        /// </summary>
        SkipZeroFromEnd = 2,

        /// <summary>
        /// <para>Trims zeros from the head and the tail</para>
        /// </summary>
        SkipStartAndEndZeror = 2,

        /// <summary>
        /// Generate a number that not contains zero
        /// </summary>
        SkipZeroAtAll = 4
    }

    [Flags]
    public enum RandomTextGeneratorOptions
    {
        Default = 0,
        IgnoreSymbols = 1,
        IgnoreDigits = 2,
        IgnoreCase = 4
    }

    public static class RandomTextGenerator
    {
        private readonly static string alphabet_lower = "abcdefghijklmnopqrstuvwxyz";
        private readonly static string alphabet_upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly static string digits = "1234567890";
        private readonly static string symbols = "!@#$%^&*?+-~[]()_={}";

        private readonly static Random random = new Random();

        private static float DistinctRate(string text)
        {
            float distinct_length = text.Distinct().Count(), flength = text.Length;
            return distinct_length / flength;
        }

        private static string Generate(ushort length, string topic)
        {
            if (length < 1)
                return string.Empty;

            var chars = new char[length];
            var chunkSize = topic.Length / 3;

            int start = 0,
                end = topic.Length - 1,
                a = chunkSize + 1,
                b = 2 * chunkSize + 1,
                x = -1;

            for (int i = 0; i < length; i++)
            {
                x = random.Next(start, end);
                chars[i] = topic[x];

                if (x > 0 && x < a)
                {
                    start = b;
                    end = topic.Length - 1;
                }
                else if (x > b)
                {
                    start = a;
                    end = b - 1;
                }
                else if (x >= a && x <= b)
                {
                    start = 0;
                    end = a - 1;
                }
            }

            return string.Join(string.Empty, chars);
        }

        public static string GenerateRandomNumber(ushort length, RandomNumberOption option)
        {
            var result = string.Empty;
            ushort len = length;

            if (option == RandomNumberOption.Default)
                result = Generate(length, "1234567890");
            else if (option == RandomNumberOption.SkipZeroAtAll)
                result = Generate(length, "123456789");
            else
            {
                do
                {
                    result = result + Generate(len, "1234567890");

                    if (option == RandomNumberOption.SkipZeroFromStart)
                        result = result.TrimStart('0');
                    else if (option == RandomNumberOption.SkipZeroFromEnd)
                        result = result.TrimEnd('0');
                    else if (option == RandomNumberOption.SkipStartAndEndZeror)
                        result = result.Trim('0');

                    if (DistinctRate(result) < .5)
                        result = string.Empty;

                    len = (ushort)(length - result.Length);

                } while (len > 0);
            }


            return result;
        }

        public static string Generate(ushort length, RandomTextGeneratorOptions option = RandomTextGeneratorOptions.Default)
        {
            var topic = option.HasFlag(RandomTextGeneratorOptions.IgnoreCase) ? alphabet_lower : alphabet_lower + alphabet_upper;

            if (!option.HasFlag(RandomTextGeneratorOptions.IgnoreDigits))
                topic += digits;

            if (!option.HasFlag(RandomTextGeneratorOptions.IgnoreSymbols))
                topic += symbols;

            var result = Generate(length, topic);

            while (DistinctRate(result) < .5)
                result = Generate(length, topic);

            return result;
        }
    }
}

