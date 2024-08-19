using System.Globalization;
using System.Net;

namespace AI_Assignments_Console.Helpers;
public enum ImageType
{
    Local,
    Online,
    Exit
}

public static class ValidationHelper
{
    public static (ImageType ImageType, string url) GetFilePathToImage(string question)
    {
        Console.Clear();
        while (true)
        {
            var isImageUrlFromWebsite = false;
            Console.WriteLine(question + "\nOr type \"Back\" to go back.\n");
            Console.Write("User: ");
            var path = Console.ReadLine()!;

            if (path == "Back")
            {
                return (ImageType.Exit, "");
            }
            if (path[0] == '\"' && path[^1] == '\"')
            {
                path = path[1..^1];
            }
            //if (path.StartsWith("http") || path.StartsWith("www"))
            //{
            isImageUrlFromWebsite = IsImageUrl(path);
            //}
            if (!isImageUrlFromWebsite && File.Exists(path) || isImageUrlFromWebsite)
            {
                return (isImageUrlFromWebsite ? ImageType.Online : ImageType.Local, path);
            }

            Console.WriteLine(isImageUrlFromWebsite
                ? "Website url is invalid. Please enter a valid url to your image file."
                : "File do not exist. Please enter a valid path to your image file.");
        }
    }

    public static bool IsImageUrl(string url)
    {
        var req = (HttpWebRequest)HttpWebRequest.Create(url);
        req.Method = "HEAD";
        using (var resp = req.GetResponse())
        {
            return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                .StartsWith("image/");
        }
    }
    public static string GetString(string prompt, string errorMessage = "Invalid input.\nYou cannot use an empty text or only white spaces.")
    {
        while (true)
        {
            Console.Write(prompt);

            var userInput = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(userInput))
            {
                Console.Clear();
                return userInput;
            }
            Console.Clear();
            Console.WriteLine($"{errorMessage}\n");
        }
    }

    public static int GetIntegerRange(string prompt, int minRange, int maxRange, string errorMessage = $"Invalid input.\nNumber must be an integer within the range")
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int validInt))
            {
                if (validInt >= minRange && validInt <= maxRange)
                {
                    Console.Clear();
                    return validInt;
                }
            }
            Console.Clear();
            Console.WriteLine($"{errorMessage} (min: {minRange} - max: {maxRange}).\n");
        }
    }

    public static string HiddenStringInput(string prompt, bool showHiddenOutput = false)
    {
        Console.Clear();

        string userInput = "";
        Console.Write(prompt);

        while (true)
        {

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                userInput += key.KeyChar;
                if (!showHiddenOutput)
                {
                    if (!string.IsNullOrEmpty(userInput))
                    {
                        Console.Write((char)2534);
                    }
                }
            }
            else
            {
                if (key.Key == ConsoleKey.Backspace && userInput.Length > 0)
                {
                    userInput = userInput.Substring(0, userInput.Length - 1);
                    if (showHiddenOutput)
                    {
                        Console.Write("\b \b");
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (string.IsNullOrEmpty(userInput))
                    {
                        Console.WriteLine("\nEmpty value not allowed.");
                        userInput = "";
                    }
                    else
                    {
                        return userInput;
                    }
                }
            }
        }
    }


}
