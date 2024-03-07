using System.Net.Mail;

namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Message
{
    public record UpdateMessageModel
    {
        public required string MessageKey { get; set; }

        public required string SenderAccountKey { get; set; }

        public required string Subject { get; set; }

        public bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public bool BodyIsHtml { get; set; }

        public MailPriority Priority { get; set; } = MailPriority.Normal;

        public Common.MailAddress? From { get; set; }

        public required Common.MailAddress[] To { get; set; }

        public Common.MailAddress[]? Cc { get; set; }

        public Common.MailAddress[]? Bcc { get; set; }

        public Common.MailAddress[]? ReplyTo { get; set; }

        public IDictionary<string, string>? Headers { get; set; }

        public override string ToString() => Subject;
    }
}
