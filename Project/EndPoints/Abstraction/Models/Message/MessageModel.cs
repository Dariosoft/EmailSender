using System.Net.Mail;

namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Message
{
    public record MessageModel
    {
        public required string Id { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }

        public required string Subject { get; set; }

        public required bool SubjectIsHtml { get; set; }

        public required string Body { get; set; }

        public required bool BodyIsHtml { get; set; }

        public required MailPriority Priority { get; set; }

        public required Enums.MessageStatus Status { get; set; }

        public Common.MailAddress? From { get; set; }

        public required Common.MailAddress[] To { get; set; }

        public Common.MailAddress[]? Cc { get; set; }

        public Common.MailAddress[]? Bcc { get; set; }

        public Common.MailAddress[]? ReplyTo { get; set; }

        public IDictionary<string, string>? Headers { get; set; }

        public override string ToString() => Subject;
    }
}
