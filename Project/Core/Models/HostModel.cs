namespace Dariosoft.EmailSender.Core.Models
{

    public record HostModel : BaseModel
    {
        public Guid? ClientId { get; set; }

        public string? ClientName { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool UseSsl { get; set; }

        public required bool Enabled { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Address;
    }

    public record CreateHostModel
    {
        public KeyModel? ClientKey { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool Enabled { get; set; }

        public required bool UseSsl { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Address;
    }

    public record UpdateHostModel
    {
        public required KeyModel Key { get; set; }

        public KeyModel? ClientKey { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool UseSsl { get; set; }

        public required bool Enabled { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Address;
    }
}
