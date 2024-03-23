using Dariosoft.EmailSender.EndPoint.gRPC.Services;

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
                .AddGrpc();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<ClientService>();
            app.MapGrpcService<HostService>();
            app.MapGrpcService<AccountService>();
            app.MapGrpcService<MessageService>();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Lifetime.RegisterLifetimeDelegates(app.Services);

            app.Run();
        }
    }
}