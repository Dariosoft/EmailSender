using Dariosoft.EmailSender.EndPoint.Abstraction.Contracts;
using Dariosoft.EmailSender.EndPoint.gRPC.SDK;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dariosoft.ToHave
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddEmailSenderGrpcSDK(sp => new ConnectionInfo
            {
                EndPointAddress = "http://localhost:5039",
                ClientId = new Guid("0e479bb3-2422-47f4-b69c-36d41f8ab909"),
                ApiKey = "t9cvv6d41f474799ksdr423449wkxi7y46b8pbxk",
            });

            var app = builder.Build();

            Console.Write("Press any key to continue...");
           // Console.ReadKey();

            var hostEndPoint = app.Services.GetRequiredService<IHostEndPoint>();

            var response = await hostEndPoint.List(new EmailSender.EndPoint.Abstraction.Models.Common.ListQueryModel {  });

            
            Console.WriteLine("Hello, World!");
        }
    }

    class ConnectionInfo : IConnectionInfo
    {
        public string EndPointAddress { get; set; } = "";
        public Guid ClientId { get; set; }
        public string ApiKey { get; set; } = "";
    }
}
