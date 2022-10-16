namespace WhiteCatCoolOmegaMegaBot.Models;

public class BUserActivity
{
    public int Id { get; set; }
    public string NameOfActivity { get; set; }
    public string UserName { get; set; }
    public string UserCode { get; set; }
    public int Minutes { get; set; } = 0;
}