using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoPlayerDiscordBot.Service;

namespace VideoPlayerDiscordBot.Slash.Commands
{
    public class VideoCommands(IDownloadService downloadService) : IVideoCommands
    {
        public IDownloadService _downloadService = downloadService;

        public async Task AddVideo(SocketSlashCommand command)
        {
            string args = (string)command.Data.Options.ToArray()[0];
            if(!args.Contains("youtube.com") && !args.Contains("youtu.be"))
            {
                await command.RespondAsync($"{args} is not a valid link.");
            }
            Console.WriteLine(args);
            string pattern = @"=(.*)";

            Match match = Regex.Match(args, pattern);
            if (!match.Success)
            {
                await command.RespondAsync($"{args} is not a valid link.");
            }
            string fileName = match.Groups[1].Value;
            string filelocation = Path.Combine(Program.downloadPath ,fileName);
            Directory.CreateDirectory(filelocation);
            fileName = Path.Combine(filelocation, fileName);
            
            await command.DeferAsync();
            
            string result = await _downloadService.DownloadVideo(filelocation, args, fileName);
            await command.FollowupAsync(result);
            Console.WriteLine(result);
        }
    }
}
