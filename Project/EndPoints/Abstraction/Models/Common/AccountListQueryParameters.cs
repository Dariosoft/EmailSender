namespace Dariosoft.EmailSender.EndPoint.Abstraction.Models.Common
{
    public record AccountListQueryParameters
    {
        public required string HostKey { get; set; }
    }
}
