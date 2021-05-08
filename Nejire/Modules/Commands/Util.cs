using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

        [Command("avatar")]
        [Alias("pic", "photo", "av")]
        [Summary("Ver a foto do perfil de um usuário.")]
        public async Task AvatarCommand()
        {

            string avataruser;
            avataruser = Context.User.GetAvatarUrl(ImageFormat.Auto, 2048);

            await Context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                .WithColor(new Color(255, 0, 255))
                .WithAuthor("Avatar de " + Context.User.Username, avataruser)
                .WithImageUrl(avataruser)
                .Build());
        }
    }
}
