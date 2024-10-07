namespace VideoPlayerDiscordBot.Service
{
    public interface IPlaylistService
    {
        string AddVideoToPlayList(string fileName);
        Task<string> CheckPlayList();
        Task<string> StartVideo();
    }
}