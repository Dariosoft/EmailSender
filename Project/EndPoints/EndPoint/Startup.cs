using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Dariosoft.EmailSender.Application;

namespace Dariosoft.EmailSender.EndPoint
{
    public static class Startup
    {
        public static IServiceCollection AddEndPointLayer(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddApplicationLayer(configuration)
                .RegisterOf<Abstraction.Contracts.IEndPoint, EndPoints.EndPoint>(ServiceLifetime.Scoped);
        }


        private static IServiceCollection RegisterOf<TServiceBase, TimplementationBase>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            Type impBaseType = typeof(TimplementationBase), svcBaseType = typeof(TServiceBase);

            var interfaces = svcBaseType.Assembly
                .GetTypes()
                .Where(t => !t.IsClass && t.IsInterface && t != svcBaseType && t.IsAssignableTo(svcBaseType))
                .ToArray();

            var serviceDescriptors = impBaseType
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && t != impBaseType && t.IsAssignableTo(impBaseType))
                .SelectMany(t => interfaces
                    .Where(t.IsAssignableTo)
                    .Select(i => new ServiceDescriptor(serviceType: i, implementationType: t, lifetime: lifetime))
                    ).ToArray();


            for (int i = 0; i < serviceDescriptors.Length; i++)
                services.Add(serviceDescriptors[i]);

            return services;
        }
    }
}
