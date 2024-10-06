namespace VideoPlayerDiscordBot.Service
{
    public interface IPlaylistService
    {
        string AddVideoToPlayList(string pathToVideo);
        Task<string> CheckPlayList();
        string StartVideo(string folder);
    }
}