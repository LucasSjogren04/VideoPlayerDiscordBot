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
        public void CheckPlayList()
        {
            while (true)
            {
                if (Playlist == null || Playlist.Count < 1)
                {
                    Thread.Sleep(100);
                }
                else if(Playlist.Where(v => v.Playing == true).FirstOrDefault() != null)
                {
                    Thread.Sleep(100);
                }
                {
                    StartVideo();
                }
            }
        }
        public void StartVideo()
        {
             if (Playlist == null || Playlist.Count < 1)
            {
                Console.WriteLine("Play list empty");
                return;
            }

            Process[] processes = Process.GetProcesses();
            Process? process = processes.Where(p => p.ProcessName == "mpv").FirstOrDefault();
            if (process != null)
            {
                return;
            }
            else
            {
                if (Playlist.First().Playing == true)
                {
                    Playlist.Remove(Playlist.First());
                    return;
                }
                Playlist.First().Playing = true;
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
        }
    }
}
