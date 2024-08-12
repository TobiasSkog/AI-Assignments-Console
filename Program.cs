using AI_Assignments_Console.Assignment_1;
using AI_Assignments_Console.Assignment_2;
using AI_Assignments_Console.Helpers;
using System.Text;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;


Console.Clear();

Console.WriteLine("Welcome to Tobias AI Service Package!");
Console.WriteLine("You will now get a few options for what assignment you would like to test.\n");

var assignmentChoice = ValidationHelper.GetIntegerRange("1). Labb 1 -  Natural Language Processing och frågetjänster i Azure AI\n" +
                                                        "2). Labb 2 - Bildtjänster i Azure AI\n" +
                                                        "Choice: ", 1, 2);
Console.Clear();
switch (assignmentChoice)
{
    case 1:
        await Labb1.Run(assignmentChoice);
        break;
    case 2:
        await Labb2.Run(assignmentChoice);
        break;
    default:
        Console.WriteLine("No lab was chosen, terminating the software.");
        break;
}

Console.WriteLine("Thank you for using Tobias AI Service Package!");
Console.WriteLine("Goodbye!");