using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Slash
{
    internal interface ISlashBuilder
    {
        Task SlashCommandHandler(SocketSlashCommand command);
        Task Client_Ready();
    }
}
