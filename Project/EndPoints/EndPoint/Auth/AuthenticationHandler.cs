using Dariosoft.EmailSender.EndPoint.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dariosoft.EmailSender.EndPoint.Auth
{

    public class AuthenticationHandler : AuthenticationHandler<AuthOptions>
    {
        private readonly Abstraction.Contracts.IClientEndPoint _organizationService;

        public AuthenticationHandler(
            Abstraction.Contracts.IClientEndPoint organizationService,
            IOptionsMonitor<AuthOptions> options,
            ILoggerFactory loggerFactory,
            ISystemClock systemClock)
            : base(options: options, logger: loggerFactory, encoder: System.Text.Encodings.Web.UrlEncoder.Default, clock: systemClock)
        {
            _organizationService = organizationService;
        }

        private ClientTokenModel GetToken()
        {

            var token = new string?[]
            {
                Request.Query["t"],
                Request.Cookies["Authorization"],
                Request.Headers["Authorization"].ToString()
            }.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e))?
            .Trim();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var i = token.IndexOf(' ');
                if (i > 0)
                    token = token[(i + 1)..];
            }

            return token;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = GetToken();

            if (token.HasValue())
            {
                var getClientResult = await _organizationService.Get(token.ClientId.ToString());

                if (getClientResult.Data is not null && getClientResult.Data.AccessKey == token.ApiKey)
                {
                    var identity = new EndPoint.Auth.UserIdentity(getClientResult.Data);

                    var principal = new Framework.Auth.UserPrincipal(identity);

                    var ticket = new AuthenticationTicket(principal, "Dariosoft");

                    return AuthenticateResult.Success(ticket);
                }

            }

            return AuthenticateResult.NoResult();
        }
    }
}
