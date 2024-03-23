using Dariosoft.EmailSender.Application;

namespace Dariosoft.EmailSender.PostmanWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services
                .AddApplicationLayer(builder.Configuration)
                .AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}