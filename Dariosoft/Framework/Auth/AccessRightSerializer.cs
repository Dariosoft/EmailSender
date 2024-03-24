
namespace Dariosoft.Framework.Auth
{
    public class AccessRightSerializer
    {
        private AccessRightSerializer() { }

        private readonly static Lazy<AccessRightSerializer> _lazy = new Lazy<AccessRightSerializer>(() => new AccessRightSerializer());

        public static AccessRightSerializer Instance => _lazy.Value;

        public string[] Serialize(IAccessRight[] accessRights)
        {
            return accessRights.SelectMany(policy => policy.Roles.Select(role => $"{policy.Name}/{role}")).ToArray();
        }

        public IAccessRight[] Deserialize(params string[] items)
        {
            return items
                .Select(Parse)
                .Where(p => p is not null && !string.IsNullOrWhiteSpace(p.Item1))
                .GroupBy(i => i!.Item1)
                .Select(app => new AccessRight { Name = app.Key, Roles = app.Select(r => r!.Item2).ToArray() })
                .ToArray();
        }

        private Tuple<string, string>? Parse(string item)
        {


            if (string.IsNullOrWhiteSpace(item))
                return null;


            var i = item.LastIndexOf('/');



            return new Tuple<string, string>(i > 0 ? item.Substring(0, i) : item, i > 0 ? item.Substring(i + 1) : "");


        }
    }
}
