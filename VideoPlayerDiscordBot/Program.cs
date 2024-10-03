using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VideoPlayerDiscordBot.Slash;
using VideoPlayerDiscordBot.Slash.Commands;
using VideoPlayerDiscordBot.Service;
using VideoPlayerDiscordBot.Models;

namespace VideoPlayerDiscordBot
{
    public class Program
    {
        private ISlashBuilder? _slashBuilder;
        private DiscordSocketClient? _client;
        private CommandService? _commands;
        private IServiceProvider? _services;
        public static readonly List<Setting> settings =
        [
            new Setting {Name = "token" , Required = true, SettingValueType = "string"},
            new Setting {Name = "guildId" , Required = true, SettingValueType = "ulong"},
            new Setting {Name = "downloadPath" , Required = true, SettingValueType = "string"},
        ];
        public readonly static string[] settingLines = GetLinesInSettingFile();
        public readonly static ulong guildId = ulong.Parse(RegisterSetting(settings.Where(s => s.Name == "guildId").First()));
        public readonly static string downloadPath = RegisterSetting(settings.Where(s => s.Name == "downloadPath").First());

        public static void Main(string[] args)
        {
            Program program = new();
            program.MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            foreach(Setting setting in settings){
                if(setting.Name == "Error" && setting.Required == true){
                    return;
                }
            }

            var services = new ServiceCollection();
            ConfigureServices(services);

            _services = services.BuildServiceProvider();

            _client = _services.GetRequiredService<DiscordSocketClient>();

            _slashBuilder = _services.GetRequiredService<ISlashBuilder>();

            string token = RegisterSetting(settings.Where(s => s.Name == "token").First());
            if (token == "Error")
            {
                Console.WriteLine("Error finding token");
                return;
            }

            _client.Log += Log;

            _commands = _services.GetRequiredService<CommandService>();
            await RegisterCommandsAsync(_client, _commands);

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
            return Task.CompletedTask;
        }
        private static string[] GetLinesInSettingFile()
        {
            string path = GetSettingsFilePath();
            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 1 || lines == null)
            {
                string[] empty = [];
                return empty;
            }
            return lines;
        }
        private static string GetSettingsFilePath()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            if (filepath == null)
            {
                Console.WriteLine("Base path not found");
                return "Error";
            }
            string settingsFilePath = Path.Combine(filepath, "settings.txt");
            if (!File.Exists(settingsFilePath))
            {
                Console.WriteLine("Settings file not found");
                return "Error";
            }
            return settingsFilePath;
        }

        private static string RegisterSetting(Setting setting)
        {
            try
            {
                string line = settingLines.Where(l => l.StartsWith(setting.Name)).First();
                if (line == null)
                {
                    Console.WriteLine($"{setting.Name} not found in settings.txt file.");
                    return "Error";
                }
                if (line.StartsWith(setting.Name))
                {
                    int lenghtPlus1 = setting.Name.Length + 1;
                    string stringValue = line[lenghtPlus1..];
                    if (setting.SettingValueType == "string")
                    {
                        return stringValue;
                    }
                    else if (setting.SettingValueType == "bool")
                    {
                        bool result = bool.TryParse(stringValue, out bool boolValue);
                        if (!result)
                        {
                            Console.WriteLine($"Could not convert the provided {setting.Name} to variable type: {setting.SettingValueType}");
                            return "Error";
                        }
                        return stringValue;
                    }
                    else if (setting.SettingValueType == "ulong")
                    {
                        bool result = ulong.TryParse(stringValue, out ulong ulongValue);
                        if (!result)
                        {
                            Console.WriteLine($"Could not convert the provided {setting.Name} to variable type: {setting.SettingValueType}");
                            return "Error";
                        }
                        return stringValue;
                    }
                };
                return "Error";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }
        private static void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(provider => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.Guilds | GatewayIntents.MessageContent
            }));

            services.AddSingleton(new CommandService());
            services.AddSingleton<ISlashBuilder, SlashBuilder>();
            services.AddSingleton<IVideoCommands, VideoCommands>();
        }

        public async Task RegisterCommandsAsync(DiscordSocketClient client, CommandService commands)
        {
            if (_slashBuilder == null)
            {
                throw new Exception("Slash builder is null");
            }
            client.Ready += _slashBuilder.Client_Ready;
            client.SlashCommandExecuted += _slashBuilder.SlashCommandHandler;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

    }
};
