using Microsoft.Extensions.DependencyInjection;

namespace Dariosoft.EmailSender.EndPoint
{
    public static class ServiceProviderExtensions
    {
        public static IEnumerable<Framework.ILifetime> GetActiveLifetimes(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetServices<Framework.ILifetime>()
                 .Where(e => e.Enabled)
                 .OrderBy(e => e.Order);
        }
    }
}
