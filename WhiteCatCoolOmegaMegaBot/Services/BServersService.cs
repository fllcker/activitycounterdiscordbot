using WhiteCatCoolOmegaMegaBot.Data;
using WhiteCatCoolOmegaMegaBot.Models;
using System.Data.Entity.Migrations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WhiteCatCoolOmegaMegaBot.Services;

public class BServersService
{
    private ApplicationContext _applicationContext;

    public BServersService()
    {
        _applicationContext = new ApplicationContext();
    }

    public bool CreateIfNotExists(BServer bServer)
    {
        if (_applicationContext.BServers.Count(p => p.ServerId == bServer.ServerId) == 0)
        {
            _applicationContext.BServers.Add(bServer);
            _applicationContext.SaveChanges();
            return true;
        }

        return false;
    }

    public bool SetSettingToConfig(ulong serverId, string title, string value)
    {
        Console.WriteLine($"val {value}");
        var bServer = _applicationContext.BServers.FirstOrDefault(p => p.ServerId == serverId);
        if (bServer != null)
        {
            if (bServer.ServerSettings.Count(p => p.Title == title) != 0)
            {
                bServer.ServerSettings.FirstOrDefault(p => p.Title == title)!.Value = value;
                _applicationContext.SaveChanges();
            }
            else
            {
                
                //_applicationContext.BServers.FirstOrDefault(p => p.ServerId == serverId)!.ServerSettings.Add(setting);

                _applicationContext.BServerSetting.Add(new BServerSetting()
                {
                    Title = title,
                    Value = value,
                    BServerId = _applicationContext.BServers.FirstOrDefault(p => p.ServerId == serverId)!.Id
                });
                //bServer.ServerSettings.Add(setting);
                _applicationContext.SaveChanges();
            }

            return true;
        }

        return false;
    }

    public string GetSettingFromConfig(ulong serverId, string title)
    {
        var bServer = _applicationContext.BServers.FirstOrDefault(p => p.ServerId == serverId);
        if (bServer != null)
        {
            if (bServer.ServerSettings.FirstOrDefault(p => p.Title == title) != null)
            {
                return bServer.ServerSettings.FirstOrDefault(p => p.Title == title)!.Value;
            }

            return "";
        }

        return "";
    }

    public bool ContinueUserActivity(ulong serverId, string userName, string userCode, string activityName, int min = 1)
    {
        var bServer = _applicationContext.BServers.FirstOrDefault(p => p.ServerId == serverId);
        if (bServer == null) return false;

        var userActivity = new BUserActivity()
            { NameOfActivity = activityName, UserName = userName, UserCode = userCode, Minutes = min };


        if (_applicationContext.BServers
                .Include(p => p.BUserActivities)
                .FirstOrDefault(p => p.ServerId == serverId)!
                .BUserActivities
                .Where(p => p.UserName == userName)
                .Where(p => p.UserCode == userCode)
                .Count(p => p.NameOfActivity == activityName) == 0)
        {
            _applicationContext.BServers
                .Include(p => p.BUserActivities)
                .FirstOrDefault(b => b.ServerId == serverId)!
                .BUserActivities.Add(userActivity);
            _applicationContext.SaveChanges();
            return true;
        }
        else
        {
            var actv = _applicationContext.BServers
                .Include(p => p.BUserActivities)
                .FirstOrDefault(p => p.ServerId == serverId)
                ?.BUserActivities
                .Where(p => p.UserName == userName)
                .Where(p => p.UserCode == userCode)
                .FirstOrDefault(p => p.NameOfActivity == activityName);

            if (actv == null) return false;
            actv.Minutes += min;
            _applicationContext.SaveChanges();
            return true;
        }
        return true;
    }

    public string GetActivityTop(ulong serverId)
    {
        var bServer = _applicationContext.BServers
            .Include(p => p.BUserActivities)
            .FirstOrDefault(p => p.ServerId == serverId);
        if (bServer == null) return "";
        var allActivities = bServer.BUserActivities
            .OrderByDescending(p => p.Minutes);
        var activitiesGrouped = allActivities.GroupBy(p => p.NameOfActivity);
        
        StringBuilder sb = new StringBuilder();
        sb.Append("**Активность пользователей на этом сервере: **\n");
        sb.Append("```");
        foreach (var group in activitiesGrouped)
        {
            sb.Append("\n[" + group.Key + "]\n");
            var groupLimited = group.Take(3);
            foreach (var act in groupLimited)
            {
                sb.Append(act.UserName + "#"+ act.UserCode + " - " + act.Minutes + "(min)\n");
            }
        }
        sb.Append("```");

        return sb.ToString();
    }
}