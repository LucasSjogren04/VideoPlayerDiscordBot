using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayerDiscordBot.Models
{
    public class Video
    {
        public required string Id { get; set; }
        public bool Downloaded { get; set; }
    }
}
