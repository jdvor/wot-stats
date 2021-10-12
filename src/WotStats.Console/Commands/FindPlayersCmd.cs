namespace WotStats.Console.Commands
{
    using Spectre.Console.Cli;
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using WotStats.Interfaces;

    public sealed class FindPlayersCmd : Command<FindPlayersCmd.Settings>
    {
        private readonly IApiClient api;

        public FindPlayersCmd(IApiClient api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandOption("-m|--message <MESSAGE>")]
            [Description("Message to print")]
            public string Message { get; set; } = string.Empty;
        }
    }
}
