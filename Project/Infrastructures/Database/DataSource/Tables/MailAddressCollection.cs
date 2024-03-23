namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record MailAddressCollection : FlaggedTable
    {
        public required Guid ClientId { get; set; }

        public required string Name { get; set; }

        public required string NameRAW { get; set; }

        public string? Description { get; set; }

        public string? DescriptionRAW { get; set; }

        public override string ToString() => NameRAW;
    }
}
