using System.Diagnostics.CodeAnalysis;

namespace Dariosoft.Framework.Types
{
    public interface IOrganizationInfo: IEquatable<IOrganizationInfo>, IEqualityComparer<IOrganizationInfo>
    {
        string Id { get; }

        string Name { get; }
    }

    public record OrganizationInfo : IOrganizationInfo
    {
        public string Id { get; init; } = "";

        public string Name { get; init; } = "";

        public bool Equals(IOrganizationInfo? other)
            => Equals(this, other);

        public bool Equals(IOrganizationInfo? x, IOrganizationInfo? y)
            => (x is null ? 0 : GetHashCode(x)) == (y is null ? 0 : GetHashCode(y));

        public int GetHashCode([DisallowNull] IOrganizationInfo obj)
            => string.IsNullOrWhiteSpace(obj?.Id) ? 0 : obj.Id.ToLower().Trim().GetHashCode();

        public override string ToString() => Name;
    }
}
