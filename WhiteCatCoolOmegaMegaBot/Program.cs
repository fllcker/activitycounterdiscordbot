using Discord;
using Discord.Commands;
using Discord.WebSocket;
using WhiteCatCoolOmegaMegaBot.Services;

namespace WhiteCatCoolOmegaMegaBot
{
    class Program
    {
        DiscordSocketClient client;
        CommandsService _commandsService;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            _commandsService = new CommandsService();
            
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            };
            client = new DiscordSocketClient(config);
            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            var token = "MTAyOTgwOTA2OTczODk2MzAyNA.GY-L8w.FnAzZFLDiIy5MJ9IvcByR2kB1HOZRdzQsFubB8";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            // voice id = 1029809627044511798
            // foreach (var user in channel.Users)
            // {
            //     Console.WriteLine(user.Username);
            // }

            Console.ReadLine();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandsHandler(SocketMessage msg)
        {
            if (!msg.Author.IsBot)
            {
                //Console.WriteLine(msg.Author.Username);
                //msg.Channel.SendMessageAsync($"{msg.Channel.Name} - {msg.Author} - {msg.Content}");
                //var user = client.GetUser("precision", "5206");
                //Console.WriteLine(msg.Content);

                var channel = client.GetChannel(1029809627044511798);
                var userChannels = channel.Users;

                if (channel.GetChannelType() == ChannelType.Voice)
                {
                    Console.Write("ITS VOICE");
                    var cl = channel as SocketVoiceChannel;
                    var connectedUsers = cl.ConnectedUsers;
                    foreach (var cUser in connectedUsers)
                    {
                        Console.WriteLine($"user connected: {cUser.Username}");
                    }
                }
                
                
                foreach (var oneUser in userChannels)
                {
                    var userUsername = oneUser.Username;
                    if (userUsername != null) Console.WriteLine(userUsername);
                    
                    var userActs = oneUser.Activities;
                    foreach (var act in userActs)
                    {
                        Console.WriteLine(act.Name);
                    }
                    
                }
                
                
                // commands
                var contentText = msg.Content.ToString();

                if (contentText[0] == '/')
                {
                    msg.Channel.SendMessageAsync(_commandsService.ExecuteCommand(contentText));
                }
            }
            return Task.CompletedTask;
        }
    }
}