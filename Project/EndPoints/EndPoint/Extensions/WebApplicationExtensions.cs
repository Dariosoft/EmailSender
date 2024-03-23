using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.EndPoint
{
    public static class WebApplicationExtensions
    {
        public static void RegisterLifetimeDelegates(this IHostApplicationLifetime app, IServiceProvider serviceProvider)
        {
            app.ApplicationStarted.Register(async stat =>
            {
                var sp = (stat as IServiceProvider)!;
                var tasks = sp.GetActiveLifetimes().Select(e => e.OnAppStarted());
                await Task.WhenAll(tasks);
            }, serviceProvider);

            app.ApplicationStopping.Register(async stat =>
            {
                var sp = (stat as IServiceProvider)!;
                var tasks = sp.GetActiveLifetimes().Select(e => e.OnAppStoping());
                await Task.WhenAll(tasks);
            }, serviceProvider);

            app.ApplicationStopped.Register(async stat =>
            {
                var sp = (stat as IServiceProvider)!;
                var tasks = sp.GetActiveLifetimes().Select(e => e.OnAppStopped());
                await Task.WhenAll(tasks);
            }, serviceProvider);
        }
    }
}
