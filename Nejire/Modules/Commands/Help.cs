using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.Commands;
using Google.Protobuf.WellKnownTypes;

namespace Nejire.Modules.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _Services;

        public Help(CommandService Service)
        {
            _Services = Service;
        }

        [Command("ajuda")]
        [Alias("help", "cmds", "commands")]
        [Summary("Exive todos os comandos do bot.")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task HelpCmd([Remainder] string command = "")
        {
            string prefix = Config.Nejire.Prefix;
            if (command == "")
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(255, 0, 255))
                    //.WithAuthor(Context.User.Username, Context.User.GetAvatarUrl())
                    .WithThumbnailUrl(Context.User.GetAvatarUrl())
                    .WithTitle("Lista de Comandos!")
                    .WithCurrentTimestamp();

                foreach (var module in _Services.Modules)
                {
                    string description = "";
                    foreach(var cmd in module.Commands)
                    {
                        description += $"{prefix}{cmd.Aliases.First()}\n";
                    }

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        string name = module.Name;

                        embed.AddField(a =>
                        {
                            a.Name = name;
                            a.Value = description;
                            a.IsInline = false;
                        });
                        embed.WithCurrentTimestamp();
                    }
                }
                await ReplyAsync(embed: embed.Build());
            } else
            {
                var result = _Services.Search(Context, command);

                if (!result.IsSuccess)
                {
                    await ReplyAsync($"O comando {command} nao foi encontrado.");
                    return;
                }

                var embed = new EmbedBuilder()
                    .WithColor(new Color(255, 0, 255))
                    .WithThumbnailUrl(Context.User.GetAvatarUrl())
                    .WithCurrentTimestamp();

                foreach (var match in result.Commands)
                {
                    var cmd = match.Command;
                    embed.AddField(a =>
                    {
                        a.Name = $"{cmd.Name}";
                        a.Value = $"{(cmd.Aliases == null || cmd.Aliases.Count == 1 ? "" : string.Join(", ", cmd.Aliases.Where(alias => Array.IndexOf(cmd.Aliases.ToArray(), alias) != 0)))}";
                        a.IsInline = false;
                    });
                    embed.AddField(a =>
                    {
                        a.Name = $"Descrição";
                        a.Value = $"{cmd.Summary}";
                        a.IsInline = false;
                    });
                }

                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
