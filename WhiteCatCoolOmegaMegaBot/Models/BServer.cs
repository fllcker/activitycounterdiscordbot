namespace WhiteCatCoolOmegaMegaBot.Models;

public class BServer
{
    public int Id { get; set; }
    public ulong ServerId { get; set; }
    public string ServerName { get; set; }
    public List<BServerSetting> ServerSettings { get; set; }
    public List<BUserActivity> BUserActivities { get; set; }
}