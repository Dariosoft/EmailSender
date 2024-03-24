namespace Dariosoft.Framework
{
    public record Reason
    {
        public required string Text { get; set; }

        public string? Code { get; set; }

        public override string ToString()
            => string.IsNullOrWhiteSpace(Code) ? Text : $"{Code}:{Text}";
    }
}
