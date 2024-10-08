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
        public List<Video>? Playlist { get; set; }
        public string AddVideoToPlayList(string fileName)
        {
            Video video = new()
            {
                Location = fileName,
                Playing = false
            };
            if (Playlist != null)
            {
                Playlist.Add(video);
            }
            else
            {
                Playlist = [new Video { Location = fileName }];
            }
            return "Video added";
        }
        public async Task<string> CheckPlayList()
        {
            while (true)
            {
                if (Playlist == null || Playlist.Count < 1)
                {
                    await Task.Delay(100);
                }
                else
                {
                    Playlist.First().Playing = true;
                    return "";
                }
            }
        }
        public async Task<string> StartVideo()
        {
            if (Playlist == null || Playlist.Count < 1)
            {
                return "Error";
            }
            string[] files = Directory.GetFiles(Playlist.First().Location);
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
            while (mpv.HasExited == false){
                Thread.Sleep(100);
            }
            Playlist.Remove(Playlist.Where(v => v.Playing == true).First());
            return "Video finnished";
        }

    }
}
