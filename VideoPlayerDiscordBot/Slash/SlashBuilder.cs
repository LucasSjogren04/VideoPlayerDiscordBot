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
            switch (command.Data.Name)
            {
                case "add-video":
                    await _video.AddVideo(command);
                    break;
               
            }

        }
        public async Task Client_Ready()
        {
            var addvideo = new SlashCommandBuilder()
                .WithName("add-video")
                .WithDescription("Downloads a video and puts it in queue to be played.")
                .AddOption("video-link", ApplicationCommandOptionType.String, "youtube link", isRequired: true);
            try
            {
                await _client.Rest.CreateGuildCommand(addvideo.Build(), Program.guildId);
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    }
}
