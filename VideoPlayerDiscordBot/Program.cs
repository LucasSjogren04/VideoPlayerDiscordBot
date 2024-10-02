using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VideoPlayerDiscordBot.Slash;
using VideoPlayerDiscordBot.Slash.Commands;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    private ISlashBuilder? _slashBuilder;
    private DiscordSocketClient? _client;
    private CommandService? _commands;
    private IServiceProvider? _services;

    public static void Main(string[] args)
    {
        var program = new Program();
        program.MainAsync().GetAwaiter().GetResult();
    }

    public async Task MainAsync()
    {

        var services = new ServiceCollection();
        ConfigureServices(services);

        // Build the service provider
        _services = services.BuildServiceProvider();

        _client = _services.GetRequiredService<DiscordSocketClient>();

        _slashBuilder = _services.GetRequiredService<ISlashBuilder>();

        string token = RegisterSecrets();
        if(token == "Error")
        {
            Console.WriteLine("Error finding token");
            return;
        }

        _client.Log += Log;

        // Resolve and register commands
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

    private string RegisterSecrets()
    {
        string filepath = AppDomain.CurrentDomain.BaseDirectory;
        if(filepath == null)
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
        Console.WriteLine(settingsFilePath);
        
        try
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("token:"))
                {
                    return line.Split(new[] { "token:" }, StringSplitOptions.None)[1].Trim();
                }
            }
        } catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return "Settings file not found";
    }
    private void ConfigureServices(IServiceCollection services)
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
        if(_slashBuilder == null)
        {
            throw new Exception("Slash builder is null");
        }
        client.Ready += _slashBuilder.Client_Ready;
        client.SlashCommandExecuted += _slashBuilder.SlashCommandHandler;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

}