using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Slash.Commands
{
    public class VideoCommands : IVideoCommands
    {

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
            string filename = match.Groups[1].Value;

            string filelocation = "X:\\myapps\\botDownloads\\" + filename;


            Process ytdlp = new();
            ytdlp.StartInfo.FileName = "yt-dlp";
            
            ytdlp.StartInfo.Arguments = $"{args} -o {filelocation}";

            // Redirect the standard output and error so you can capture or log them
            //mpvprocess.StartInfo.RedirectStandardOutput = true;
            //mpvprocess.StartInfo.RedirectStandardError = true;
            ytdlp.StartInfo.UseShellExecute = false;
            ytdlp.StartInfo.CreateNoWindow = true; // Don't show a command prompt window

            // Subscribe to output and error data events if needed (optional)
            ytdlp.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            ytdlp.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            try
            {
  
                ytdlp.Start();
                await command.RespondAsync("Video started");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
