using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Nejire
{
    public class CommandManager
    {
        private readonly DiscordSocketClient _Client;
        private readonly CommandService _Commands;
        private readonly IServiceProvider _services;

        public CommandManager(DiscordSocketClient Client, CommandService Commands, IServiceProvider services)
        {
            _Client = Client;
            _Commands = Commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _Commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _Commands.Log += CommandAsync;
            _Client.MessageReceived += Message_Event;
        }

        private Task CommandAsync(LogMessage Command)
        {
            Console.WriteLine(Command.Message);
            return Task.CompletedTask;
        }

        private async Task Message_Event(SocketMessage MessageA)
        {
            var Message = MessageA as SocketUserMessage;
            var Context = new SocketCommandContext(_Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;
            if (!(Message.HasStringPrefix(Config.Nejire.Prefix, ref ArgPos) || Message.HasMentionPrefix(_Client.CurrentUser, ref ArgPos))) return;

            var Result = await _Commands.ExecuteAsync(Context, ArgPos, null);
            if (!Result.IsSuccess && Result.Error != CommandError.UnknownCommand)
            {
                //Console.WriteLine($"{DateTime.Now} | Usou o comando: {_Commands.Search(Context, ArgPos).Commands[0].Command.Name}");
                var embed = new EmbedBuilder();

                if (Result.ErrorReason == "O texto de entrada tem muitos parâmetros!")
                {
                    embed.WithDescription("Comando não encontrado");
                }
                else
                {
                    embed.WithDescription("Ocorreu um erro:" + Result.ErrorReason);
                }

                await Context.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}
