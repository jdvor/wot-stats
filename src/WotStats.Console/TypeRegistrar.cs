namespace WotStats.Console
{
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console.Cli;
    using System;

    internal sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection services;

        public TypeRegistrar(IServiceCollection services)
        {
            this.services = services;
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(services.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            services.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            services.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> func)
        {
            services.AddSingleton(service, provider => func());
        }
    }
}
