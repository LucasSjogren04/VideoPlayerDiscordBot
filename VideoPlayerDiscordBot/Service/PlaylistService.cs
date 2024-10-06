using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerDiscordBot.Models;

namespace VideoPlayerDiscordBot.Service
{
    public class PlaylistService : IPlaylistService
    {
        public List<Video>? PlayList { get; set; }
        public string AddVideoToPlayList(string pathToVideo)
        {
            return "";
        }
        public async Task<string> CheckPlayList()
        {
            Task task = Task.Run(async () =>
            {
                while (true)
                {
                    if (PlayList == null)
                    {
                        await Task.Delay(100);
                    }
                    else if (PlayList.Count() < 1)
                    {
                        await Task.Delay(100);
                    } else {
                        
                    }

                }
            });
        }
        public string StartVideo(string folder)
        {
            string[] files = Directory.GetFiles(folder);
            Process mpv = new();
            mpv.StartInfo.FileName = "mpv";

            mpv.StartInfo.Arguments = $"{files.First()}";

            mpv.StartInfo.UseShellExecute = false;
            mpv.StartInfo.CreateNoWindow = true;

            mpv.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            mpv.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
            try
            {
                mpv.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
