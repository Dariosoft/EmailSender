using System;
using System.Net.Mail;

namespace Dariosoft.EmailSender.Core.Models
{
    public record MessageModel : BaseModel
    {
        public required DateTimeOffset LastStatusTime { get; set; }

        public required Guid AccountId { get; set; }

        public required string Subject { get; set; }

        public required bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public required bool BodyIsHtml { get; set; }

        public required MailPriority Priority { get; set; }

        public required Enums.MessageStatus Status { get; set; }

        public MailAddress? From { get; set; }

        public required MailAddress[] To { get; set; }

        public MailAddress[]? Cc { get; set; }

        public MailAddress[]? Bcc { get; set; }

        public MailAddress[]? ReplyTo { get; set; }

        public IDictionary<string, string>? Headers { get; set; }

        public override string ToString() => Subject;
    }

    public record CreateMessageModel
    {
        public required KeyModel SourceAccountKey { get; set; }

        public bool IsDraft { get; set; }

        public required string Subject { get; set; }

        public bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public bool BodyIsHtml { get; set; }

        public MailPriority Priority { get; set; } = MailPriority.Normal;

        public MailAddress? From { get; set; }

        public required MailAddress[] To { get; set; }

        public MailAddress[]? Cc { get; set; }

        public MailAddress[]? Bcc { get; set; }

        public MailAddress[]? ReplyTo { get; set; }

        public IDictionary<string, string>? Headers { get; set; }

        public override string ToString() => Subject;
    }

    public record UpdateMessageModel
    {
        public required KeyModel Key { get; set; }

        public required KeyModel SourceAccountKey { get; set; }

        public required string Subject { get; set; }

        public bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public bool BodyIsHtml { get; set; }

        public MailPriority Priority { get; set; } = MailPriority.Normal;

        public MailAddress? From { get; set; }

        public required MailAddress[] To { get; set; }

        public MailAddress[]? Cc { get; set; }

        public MailAddress[]? Bcc { get; set; }

        public MailAddress[]? ReplyTo { get; set; }

        public IDictionary<string, string>? Headers { get; set; }

        public override string ToString() => Subject;
    }
}
