using Microsoft.Extensions.DependencyInjection;


namespace Dariosoft.Framework
{
    public static class ServiceCollection
    {
        #region AddSingleton<T>
        public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TImplementation : class, TService1, TService2
        {
            services.AddSingleton<TImplementation>()
                .AddSingleton<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(x => x.GetRequiredService<TImplementation>());

            return services;
        }

        public static IServiceCollection AddSingleton<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TImplementation : class, TService1, TService2, TService3
        {
            services.AddSingleton<TImplementation>()
                .AddSingleton<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService3>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }

        public static IServiceCollection AddSingleton<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TService4 : class
           where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddSingleton<TImplementation>()
                .AddSingleton<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService3>(x => x.GetRequiredService<TImplementation>())
                .AddSingleton<TService4>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }
        #endregion

        #region AddScoped<T>
        public static IServiceCollection AddScoped<TService1, TService2, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TImplementation : class, TService1, TService2
        {
            services.AddScoped<TImplementation>()
                .AddScoped<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService2>(x => x.GetRequiredService<TImplementation>());

            return services;
        }

        public static IServiceCollection AddScoped<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TImplementation : class, TService1, TService2, TService3
        {
            services.AddScoped<TImplementation>()
                .AddScoped<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService3>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }

        public static IServiceCollection AddScoped<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TService4 : class
           where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddScoped<TImplementation>()
                .AddScoped<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService3>(x => x.GetRequiredService<TImplementation>())
                .AddScoped<TService4>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }
        #endregion

        #region AddTransient<T>
        public static IServiceCollection AddTransient<TService1, TService2, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TImplementation : class, TService1, TService2
        {
            services.AddTransient<TImplementation>()
                .AddTransient<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService2>(x => x.GetRequiredService<TImplementation>());

            return services;
        }

        public static IServiceCollection AddTransient<TService1, TService2, TService3, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TImplementation : class, TService1, TService2, TService3
        {
            services.AddTransient<TImplementation>()
                .AddTransient<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService3>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }

        public static IServiceCollection AddTransient<TService1, TService2, TService3, TService4, TImplementation>(this IServiceCollection services)
           where TService1 : class
           where TService2 : class
           where TService3 : class
           where TService4 : class
           where TImplementation : class, TService1, TService2, TService3, TService4
        {
            services.AddTransient<TImplementation>()
                .AddTransient<TService1>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService2>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService3>(x => x.GetRequiredService<TImplementation>())
                .AddTransient<TService4>(x => x.GetRequiredService<TImplementation>())
                ;

            return services;
        }
        #endregion

        public static IServiceCollection RegisterOf<TServiceBase, TimplementationBase>(this IServiceCollection services, ServiceLifetime lifetime)
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
