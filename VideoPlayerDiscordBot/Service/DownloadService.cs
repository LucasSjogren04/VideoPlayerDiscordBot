using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Service
{
    public class DownloadService : IDownloadService
    {
        public async Task<string> DownloadVideo(string path, string args)
        {
            Process ytdlp = new();
            ytdlp.StartInfo.FileName = "yt-dlp";
            
            ytdlp.StartInfo.Arguments = $"{args} -o {path} --max-filesize {Program.maxFileSize}";

            ytdlp.StartInfo.UseShellExecute = false;
            ytdlp.StartInfo.CreateNoWindow = true; 

            ytdlp.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            ytdlp.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
            try
            {
                ytdlp.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return "okay..";
        }
    }
}
