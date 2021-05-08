using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Nejire.Hand
{
    public class CommandManager
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        private ILogger Logger;
        private IServiceProvider Services;

        public CommandManager(IServiceProvider services)
        {
            Commands = services.GetRequiredService<CommandService>();
            Logger = services.GetRequiredService<ILogger>();
            Services = services;
        }

        public async Task InitAsync()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            foreach (var command in Commands.Commands)
                Console.WriteLine($"{command.Name} carregado com sucesso.");
        }
    }
}
