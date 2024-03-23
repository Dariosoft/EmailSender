namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record MessageTrySendLog : BaseTable
    {
        public required Guid MessageId { get; set; }

        public required DateTime When { get; set; }

        public required Enums.MessageStatus Status { get; set; }

    }
}
