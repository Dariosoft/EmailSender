namespace Dariosoft.EmailSender.Enums
{
    public enum MessageStatus : byte
    {
        Unknown = 0,
        Draft = 1,
        Pending = 2,
        Sending = 3,
        Failed = 4,
        Sent = 5,
        Canceled = 6
    }
}
