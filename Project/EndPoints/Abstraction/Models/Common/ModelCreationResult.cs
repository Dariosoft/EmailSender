namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record ModelCreationResult
    {
        public required string Key { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }
    }
}
