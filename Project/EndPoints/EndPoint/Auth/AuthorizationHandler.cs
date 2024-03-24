using Microsoft.AspNetCore.Authorization;

namespace Dariosoft.EmailSender.EndPoint.Auth
{
    public class AuthorizationHandler : AuthorizationHandler<DynamicRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated == true)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        public override Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                /*var roleRequirements = context.PendingRequirements.OfType<Microsoft.AspNetCore.Authorization.Infrastructure.RolesAuthorizationRequirement>();
                foreach (var item in roleRequirements)
                {
                    item.AllowedRoles.Contains("");
                    context.Succeed(item);
                }*/

                foreach (var item in context.PendingRequirements)
                {
                    //context.Fail(new AuthorizationFailureReason(this, ""));
                    context.Succeed(item);
                }
            }
            return base.HandleAsync(context);
        }
    }
}
