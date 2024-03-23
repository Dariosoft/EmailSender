using System.ComponentModel;

namespace Dariosoft.Framework.Helpers
{
    public class EnumHelper
    {
        private EnumHelper() { }

        private readonly static Lazy<EnumHelper> _lazy = new Lazy<EnumHelper>(() => new EnumHelper());

        public static EnumHelper Instance => _lazy.Value;

        #region GetDescription
        public string GetDescription(Type enumType, string name)
        {
            if (!enumType.IsEnum)
                return string.Empty;

            return enumType.GetMember(name)
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Select(attr => attr as DescriptionAttribute)
                .Where(attr => attr != null)
                .Select(attr => attr!.Description)
                .FirstOrDefault() ?? string.Empty;
        }

        public string GetDescription<TEnum>(TEnum value)
            where TEnum : Enum
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);

            if (string.IsNullOrEmpty(name))
                return string.Empty;

            return GetDescription(enumType, name);
        }
        #endregion

        #region GetDescriptions
        public IDictionary<int, string> GetDescriptions(Type enumType, bool skipZeroItem = true)
        {
            if (!enumType.IsEnum)
                return new Dictionary<int, string>();

            return Enum.GetNames(enumType)
                       .Select(name => new { Val = Convert.ToInt32(Enum.Parse(enumType, name)), Name = GetDescription(enumType, name) })
                       .Where(e => !skipZeroItem || e.Val > 0)
                       .OrderBy(e => e.Val)
                       .ToDictionary(e => e.Val, e => e.Name);
        }

        public IDictionary<int, string> GetDescriptions<TEnum>(bool skipZeroItem = true)
            where TEnum : Enum
        {
            return GetDescriptions(typeof(TEnum), skipZeroItem);
        }
        #endregion

        #region GetItems
        public IDictionary<string, int> GetItems(Type enumType, bool skipZeroItem = true)
        {
            if (!enumType.IsEnum)
                return new Dictionary<string, int>();

            return Enum.GetNames(enumType)
                .Select(name => new { Name = name, Val = Convert.ToInt32(Enum.Parse(enumType, name)) })
                .Where(e => !skipZeroItem || e.Val > 0)
                .ToDictionary(e => e.Name, e => e.Val);
        }

        public IDictionary<string, int> GetItems<TEnum>(bool skipZeroItem = true)
            where TEnum : Enum
        {
            return GetItems(typeof(TEnum), skipZeroItem);
        }
        #endregion

        #region GetValue
        public T GetValue<T>(string name)
            where T : Enum => (Enum.TryParse(typeof(T), name, out var result) && result is not null ? (T)result : default)!;

        public T GetValue<T>(long value)
            where T : Enum
        {
            var name = Enum.GetName(typeof(T), value);
            return (string.IsNullOrWhiteSpace(name) ? default : GetValue<T>(name))!;
        }
        #endregion

        public bool HasFlags<TEnum>(TEnum val, params Enum[] flags)
          where TEnum : struct, Enum

        {
            ulong uLong = Convert.ToUInt64(val);

            return flags.Select(Convert.ToUInt64).All(f => (uLong & f) != 0);

        }
    }
}
