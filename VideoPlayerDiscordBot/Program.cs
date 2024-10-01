using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VideoPlayerDiscordBot.Slash;
using VideoPlayerDiscordBot.Slash.Commands;

public class Program
{
    private ISlashBuilder? _slashBuilder;
    private DiscordSocketClient? _client;
    private CommandService? _commands;
    private IServiceProvider? _services;

    public static void Main(string[] args)
    {
        // Create an instance of the Program class
        var program = new Program();
        // Call the instance method Main and wait for it to complete
        program.MainAsync(args[0]).GetAwaiter().GetResult();
    }

    public async Task MainAsync(string token)
    {

        var services = new ServiceCollection();
        ConfigureServices(services, token);

        // Build the service provider
        _services = services.BuildServiceProvider();

        _client = _services.GetRequiredService<DiscordSocketClient>();

        _slashBuilder = _services.GetRequiredService<ISlashBuilder>();

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

    private void ConfigureServices(IServiceCollection services, string token)
    {

        services.AddSingleton(provider => new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.Guilds | GatewayIntents.MessageContent 
        }));

        //var commandServiceConfig = new CommandServiceConfig { DefaultRunMode = RunMode.Async };
        services.AddSingleton(new CommandService());
        services.AddSingleton<ISlashBuilder, SlashBuilder>();
        services.AddSingleton<IVideoCommands, VideoCommands>();
        //services.AddSingleton<, GuessingCommands>();
    }

    public async Task RegisterCommandsAsync(DiscordSocketClient client, CommandService commands)
    {
        if(_slashBuilder == null)
        {
            throw new Exception("Slash builder is null");
        }
        client.Ready += _slashBuilder.Client_Ready;
        client.SlashCommandExecuted += _slashBuilder.SlashCommandHandler;

        // Here we discover all of the command modules in the assembly
        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
    }

}