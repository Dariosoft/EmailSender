namespace Dariosoft.EmailSender.Core.Models
{
    public record HostModel
    {
        public required Guid Id { get; set; }

        public required int Serial { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool UseSsl { get; set; }

        public required bool Enabled { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Address;
    }
}
