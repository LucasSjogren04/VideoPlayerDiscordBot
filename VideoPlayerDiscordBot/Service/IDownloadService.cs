namespace VideoPlayerDiscordBot.Service
{
    public interface IDownloadService 
    {
        string DownloadVideo(string folderPath, string args, string fileName);
    }
}