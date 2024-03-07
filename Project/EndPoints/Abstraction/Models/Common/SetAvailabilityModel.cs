namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record SetAvailabilityModel
    {
        public required string Key { get; set; }

        public required bool Enabled { get; set; }
    }
}
