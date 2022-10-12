namespace WhiteCatCoolOmegaMegaBot.Services;

public class CommandsService
{
    public string ExecuteCommand(string commandText)
    {
        string[] commandAr = commandText.Split(" ");
        string commandName = commandAr[0].TrimStart('/');
        commandAr = commandAr.Where(val => val != commandAr[0]).ToArray();

        // commandName - commandName
        // commandArgs - commandAr[]
        switch (commandName)
        {
            case "random":
            {
                return $"Рандомное число (0-1000): {RandomNumber().ToString()}";
            }
        }

        return "";
    }

    public int RandomNumber(int min = 0, int max = 1000)
    {
        Random rnd = new Random();
        return rnd.Next(min, max);
    }
}