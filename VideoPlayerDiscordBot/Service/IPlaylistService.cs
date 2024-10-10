namespace VideoPlayerDiscordBot.Service
{
    public interface IPlaylistService
    {
        string AddVideoToPlayList(string fileName);
        void CheckPlayList();
        void StartVideo();
    }
}