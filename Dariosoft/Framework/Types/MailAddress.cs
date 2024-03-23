using System.Text.RegularExpressions;

namespace Dariosoft.Framework.Types
{
    public struct MailAddress
    {
        public MailAddress(string? value)
        {
            value = (value ?? "").Trim(' ', '\r', '\n', '\t', ';');

            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Count(c => c == ';') > 1)
                    throw new ArgumentException($"The value \"{value}\" is invalid. If value contains display name it should be sperated by a semicolon.");

                var i = value.IndexOf(';');

                if (i > 0)
                {
                    EmailAddress = value[..i].ToLower();
                    DisplayName = value[(i + 1)..];
                }
                else
                {
                    EmailAddress = value.ToLower();
                    DisplayName = null;
                }
            }
            else
            {
                EmailAddress = "";
                DisplayName = null;
            }

            Value = value;
        }

        public string Value { get; }

        public string EmailAddress { get; }

        public string? DisplayName { get; }

        public bool IsValid => Regex.IsMatch(EmailAddress, @"(?i)^\w{1,}[\w-\.]{0,}@\w{1,}\.{1}\w{1,}(\.\w+){0,}$");

        public static implicit operator string(MailAddress mailAddress) => mailAddress.Value;
        public static implicit operator string(MailAddress? mailAddress) => mailAddress == null ? "" : mailAddress.Value.Value;

        public static implicit operator MailAddress(string value) => new MailAddress(value);
        public static implicit operator MailAddress?(string? value) => string.IsNullOrWhiteSpace(value) ? null : new MailAddress?(value);
    }
}
