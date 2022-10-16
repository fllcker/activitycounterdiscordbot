using Discord;
using Discord.WebSocket;
using WhiteCatCoolOmegaMegaBot.Models;
using WhiteCatCoolOmegaMegaBot.Services;

namespace WhiteCatCoolOmegaMegaBot
{
    class Program
    {
        DiscordSocketClient client;
        CommandsService _commandsService;
        BMain _bMain;
        BServersService _bServersService;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            _commandsService = new CommandsService();
            _bMain = new BMain();
            _bServersService = new BServersService();


            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All,
                UseInteractionSnowflakeDate = false
            };
            client = new DiscordSocketClient(config);
            client.MessageReceived += CommandsHandler;
            client.Log += Log;
            client.SlashCommandExecuted += SlashCommandHandler;


            var token = "";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            _bMain.StartIntervalTimer(ref client);
            Console.ReadLine();
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await command.RespondAsync(_commandsService.ExecuteSlashCommand("/" + command.Data.Name, ref client, ref command));
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task CommandsHandler(SocketMessage msg)
        {
            var ch = msg.Channel as SocketGuildChannel;
            var guildId = ch.Guild.Id;
            _bMain.guildId = guildId;
            await _bMain.CreateCommandsIntegrations();

            _bServersService.CreateIfNotExists(new BServer()
            {
                ServerId = guildId,
                ServerName = ch.Guild.Name
            });
            
            if (!msg.Author.IsBot)
            {
                // commands
                var contentText = msg.Content.ToString();

                // if (contentText[0] == '/')
                // {
                //     _commandsService.ExecuteCommand(contentText, ref client, ref msg);
                // }
                if (contentText == "/start")
                {
                    
                }
            }
            
        }
    }
}