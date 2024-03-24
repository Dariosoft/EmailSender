namespace Dariosoft.Framework.Auth
{
    public interface IUserInfo : IEquatable<IUserInfo>, IEqualityComparer<IUserInfo>
    {
        Types.UserType Type { get; }

        string Id { get; }

        int Serial { get; }

        string UserName { get; }

        string DisplayName { get; }

        string Email { get; }

        string? Reference { get; }

        Types.IOrganizationInfo? Organization { get; }

        Types.IRegionalInfo? RegionalInfo { get; }

        IAccessRight[] AccessRights { get; }
    }
}
