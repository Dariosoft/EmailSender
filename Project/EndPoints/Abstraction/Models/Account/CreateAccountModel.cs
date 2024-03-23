namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Account
{
    public record CreateAccountModel
    {
        public required string ClientKey { get; set; }

        public required string HostKey { get; set; }

        public required bool Enabled { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? Description { get; set; }

        public override string ToString() => EmailAddress;
    }
}
