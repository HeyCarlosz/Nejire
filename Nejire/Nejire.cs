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
using Nejire.Services;

namespace Nejire
{
    public class Nejire
    {
        private static DiscordSocketClient Client;
        private static CommandService Commands;
        private static IServiceProvider services;

        public Nejire()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Sync,
                LogLevel = LogSeverity.Info
            });
        }

        public async Task MainAsync()
        {
            CommandManager cmdmg = new CommandManager(Client, Commands, services);
            await cmdmg.InitializeAsync();

            Client.Ready += ReadyAsync;
            Client.Log += LogAsync;
            if (Config.Nejire.Token == "" || Config.Nejire.Token == null) return;
            await Client.LoginAsync(TokenType.Bot, Config.Nejire.Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task ReadyAsync()
        {
            Console.WriteLine($"{Client.CurrentUser.Username} está conectado!");
            await Client.SetGameAsync($"da nada");
            await Client.SetStatusAsync(UserStatus.AFK);
        }
    }
}
