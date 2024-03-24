
namespace Dariosoft.Framework.Auth
{

    public static class CustomClaimTypes
    {
        #region Regional Info
        public const string CountryCode = "https://dariosoft.com/identity/claims/region/country-code";
        public const string CountryName = "https://dariosoft.com/identity/claims/region/country-name";
        public const string State = "https://dariosoft.com/identity/claims/region/state";
        public const string City = "https://dariosoft.com/identity/claims/region/city";
        public const string ZipCode = "https://dariosoft.com/identity/claims/region/zipcode";
        public const string Longitude = "https://dariosoft.com/identity/claims/region/longitude";
        public const string Latitude = "https://dariosoft.com/identity/claims/region/latitude";
        public const string TimeZone = "https://dariosoft.com/identity/claims/region/timezone";
        public const string Culture = "https://dariosoft.com/identity/claims/region/culture";
        public const string Currency = "https://dariosoft.com/identity/claims/region/currency";
        #endregion

        #region Organization Info
        public const string OrganizationId = "https://dariosoft.com/identity/claims/organization/id";
        public const string OrganizationName = "https://dariosoft.com/identity/claims/organization/name";
        #endregion

        #region User Info
        public const string UserReference = "https://dariosoft.com/identity/claims/user/reference"; 
        #endregion

        public const string AccessRight = "https://dariosoft.com/identity/claims/access-right"; 
    }
}
