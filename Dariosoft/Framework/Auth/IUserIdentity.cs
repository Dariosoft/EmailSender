using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Dariosoft.Framework.Auth
{
    public interface IUserIdentity : System.Security.Principal.IIdentity, IUserInfo
    {
        bool UserIsAnonymouse { get; }
    }

    public record UserIdentity : IUserIdentity
    {
        #region IUserIdentity
        public bool UserIsAnonymouse { get; init; } = true;
        #endregion

        #region IUserInfo
        public Types.UserType Type { get; init; } = Types.UserType.User;

        public string Id { get; init; } = "";

        public int Serial { get; init; }

        public string UserName { get; init; } = "";

        public string DisplayName { get; init; } = "";

        public string Email { get; init; } = "";

        public string? Reference { get; init; }

        public Types.IOrganizationInfo? Organization { get; init; }

        public Types.IRegionalInfo? RegionalInfo { get; init; }

        public IAccessRight[] AccessRights { get; init; } = [];
        #endregion

        #region System.Security.Principal.IIdentity
        public string? AuthenticationType { get; } = "Dariosoft";

        public bool IsAuthenticated { get; init; } = false;

        public string? Name => UserName;
        #endregion

        #region IEquatable
        public bool Equals(IUserInfo? other)
            => Equals(this, other);
        #endregion

        #region IEqualityComparer
        public bool Equals(IUserInfo? x, IUserInfo? y)
            => (x?.GetHashCode() ?? 0) == (y?.GetHashCode() ?? 0);

        public int GetHashCode([DisallowNull] IUserInfo obj)
           => obj.Id.ToString().ToLower().GetHashCode();
        #endregion
    }
}
