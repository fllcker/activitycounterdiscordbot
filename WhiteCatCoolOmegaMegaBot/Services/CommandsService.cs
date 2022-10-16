using Discord.WebSocket;
using WhiteCatCoolOmegaMegaBot.Models;

namespace WhiteCatCoolOmegaMegaBot.Services;

public class CommandsService
{
    private BServersService _bServersService = new BServersService();
    
    public string ExecuteCommand(string commandText, ref DiscordSocketClient? dsc, ref SocketMessage? sm)
    {
        string[] commandAr = commandText.Split(" ");
        string commandName = commandAr[0].TrimStart('/');
        commandAr = commandAr.Where(val => val != commandAr[0]).ToArray();
        if (sm == null) return "";

        // config command
        if (sm.Channel.Name == "cat-config")
        {
            if (commandName == "set" && commandAr.Length == 2)
            {
                var chs = sm.Channel as SocketGuildChannel;
                var guildId = chs.Guild.Id;
                if (_bServersService.SetSettingToConfig(guildId, commandAr[0], commandAr[1]))
                {
                    sm.Channel.SendMessageAsync($"Готово. {commandAr[0]} - {commandAr[1]}");
                }
                else sm.Channel.SendMessageAsync($"Ошибка");
            }
        }
        
        // commandName - commandName
        // commandArgs - commandAr[]
        switch (commandName)
        {
            case "random":
            {
                sm.Channel.SendMessageAsync($"Рандомное число (0-1000): {RandomNumber().ToString()}");
                return "";
            }
            case "activity":
            {
                var chs = sm.Channel as SocketGuildChannel;
                var guildId = chs.Guild.Id;
                sm.Channel.SendMessageAsync(_bServersService.GetActivityTop(guildId));
                return "";
            }
        }

        return "";
    }
    
    public string ExecuteSlashCommand(string commandText, ref DiscordSocketClient? dsc, ref SocketSlashCommand? sm)
    {
        string[] commandAr = commandText.Split(" ");
        string commandName = commandAr[0].TrimStart('/');
        commandAr = commandAr.Where(val => val != commandAr[0]).ToArray();
        if (sm == null) return "";
        
        switch (commandName)
        {
            case "activities":
            {
                var chs = sm.Channel as SocketGuildChannel;
                var guildId = chs.Guild.Id;
                //sm.Channel.SendMessageAsync();
                return _bServersService.GetActivityTop(guildId);
            }
            case "epilepsy":
            {
                //var chan = dsc.GetChannel(1029809627044511798) as SocketGuildChannel;

                //chan.ModifyAsync(p => p.Name = "");
                //StartEpelepsycTimer();
                new BMain().StartEpelepsycTimer(ref dsc);
                return "done, lets fun!";
            }
        }

        return "err";
    }

    public int RandomNumber(int min = 0, int max = 1000)
    {
        Random rnd = new Random();
        return rnd.Next(min, max);
    }
}