namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record BaseModel
    {
        public required string Id { get; set; }

        public required int Serial { get; set; }

        public required DateTimeOffset CreationTime { get; set; }
    }
}
