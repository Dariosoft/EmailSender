using System;
using System.Net.Mail;

namespace Dariosoft.EmailSender.Core.Models
{
    public record MessageModel : BaseModel
    {

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
}
