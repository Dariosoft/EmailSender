
namespace Dariosoft.EmailSender.Core.Models
{

    public record AccountModel : BaseModel
    {
        public required Guid ClientId { get; set; }

        public string ClientName { get; set; } = "";

        public required Guid HostId { get; set; }

        public string Host { get; set; } = "";

        public required bool Enabled { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => EmailAddress;
    }

    public record CreateAccountModel
    {
        public required KeyModel ClientKey { get; set; }

        public required KeyModel HostKey { get; set; }

        public required bool Enabled { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => EmailAddress;
    }

    public record UpdateAccountModel
    {
        public required KeyModel Key { get; set; }

        public required KeyModel ClientKey { get; set; }

        public required KeyModel HostKey { get; set; }

        public required bool Enabled { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => EmailAddress;
    }
}
