namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record MailAddressCollectionItem : BaseTable
    {
        public required Guid CollectionId { get; set; }

        public required string Address { get; set; }

        public string? DisplayName { get; set; }

        public string? DisplayNameRAW { get; set; }

        public override string ToString() => Address;
    }
}
