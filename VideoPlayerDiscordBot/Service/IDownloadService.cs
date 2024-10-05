namespace VideoPlayerDiscordBot.Service
{
    public interface IDownloadService 
    {
        Task<string> DownloadVideo(string path, string args);
    }
}