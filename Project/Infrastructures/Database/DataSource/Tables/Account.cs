namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record Account : FlaggedTable
    {
        public required Guid ClientId { get; set; }

        public required Guid HostId { get; set; }

        public required string EmailAddress { get; set; }

        public required string Password { get; set; }

        public string? DisplayName { get; set; }

        public string? DisplayNameRAW { get; set; }

        public string? Description { get; set; }

        public string? DescriptionRAW { get; set; }

        public override string ToString() => EmailAddress;
    }
}
