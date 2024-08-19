using AI_Assignments_Console.Services;

namespace AI_Assignments_Console.Assignment_2;
public static class Labb2
{
    private static readonly AiServices Ai = new();

    public static async Task Run(int assignmentChoice)
    {
        await Ai.ConfigureAiServices(assignmentChoice);
    }
}