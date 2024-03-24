using System.Security.Claims;
using System.Security.Principal;

namespace Dariosoft.Framework.Auth
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(IUserIdentity? identity)
        {
            this.Identity = identity = identity ?? UserIdentityFactory.Anonymous();

            List<Claim> claims = new([
                new Claim(ClaimTypes.Authentication, identity.IsAuthenticated ? "true" : "false", ClaimValueTypes.Boolean),
                new Claim(ClaimTypes.PrimarySid, identity.Id.ToLower(), ClaimValueTypes.String),
                new Claim(ClaimTypes.SerialNumber, identity.Serial.ToString(), ClaimValueTypes.Integer32),
                new Claim(ClaimTypes.Name, identity.UserName, ClaimValueTypes.String),
                new Claim(ClaimTypes.GivenName, identity.DisplayName, ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, identity.Email, ClaimValueTypes.String),
                new Claim(ClaimTypes.Actor, Enum.GetName<Types.UserType>(identity.Type)!, ClaimValueTypes.String),
            ]);

            SetClaims(identity, claims);
            Claims = claims.ToArray();
            claims.Clear();
        }

        private void SetClaims(IUserIdentity identity, List<Claim> claims)
        {
            if (!string.IsNullOrWhiteSpace(identity.Reference))
                claims.Add(new Claim(CustomClaimTypes.UserReference, identity.Reference, ClaimValueTypes.String));

            if (identity.Organization is not null)
            {
                claims.AddRange([
                    new Claim(CustomClaimTypes.OrganizationId, identity.Organization.Id, ClaimValueTypes.String),
                    new Claim(CustomClaimTypes.OrganizationName, identity.Organization.Name, ClaimValueTypes.String),
                    ]);
            }

            if (identity.RegionalInfo is not null)
            {
                claims.AddRange([
                    new Claim(CustomClaimTypes.Culture, identity.RegionalInfo.Culture, ClaimValueTypes.String),
                    new Claim(CustomClaimTypes.CountryCode, identity.RegionalInfo.CountryCode, ClaimValueTypes.String),
                    ]);

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.Country))
                    claims.Add(new Claim(CustomClaimTypes.CountryName, identity.RegionalInfo.Country, ClaimValueTypes.String));

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.State))
                    claims.Add(new Claim(CustomClaimTypes.State, identity.RegionalInfo.State, ClaimValueTypes.String));

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.City))
                    claims.Add(new Claim(CustomClaimTypes.City, identity.RegionalInfo.City, ClaimValueTypes.String));

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.ZipCode))
                    claims.Add(new Claim(CustomClaimTypes.ZipCode, identity.RegionalInfo.ZipCode, ClaimValueTypes.String));

                if (identity.RegionalInfo.Longitude != null)
                    claims.Add(new Claim(CustomClaimTypes.Longitude, identity.RegionalInfo.Longitude.Value.ToString(), ClaimValueTypes.Double));

                if (identity.RegionalInfo.Latitude != null)
                    claims.Add(new Claim(CustomClaimTypes.Latitude, identity.RegionalInfo.Latitude.Value.ToString(), ClaimValueTypes.Double));

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.TimeZone))
                    claims.Add(new Claim(CustomClaimTypes.TimeZone, identity.RegionalInfo.TimeZone, ClaimValueTypes.String));

                if (!string.IsNullOrWhiteSpace(identity.RegionalInfo.Currency))
                    claims.Add(new Claim(CustomClaimTypes.Currency, identity.RegionalInfo.Currency, ClaimValueTypes.String));
            }

            if (identity.AccessRights is not null && identity.AccessRights.Length > 0)
            {
                claims.AddRange(
                    AccessRightSerializer.Instance
                    .Serialize(identity.AccessRights)
                    .Select(item => new Claim(CustomClaimTypes.AccessRight, item, ClaimValueTypes.String))
                    );
            }
        }

        public override IIdentity? Identity { get; }

        public override IEnumerable<Claim> Claims { get; }

        public static UserPrincipal Anonymouse() => new UserPrincipal(null);
    }
}
