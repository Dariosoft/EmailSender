using System.Net.Mail;

namespace Dariosoft.EmailSender.Infrastructure.Database.DataSource.Tables
{
    internal record Message : FlaggedTable
    {
        public required Guid AccountId { get; set; }

        public required string Subject { get; set; }

        public required string SubjectRAW { get; set; }

        public required bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public required bool BodyIsHtml { get; set; }

        public required MailPriority Priority { get; set; }

        public required Enums.MessageStatus Status { get; set; }

        public required DateTime LastStatusTime { get; set; }

        public string? From { get; set; }

        public required Guid To { get; set; }

        public Guid? Cc { get; set; }

        public Guid? Bcc { get; set; }

        public Guid? ReplyTo { get; set; }

        public string? Headers { get; set; }

        public override string ToString() => Subject;
    }
}
