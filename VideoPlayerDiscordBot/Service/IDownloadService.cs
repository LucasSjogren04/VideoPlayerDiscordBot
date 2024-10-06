namespace VideoPlayerDiscordBot.Service
{
    public interface IDownloadService 
    {
        Task<string> DownloadVideo(string folderPath, string args, string fileName);
    }
}