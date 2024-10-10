using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Service
{
    public class DownloadService (IPlaylistService playlistService) : IDownloadService
    {
        public IPlaylistService _playlistService = playlistService;
        public string DownloadVideo(string folderPath, string args, string fileName)
        {
            Process ytdlp = new();
            ytdlp.StartInfo.FileName = "yt-dlp";
            
            ytdlp.StartInfo.Arguments = $"{args} -o {fileName} --max-filesize {Program.maxFileSize}M";

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
            while (ytdlp.HasExited == false){
                Thread.Sleep(100);
            }
            if(ytdlp.ExitCode != 0)
            {
                Console.WriteLine("An error occured while downloading the video");
                return "Error";
            }
            _playlistService.AddVideoToPlayList(folderPath);
            Task.Run(() => _playlistService.CheckPlayList());
            return "Video Added";
        }
    }
}
