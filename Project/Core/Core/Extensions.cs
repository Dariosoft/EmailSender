namespace Dariosoft.EmailSender
{
    public static class Extensions
    {
        public static Guid GetUserId(this Framework.Auth.IUserIdentity identity)
            => Guid.TryParse(identity.Id, out var id) ? id : Guid.Empty;
    }
}
