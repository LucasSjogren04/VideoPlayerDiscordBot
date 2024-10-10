using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VideoPlayerDiscordBot.Service;

namespace VideoPlayerDiscordBot.Slash.Commands
{
    public class VideoCommands(IValidationService validationService) : IVideoCommands
    {
        public IValidationService _validationService = validationService;

        public async Task AddVideo(SocketSlashCommand command)
        {
            string args = (string)command.Data.Options.ToArray()[0];
            string result = _validationService.ValidateLink(args);
            await command.RespondAsync(result);
            return;
        }
    }
}
