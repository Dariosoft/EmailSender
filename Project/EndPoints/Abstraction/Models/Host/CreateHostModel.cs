namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Host
{
    public record CreateHostModel
    {
        public string? ClientKey { get; set; }

        public required string Address { get; set; }

        public required int PortNumber { get; set; }

        public required bool Enabled { get; set; }

        public required bool UseSsl { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Address;
    }
}
