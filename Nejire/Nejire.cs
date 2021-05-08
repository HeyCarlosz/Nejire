using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Nejire.Hand;
using Serilog;

namespace Nejire
{
    public class Nejire
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        private IServiceProvider Services;

        public Nejire()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                DefaultRetryMode = RetryMode.Retry502,
                ExclusiveBulkDelete = true,
                AlwaysDownloadUsers = false,
                MessageCacheSize = 0,
                LargeThreshold = 50,
                GatewayIntents = GatewayIntents.Guilds |
                    GatewayIntents.GuildMembers |
                    GatewayIntents.GuildVoiceStates |
                    GatewayIntents.GuildMessages
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Sync,
                LogLevel = LogSeverity.Info
            });

            Services = new ServiceCollection()

                .AddSingleton(Client)
                .AddSingleton(Commands)
                .AddSingleton(Log.Logger)
                .BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            await new CommandManager(Services).InitAsync();
            await new EventManager(Services).InitAsync();

            if (Config.Nejire.Token == "" || Config.Nejire.Token == null) return;
            await Client.LoginAsync(TokenType.Bot, Config.Nejire.Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
