using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Dariosoft.EmailSender.EndPoint.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddEndPointLayer(builder.Configuration)
                .AddHttpContextAccessor()
                .AddGrpc()
                .Services
                .AddAuthentication(defaultScheme: "Dariosoft")
                .AddScheme<EndPoint.Auth.AuthOptions, EndPoint.Auth.AuthenticationHandler>("Dariosoft", options => { })
                .Services
                .AddSingleton<IAuthorizationPolicyProvider, EndPoint.Auth.AuthorizationPolicyProvider>()
                .AddSingleton<IAuthorizationHandler, EndPoint.Auth.AuthorizationHandler>()
                .AddAuthorization();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<Services.ClientService>();
            app.MapGrpcService<Services.HostService>();
            app.MapGrpcService<Services.AccountService>();
            app.MapGrpcService<Services.MessageService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Lifetime.RegisterLifetimeDelegates(app.Services);

            app.Run();
        }
    }
}