namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record MessageListQueryParameters
    {
        public required string AccountKey { get; set; }
    }
}
