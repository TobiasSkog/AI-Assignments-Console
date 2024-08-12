namespace AI_Assignments_Console.Assignment_1;
public static class Labb1Faq
{
    private static readonly Dictionary<string, string> Faq = new()
        {
            { "hours", "We are open from 8 AM to 10 PM, Monday through Sunday." },
            { "support", "You can reach our support team at support@tobyApiServices.ai." },
            { "location", "We are located at 1234 Main St, AI-Town, Canada." }
        };
    public static string AnswerFaq(string userQuestion, string languageIso6391)
    {
        foreach (var entry in Faq.Where(entry => userQuestion.Contains(entry.Key, StringComparison.OrdinalIgnoreCase)))
        {
            return entry.Value;
        }

        return "I'm sorry, I don't have an answer to that question.";
    }
}
