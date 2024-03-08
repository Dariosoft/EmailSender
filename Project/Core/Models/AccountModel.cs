
namespace Dariosoft.EmailSender.Core.Models
{

    public record AccountModel
    {
        public required Guid Id { get; set; }

        public required int Serial { get; set; }

        public required string HostId { get; set; }

        public required bool Enabled { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => Username;
    }
}
