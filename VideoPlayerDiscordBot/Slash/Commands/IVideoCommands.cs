using Discord.WebSocket;

namespace VideoPlayerDiscordBot.Slash.Commands
{
    public interface IVideoCommands
    {
        Task AddVideo(SocketSlashCommand command);
    }
}