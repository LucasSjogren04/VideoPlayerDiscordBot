using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Slash.Commands
{
    public class VideoCommands : IVideoCommands
    {

        public async Task AddVideo(SocketSlashCommand command)
        {
            // Define the arguments for ffmpeg (adjust these based on your use case)
            string args = (string)command.Data.Options.ToArray()[0];
            Console.WriteLine(args);

            Process mpvprocess = new();
            mpvprocess.StartInfo.FileName = "mpv"; 
            mpvprocess.StartInfo.Arguments = args;

            // Redirect the standard output and error so you can capture or log them
            //mpvprocess.StartInfo.RedirectStandardOutput = true;
            //mpvprocess.StartInfo.RedirectStandardError = true;
            mpvprocess.StartInfo.UseShellExecute = false;
            mpvprocess.StartInfo.CreateNoWindow = true; // Don't show a command prompt window

            // Subscribe to output and error data events if needed (optional)
            mpvprocess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            mpvprocess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            try
            {
  
                mpvprocess.Start();
                await command.RespondAsync("Video started");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
