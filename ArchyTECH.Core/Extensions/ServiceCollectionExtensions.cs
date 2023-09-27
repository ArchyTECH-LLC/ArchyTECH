#if NETCOREAPP

using ArchyTECH.Core.Caching;
using ArchyTECH.Core.EmbeddedResources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ArchyTECH.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static void AddArchyTechCore(this IServiceCollection services)
        {
            services.AddSingleton<ICache, Cache>();
            services.AddSingleton<IEmbeddedResourceService, EmbeddedResourceService>();
        }

        
        public static void AddConfigurationSection<TConfiguration>(
            this IServiceCollection services,
            IConfigurationSection configurationSection) where TConfiguration : class, new()
        {
            services.AddOptions();
            services.Configure<TConfiguration>(configurationSection);
            services.AddSingleton(provider =>
                provider.GetService<IOptions<TConfiguration>>().Value);
        }

        /// <summary>
        /// Automatically scan for classes that have an interface with the same name and tries to bind them with Scoped lifetime
        /// </summary>
        /// <typeparam name="TType">A type in the assembly you would like to scan and auto bind interfaces to types</typeparam>
        /// <param name="services">Your dependency services container</param>
        /// <returns></returns>
        public static IServiceCollection BindDefaultInterfacesFromAssemblyContainingType<TType>(this IServiceCollection services)
        {
            var assemblyTypes = typeof(TType).Assembly.GetTypes();
            var concreteTypes = assemblyTypes
                .Where(type => type.IsClass && type.IsAbstract == false);

            foreach (var concreteType in concreteTypes)
            {
                foreach (var @interface in concreteType.GetInterfaces())
                {
                    if ($"I{concreteType.Name}" == @interface.Name)
                        services.TryAddScoped(@interface, concreteType);
                }
            }

            return services;
        }
    }
}

#endif