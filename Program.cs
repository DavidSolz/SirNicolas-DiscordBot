using Discord;
using Discord.WebSocket;

public class Program
{

    private static DiscordSocketClient? client;

    private static Task Log(LogMessage message)
    {
        System.Console.WriteLine(message);
        return Task.CompletedTask;
    }

    private static string GetTokenFromFile(string? filename)
    {
        var lines = File.ReadLines("secret.env");

        Dictionary<string, string> keyValues = new();

        int lineNum = 1;
        foreach (var line in lines)
        {
            try
            {

                var tokens = line.Trim().Split('=');

                if (tokens.Length != 2)
                    throw new InvalidDataException($"Invalid data format in line: {lineNum}");

                keyValues.Add(tokens[0], tokens[1]);

            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                System.Console.WriteLine(e.Message);
                Console.ResetColor();
            }

            lineNum++;
        }

        var secret = keyValues
            .Where(x => x.Key == "SECRET_KEY")
            .Select(x => x.Value)
            .FirstOrDefault() ?? throw new InvalidDataException("SECRET_KEY does not exist in current context");
        return secret;
    }

    public static async Task Main(string[] args)
    {

        client = new DiscordSocketClient();

        client.Log += Log;

        try
        {
            var secret = GetTokenFromFile("secret.env");

            await client.LoginAsync(TokenType.Bot, secret);
            await client.StartAsync();

            await Task.Delay(-1);

        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            System.Console.WriteLine(e.Message);
            Console.ResetColor();
        }

    }

}
