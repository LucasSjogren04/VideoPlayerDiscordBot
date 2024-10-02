using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Service
{
    public class SettingsRegistration
    {
        public string? OutputDirectory { get; set; }
        public ulong GuildId { get; set; }
    }
}
