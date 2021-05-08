using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nejire.Modules.Commands
{
    public class Util : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong")]
        [Summary("Verifica a latência do bot!")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task PingCommand()
        {
            await Context.Channel.SendMessageAsync($"Ping é {Context.Client.Latency}");
        }

        [Command("teste")]
        [Alias("a")]
        [Summary("Verifica a latência do bot!")]
        public async Task TesteCommand()
        {
            await Context.Channel.SendMessageAsync($"Ping é {Context.Client.Latency}");
        }
    }
}
