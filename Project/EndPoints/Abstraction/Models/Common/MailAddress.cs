namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record MailAddress
    {
        public required string Address { get; set; }

        public string? DisplayName { get; set; }

        public override string ToString()
            => string.IsNullOrWhiteSpace(DisplayName) ? Address : $"{Address};{DisplayName}";
    }
}
