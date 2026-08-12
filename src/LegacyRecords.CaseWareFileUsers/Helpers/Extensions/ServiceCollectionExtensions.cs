using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using LegacyRecords.CaseWareFileUsers.HttpClients;
using LegacyRecords.CaseWareFileUsers.Options;
using LegacyRecords.CaseWareFileUsers.Wrappers;

namespace LegacyRecords.CaseWareFileUsers.Helpers.Extensions
{
    [ExcludeFromCodeCoverage(Justification = "There is nothing to test in this class at this point.")]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleAppServices(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            ConfigurationOptions configOptions)
        {
            // services
            // e.g. services.Add<ISomeService, SomeService>(serviceLifetime);

            // http clients
            services.AddHttpClient<IFakeApiHttpClient, FakeApiHttpClient>(
                client =>
                {
                    client.BaseAddress = new Uri(configOptions.FakeApi.BaseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("User-Agent", Constants.ApplicationTitle);
                });

            // wrappers
            services.Add<IHttpClientWrapper, HttpClientWrapper>(serviceLifetime);

            return services;
        }

        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime)
            where TService : class
            where TImplementation : class, TService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (!Enum.IsDefined(typeof(ServiceLifetime), serviceLifetime))
            {
                throw new InvalidEnumArgumentException(nameof(serviceLifetime), (int)serviceLifetime, typeof(ServiceLifetime));
            }

            return Add(services, typeof(TService), typeof(TImplementation), serviceLifetime);
        }

        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime)
            where TService : class
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!Enum.IsDefined(typeof(ServiceLifetime), serviceLifetime))
            {
                throw new InvalidEnumArgumentException(nameof(serviceLifetime), (int)serviceLifetime, typeof(ServiceLifetime));
            }

            return Add(services, typeof(TService), implementationFactory, serviceLifetime);
        }

        private static IServiceCollection Add(IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            if (!Enum.IsDefined(typeof(ServiceLifetime), lifetime))
            {
                throw new InvalidEnumArgumentException(nameof(lifetime), (int)lifetime, typeof(ServiceLifetime));
            }

            var serviceDescriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            services.Add(serviceDescriptor);

            return services;
        }

        private static IServiceCollection Add(
            IServiceCollection services,
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!Enum.IsDefined(typeof(ServiceLifetime), lifetime))
            {
                throw new InvalidEnumArgumentException(nameof(lifetime), (int)lifetime, typeof(ServiceLifetime));
            }

            var serviceDescriptor = new ServiceDescriptor(serviceType, implementationFactory, lifetime);
            services.Add(serviceDescriptor);

            return services;
        }
    }
}
