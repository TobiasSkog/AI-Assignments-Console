namespace AI_Assignments_Console.Helpers;
public static class ValidationHelper
{
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
