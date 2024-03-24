using System.Security.Claims;

namespace Dariosoft.Framework.Auth
{
    public class UserIdentityFactory
    {
        public static IUserIdentity Anonymous()
           => new UserIdentity
           {
               Type = Types.UserType.Anonymous,
               Id = "",
               Serial = -1,
               Email = "anonymous@user.com",
               DisplayName = "Anonymous",
               UserName = "Anonymous",
               IsAuthenticated = false,
               UserIsAnonymouse = true,
           };

        public static IUserIdentity Create(IUserInfo? info)
            => info is null ? Anonymous() : new UserIdentity
            {
                Type = info.Type,
                Id = info.Id,
                Serial = info.Serial,
                Email = info.Email,
                DisplayName = info.DisplayName,
                UserName = info.UserName,
                IsAuthenticated = true,
                UserIsAnonymouse = false,
                Reference = info.Reference,
                Organization = info.Organization,
                RegionalInfo = info.RegionalInfo,
                AccessRights = info.AccessRights
            };

        public static IUserIdentity Create(ClaimsPrincipal? principal)
        {
            if (principal?.Identity is null || !principal.HasClaim(m => m.Type == ClaimTypes.Authentication && m.Value == "true"))
                return Anonymous();

            var id = GetUserId(principal);

            if (string.IsNullOrWhiteSpace(id))
                return Anonymous();

            return new UserIdentity
            {
                Id = id,
                Serial = Helpers.StringHelper.ParseToInt(principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value ?? ""),
                Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "",
                DisplayName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "",
                UserName = principal.Identity.Name!,
                IsAuthenticated = principal.Identity.IsAuthenticated,
                UserIsAnonymouse = false,
                Type = GetUserType(principal),
                Reference = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.UserReference)?.Value,
                Organization = GetOrganization(principal),
                RegionalInfo = GetRegionalInfo(principal),
                AccessRights = GetAccessRights(principal),
            };
        }

        private static string? GetUserId(ClaimsPrincipal principal)
            => principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value;

        private static Types.UserType GetUserType(ClaimsPrincipal principal)
        {
            var typeName = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;

           return string.IsNullOrWhiteSpace(typeName)
                ? Types.UserType.Anonymous
                : Helpers.EnumHelper.Instance.GetValue<Types.UserType>(typeName, Types.UserType.Anonymous);
        }

        private static Types.IOrganizationInfo? GetOrganization(ClaimsPrincipal principal)
        {
            var id = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.OrganizationId)?.Value ?? "";
            var name = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.OrganizationName)?.Value ?? "";

            return string.IsNullOrWhiteSpace(id)
                ? null
                : new Types.OrganizationInfo { Id = id, Name = name };
        }

        private static Types.IRegionalInfo? GetRegionalInfo(ClaimsPrincipal principal)
        {
            var culture = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Culture)?.Value ?? "";
            var countryCode = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.CountryCode)?.Value ?? "";

            if (string.IsNullOrWhiteSpace(culture) && string.IsNullOrWhiteSpace(countryCode))
                return null;

            return new Types.RegionalInfo
            {
                Culture = culture,
                CountryCode = countryCode,
                Country = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.CountryName)?.Value,
                State = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.State)?.Value,
                City = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.City)?.Value,
                ZipCode = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.ZipCode)?.Value,
                TimeZone = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.TimeZone)?.Value,
                Currency = principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Currency)?.Value,
                Latitude = Helpers.StringHelper.ParseToNullableDouble(principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Latitude)?.Value),
                Longitude = Helpers.StringHelper.ParseToNullableDouble(principal.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Longitude)?.Value),
            };
        }

        private static IAccessRight[] GetAccessRights(ClaimsPrincipal principal)
            => AccessRightSerializer.Instance.Deserialize(principal.Claims.Where(x => x.Type == CustomClaimTypes.AccessRight).Select(x => x.Value).ToArray());
    }
}
