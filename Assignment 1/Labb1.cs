using AI_Assignments_Console.Services;

namespace AI_Assignments_Console.Assignment_1;
public static class Labb1
{
    // I denna labb kommer du bygga en app där du använder dig av Natural Language Processing
    // och QnA i Azure AI. Du kan exempelvis utgå från det fiktiva caset att kundtjänsten på
    // ett företag vill kunna ge enkla svar oberoende av input-språk, och ge virtuell hjälp.

    // Användaren ska kunna skicka in information som bearbetas av AI-komponenten och
    // sedan presenteras resultatet för användaren.

    private static readonly AiServices Ai = new();

    public static async Task Run(int assignmentChoice)
    {

        await Ai.ConfigureAiServices(assignmentChoice);
    }
}
