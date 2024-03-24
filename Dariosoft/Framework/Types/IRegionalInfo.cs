namespace Dariosoft.Framework.Types
{
    public interface IRegionalInfo
    {
        string Culture { get; }

        string CountryCode { get; }

        string? Country { get; }

        string? State { get; }

        string? City { get; }

        string? ZipCode { get; }

        double? Longitude { get; }

        double? Latitude { get; }

        string? TimeZone { get; }

        string? Currency { get; }
    }

    public record RegionalInfo: IRegionalInfo
    {
        public string Culture { get; init; } = "";

        public string CountryCode { get; init; } = "";

        public string? Country { get; init; }

        public string? State { get; init; }

        public string? City { get; init; }

        public string? ZipCode { get; init; }

        public double? Longitude { get; init; }

        public double? Latitude { get; init; }

        public string? TimeZone { get; init; }

        public string? Currency { get; init; }

        public override string ToString() => Country ?? CountryCode;
    }
}
