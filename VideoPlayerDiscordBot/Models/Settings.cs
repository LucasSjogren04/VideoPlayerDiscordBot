using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Models
{
    public class Settings
    {
        public required string Token { get; set; }
        public required ulong GuildId { get; set; }
        public required string OutputDirectory { get; set; }

    }
}
