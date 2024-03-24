namespace Dariosoft.EmailSender.EndPoint.Auth
{
    public record UserIdentity : Framework.Auth.UserIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(Abstraction.Models.Client.ClientModel client)
        {
            this.Id = client.Id;
            this.Serial = client.Serial;
            this.UserName = $"client-{client.Serial}";
            this.DisplayName = client.Name;
            this.Reference = client.AccessKey;
            this.IsAuthenticated = true;
            this.UserIsAnonymouse = false;
        }

    }
}
