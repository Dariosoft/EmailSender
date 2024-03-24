namespace Dariosoft.EmailSender.EndPoint.Auth
{
    public struct ClientTokenModel
    {
        public ClientTokenModel(string? token)
        {
            Token = (token ?? "").Trim();
            var i = Token.IndexOf('@');
            if (i < 36)
            {
                ClientId = Guid.Empty;
                ApiKey = "";
            }
            else
            {
                ClientId = Guid.TryParse(Token[..i], out var clientId) ? clientId : Guid.Empty;
                ApiKey = Token[(i + 1)..];
            }
        }

        public ClientTokenModel(Guid clientId, string apiKey)
        {
            ApiKey = apiKey = apiKey ?? "";
            ClientId = clientId;
            Token = $"{clientId.ToString().ToLower()}@{apiKey}";
        }

        public string Token { get; set; }

        public Guid ClientId { get; }

        public string ApiKey { get; }

        public bool HasValue()
            => ClientId != Guid.Empty && !string.IsNullOrWhiteSpace(ApiKey);

        public override string ToString() => Token;

        public static implicit operator ClientTokenModel(string? token) => new ClientTokenModel(token);
    }
}
