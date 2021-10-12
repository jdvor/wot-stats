namespace WotStats.Console
{
    using Microsoft.Extensions.DependencyInjection;
    using Spectre.Console.Cli;
    using WotStats.Console.Commands;
    using WotStats.Core;
    using WotStats.Interfaces;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandApp(BootstrapServices());
            app.Configure(opts =>
            {
                opts.SetApplicationName("spectbug");
                opts.SetApplicationVersion("1.0");

                opts.ValidateExamples();

                opts.AddCommand<FindPlayersCmd>("findplayer")
                    .WithDescription("Print message several times")
                    .WithExample(new[] { "findplayer", "-n", "iandr", "-r", "EU" });
            });

            app.Run(args);
        }

        private static TypeRegistrar BootstrapServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<FindPlayersCmd.Settings>();
            services.AddSingleton<FindPlayersCmd>();

            services.AddSingleton<IApiClient, ApiClient>();

            return new TypeRegistrar(services);
        }
    }
}
