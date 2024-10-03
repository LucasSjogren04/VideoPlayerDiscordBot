using Discord.Net;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerDiscordBot.Slash.Commands;

namespace VideoPlayerDiscordBot.Slash
{
    public class SlashBuilder(DiscordSocketClient client, IVideoCommands video) : ISlashBuilder
    {
        private readonly IVideoCommands _video = video;
        private readonly DiscordSocketClient _client = client;
        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            //if (command.ChannelId != 958698755169853460)
            //{
            //    return;
            //}
            switch (command.Data.Name)
            {
                case "add-video":
                    await _video.AddVideo(command);
                    break;
               
            }

        }

        public async Task Client_Ready()
        {
            ulong guidid = 1250182830701412384;
            //var guild = client.GetGuild(1250182830701412384);
            //var guildCommand = new SlashCommandBuilder()
            //   .WithName("a")
            //   .AddOption("video link", ApplicationCommandOptionType.String, "youtube link", isRequired: true);

            var addvideo = new SlashCommandBuilder()
                .WithName("add-video")
                .WithDescription("Zhing zhong ding dong")
                .AddOption("video-link", ApplicationCommandOptionType.String, "youtube link", isRequired: true);

            //var globalCommand = new SlashCommandBuilder();
            //globalCommand.WithName("first-global-command");
            //globalCommand.WithDescription("This is my first global slash command");
            //globalCommand.AddOption("video-link", ApplicationCommandOptionType.String, "youtube-link", isRequired: true);

            try
            {
                //await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                //await guild.CreateApplicationCommandAsync(guildCommand.Build());
                await _client.Rest.CreateGuildCommand(addvideo.Build(), guidid);
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    }
}
