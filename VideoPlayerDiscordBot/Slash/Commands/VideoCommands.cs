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

            // Start a new process to run ffmpeg
            Process ffmpegProcess = new();
            ffmpegProcess.StartInfo.FileName = "mpv"; // Since ffmpeg is in the PATH, you don't need to specify the full path
            ffmpegProcess.StartInfo.Arguments = args;

            // Redirect the standard output and error so you can capture or log them
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true; // Don't show a command prompt window

            // Subscribe to output and error data events if needed (optional)
            ffmpegProcess.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            ffmpegProcess.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

            try
            {
                // Start the ffmpeg process
                ffmpegProcess.Start();
                await command.RespondAsync();

                // Begin reading the output and error streams asynchronously
                ffmpegProcess.BeginOutputReadLine();
                ffmpegProcess.BeginErrorReadLine();

                // Wait for the process to exit
                ffmpegProcess.WaitForExit();

                // Optionally, check the exit code
                int exitCode = ffmpegProcess.ExitCode;
                Console.WriteLine($"mpv exited with code {exitCode}");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
