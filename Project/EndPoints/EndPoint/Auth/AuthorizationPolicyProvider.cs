using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dariosoft.EmailSender.EndPoint.Auth
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = new AuthorizationPolicyBuilder("Dariosoft")
                   .RequireAuthenticatedUser()

                   // .AddRequirements(new DynamicRequirement { })
                   .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);

            // return base.GetPolicyAsync(policyName);
        }


    }
}
