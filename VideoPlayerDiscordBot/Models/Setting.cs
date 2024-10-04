namespace VideoPlayerDiscordBot.Models
{
    public class Setting
    {
        public required string Name { get; set; }
        public bool Required { get; set; }
        public required string SettingValueType { get; set; }
        public string? Value { get; set; }
    }
}