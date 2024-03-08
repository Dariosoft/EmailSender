namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models
{
    public record ResultMessage
    {
        public required string Text { get; set; }

        public string? Code { get; set; }

        public override string ToString() => Text;
    }
}
