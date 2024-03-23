using System.Text;
using System.Text.RegularExpressions;

namespace Dariosoft.Framework
{
    public static class StringExtensions
    {
        public static string? ReduceSpaces(this string? input)
        {
            return string.IsNullOrWhiteSpace(input) 
                ? null
                : Regex.Replace(input: input, pattern: @"\s{2,}", replacement: " ");
        }

        /// <summary>
        /// <para>Cleaning the input string</para>
        /// <para>Removes the special characters(e.g Symbols, Punctuation marks, etc) in the input string and converts all characters to lower case as well.</para>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keepOriginalCase"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public static string? Clean(this string? input, bool keepOriginalCase = false, params char[]? exceptions)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            exceptions ??= Array.Empty<char>();

            input = string.Join(string.Empty, TranslateDigitToEnglish(input)
                .Normalize(NormalizationForm.FormD)
                .Where(letter => char.IsLetterOrDigit(letter) ||
                                 char.IsWhiteSpace(letter) ||
                                 exceptions.Contains(letter)
                      ));

            input = ReduceSpaces(input)?.Trim();

            return keepOriginalCase ? input : input?.ToLower();
        }

        /// <summary>
        /// <para>Cleaning the input string</para>
        /// <para>Removes the special characters(e.g Symbols, Punctuation marks, etc) in the input string and converts all characters to lower case as well.</para>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keepOriginalCase"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public static string? Clean(this string? input, bool keepOriginalCase = false, string? exceptions = null)
            => Clean(input, keepOriginalCase, exceptions?.ToCharArray());

        public static string? XNormalize(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            return TranslateDigitToEnglish(input).Normalize(NormalizationForm.FormD);
        }

        /// <summary>
        /// <para>Translates the non-english digits to English digits.</para>
        /// <para>Example: ٢ -> 2 , ۵ -> 5 </para>
        /// </summary>
        /// <returns></returns>
        public static string TranslateDigitToEnglish(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Replace('٠', '0').Replace('۰', '0')
                .Replace('١', '1').Replace('۱', '1')
                .Replace('٢', '2').Replace('۲', '2')
                .Replace('٣', '3').Replace('۳', '3')
                .Replace('٤', '4').Replace('۴', '4')
                .Replace('٥', '5').Replace('۵', '5')
                .Replace('٦', '6').Replace('۶', '6')
                .Replace('٧', '7').Replace('۷', '7')
                .Replace('٨', '8').Replace('۸', '8')
                .Replace('٩', '9').Replace('۹', '9');
        }

        /// <summary>
        /// <para>Converting Arabic characters to its Persian equivalent.</para>
        /// <para>Example: ي => ی</para>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TranslateArabicCharsToPersianchars(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Replace('ي', 'ی')
                .Replace('ك', 'ک')
                .Replace('ة', 'ه')
                .Replace('ؤ', 'و')
                .Replace('إ', 'ا')
                .Replace('أ', 'ا')
                .Replace('ء', '\0')
                .Replace('ۀ', 'ه');
        }


        /// <summary>
        /// Verfies that the input string is a valid email address or not.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return Regex.IsMatch(pattern: @"(?i)^\w{1,}[\w-\.]{0,}@\w{1,}\.{1}\w{1,}(\.\w+){0,}$", input: input);
        }
    }
}
