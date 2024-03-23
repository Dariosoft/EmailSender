namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Account
{
    public record AccountModel
    {
        public required string Id { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }

        public required string ClientId { get; set; }
        public string ClientName { get; set; } = "";

        public required string HostId { get; set; }

        public string Host { get; set; } = "";

        public required bool Enabled { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => EmailAddress;
    }
}
