using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nejire.Hand
{
    public class EventManager
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        private ILogger Logger;
        private IServiceProvider _Services;

        public EventManager(IServiceProvider Services)
        {
            Client = Services.GetRequiredService<DiscordSocketClient>();
            Commands = Services.GetRequiredService<CommandService>();
            Logger = Services.GetRequiredService<ILogger>();
            _Services = Services;
        }

        public Task InitAsync()
        {
            Client.Log += LogAsync;
            Commands.Log += CommandsLog;
            Client.Ready += ReadyAsync;
            Client.MessageReceived += MessageEvent;
            return Task.CompletedTask;
        }


        private async Task MessageEvent(SocketMessage arg)
        {
            var Message = arg as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if (Message.Author.IsBot || Message.Channel is IDMChannel) return;

            int ArgPos = 0;

            if (!(Message.HasStringPrefix(Config.Nejire.Prefix, ref ArgPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos))) return;

            var Result = await Commands.ExecuteAsync(Context, ArgPos, _Services);

            if (!Result.IsSuccess)
            {
                if (Result.Error == CommandError.UnknownCommand) return;
            }
        }

        private async Task ReadyAsync()
        {
            Console.WriteLine($"{Client.CurrentUser.Username} está conectado!");
            await Client.SetGameAsync($"da nada");
            await Client.SetStatusAsync(UserStatus.AFK);
        }

        private Task CommandsLog(LogMessage arg)
        {
            //Console.WriteLine($"Commands: [{arg.Source}] => {arg.Message}");
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage arg)
        {
            Console.WriteLine($"Log: [{arg.Source}] => {arg.Message}");
            return Task.CompletedTask;
        }
    }
}