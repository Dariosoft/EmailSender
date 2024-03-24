namespace Dariosoft.Framework.Helpers
{
    public static class StringHelper
    {
        public static string? Coalesce(params string?[] items)
            => items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? items.LastOrDefault();

        public static int ParseToInt(string input, int defaultValue = default)
            => int.TryParse(input?.Trim() ?? "", out var value) ? value : defaultValue;

        public static long ParseToLong(string input, int defaultValue = default)
            => int.TryParse(input?.Trim() ?? "", out var value) ? value : defaultValue;

        public static double ParseToDouble(string input, double defaultValue = default)
            => double.TryParse(input?.Trim() ?? "", out var value) ? value : defaultValue;

        public static double? ParseToNullableDouble(string? input)
            => double.TryParse(input?.Trim() ?? "", out var value) ? value : null;
    }
}
