using System.Text;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using WhiteCatCoolOmegaMegaBot.Services;

namespace WhiteCatCoolOmegaMegaBot.Models;

using System.Timers;

public class BMain
{
    public ulong guildId { get; set; } = 0;
    public BMain()
    {
    }

    private static Timer aTimer;
    private DiscordSocketClient _discordSocketClient;
    private BServersService _bServersService;

    public void StartIntervalTimer(ref DiscordSocketClient dsc)
    {
        _discordSocketClient = dsc;
        _bServersService = new BServersService();

        aTimer = new System.Timers.Timer();
        aTimer.Interval = 60000;
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        if (guildId == 0) return;
        var guild = _discordSocketClient.GetGuild(guildId);
        var channels = guild.Channels;
        var firstChannel = channels.FirstOrDefault();
        if (firstChannel == null) return;
        var userChannels = firstChannel?.Users;
        
        foreach (var oneUser in userChannels)
        {
            var userUsername = oneUser.Username;
            var userCode = oneUser.Discriminator;

            var userActs = oneUser.Activities;
            foreach (var act in userActs)
            {
                //Console.WriteLine(act.Name);
                Console.WriteLine($"{guildId} - {userUsername} - {userCode} - {act.Name}");
                _bServersService.ContinueUserActivity(guildId, userUsername, userCode, act.Name, 1);
            }
        }
        
        // getting channels where user voice
        
        foreach (var oneChannel in channels)
        {
            if (oneChannel.GetChannelType() == ChannelType.Voice)
            {
                var thisChannel = oneChannel as SocketVoiceChannel;
                var voiceUsers = thisChannel?.ConnectedUsers;
                if (voiceUsers != null)
                {
                    foreach (var connectedUser in voiceUsers)
                    {
                        var username = connectedUser.Username;
                        var usercode = connectedUser.Discriminator;
                        _bServersService.ContinueUserActivity(guildId, username, usercode, $"{thisChannel?.Name}", 1);
                    }
                }
            }
        }
    }
    
    public async Task CreateCommandsIntegrations()
    {
        await AddCommand("activities", "Посмотреть активность пользователей в голосовых каналах и играх");
        await AddCommand("epilepsy", "Лучше не пробуй");
    }

    private async Task AddCommand(string commandName, string commandDesc)
    {
        var guild = _discordSocketClient.GetGuild(guildId);
        var guildCommand = new SlashCommandBuilder();
        guildCommand.WithName(commandName);
        guildCommand.WithDescription(commandDesc);
        try
        {
            var x1 = await guild.CreateApplicationCommandAsync(guildCommand.Build());
            
        }
        catch(ApplicationCommandException exception)
        {
            Console.WriteLine("err");
        }
    }
    
    public void StartEpelepsycTimer(ref DiscordSocketClient dsc)
    {
        _discordSocketClient = dsc;
        //var chan = dsc.GetChannel(1029809627044511798) as SocketGuildChannel;

        aTimer = new System.Timers.Timer();
        aTimer.Interval = 250;
        aTimer.Elapsed += Epelepsyyyy;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }

    private int epState = 0;
    private int epMax = 6;
    
    private void Epelepsyyyy(object? sender, ElapsedEventArgs e)
    {
        var chan = _discordSocketClient.GetChannel(1029809627044511798) as SocketGuildChannel;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < epState; i++) sb.Append('_');
        sb.Append('^');
        for (int i = 0; i < epMax - epState; i++) sb.Append('_');

        if (epState >= epMax) epState = 0;
        
        chan.ModifyAsync(p => p.Name = sb.ToString());
    }
}