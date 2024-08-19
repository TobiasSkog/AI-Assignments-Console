using AI_Assignments_Console.Assignment_1;
using AI_Assignments_Console.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using ImageType = AI_Assignments_Console.Helpers.ImageType;

namespace AI_Assignments_Console.Services;

public class AiServices
{
    private string? _cogSvcEndpoint;
    private string? _cogSvcKey;
    private string? _cogSvcRegion;
    private int _labChoice;
    private const string TranslatorEndpoint = "https://api.cognitive.microsofttranslator.com";
    private static ComputerVisionClient? _cvClient;

    public async Task ConfigureAiServices(int assignmentChoice)
    {
        Console.Clear();
        _labChoice = assignmentChoice;
        _cogSvcKey = ValidationHelper.HiddenStringInput("We now need to setup the Key, Endpoint and Region for the AI services!\n\n" +
                                                        "Enter the Key for the Cognitive Services API: ");
        _cogSvcEndpoint = ValidationHelper.HiddenStringInput("Enter the Endpoint for the Cognitive Services API: ");
        _cogSvcRegion = ValidationHelper.HiddenStringInput("Enter the Region for the Cognitive Services API: ");
        switch (assignmentChoice)
        {
            case 1:
                await ProcessUserQuestion("Ocp-Apim-Subscription-Key");
                break;
            case 2:
                await GetImageUrlFromUser();
                break;
            default:
                Console.WriteLine("Invalid lab choice!");
                break;
        }
    }

    private async Task GetImageUrlFromUser()
    {
        _cvClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_cogSvcKey))
        {
            Endpoint = _cogSvcEndpoint
        };

        while (true)
        {
            var userChoice = ValidationHelper.GetFilePathToImage("Please enter the path to the image you want to analyze (-1 to quit): ");
            if (userChoice.url is "-1" or "")
            {
                return;
            }
            // Specify features to be retrieved
            List<VisualFeatureTypes?> features =
            [
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Objects
            ];

            // Get image type
            switch (userChoice.ImageType)
            {
                case ImageType.Local:
                    await AnalyzeLocalImage(userChoice.url, features);
                    await GetThumbnail(userChoice.url);

                    break;
                case ImageType.Online:
                    await AnalyzeOnlineImage(userChoice.url, features);
                    await GetThumbnailFromUrl(userChoice.url);

                    break;
                case ImageType.Exit:
                default:
                    Console.WriteLine("Invalid image type!");
                    return;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private static async Task AnalyzeLocalImage(string path, List<VisualFeatureTypes?> features)
    {
        using (var imageStream = File.OpenRead(path))
        {
            // Get image analysis
            var analysis = await _cvClient.AnalyzeImageInStreamAsync(imageStream, features);

            if (analysis.Description.Captions is { Count: > 0 })
            {
                // get image captions
                foreach (var caption in analysis.Description.Captions)
                {
                    Console.WriteLine($"Description: {caption.Text} (confidence: {caption.Confidence:P})");
                }
            }

            // Get image tags
            if (analysis.Tags is { Count: > 0 })
            {
                Console.WriteLine("Tags:");
                foreach (var tag in analysis.Tags)
                {
                    Console.WriteLine($" -{tag.Name} (confidence: {tag.Confidence:P})");
                }
            }

            // Get image categories
            List<LandmarksModel> landmarks = [];
            Console.WriteLine("Categories:");
            if (analysis.Categories is { Count: > 0 })
            {
                foreach (var category in analysis.Categories)
                {
                    // Print the category
                    Console.WriteLine($" -{category.Name} (confidence: {category.Score:P})");

                    // Get landmarks in this category
                    if (category.Detail?.Landmarks != null)
                    {
                        foreach (var landmark in category.Detail.Landmarks)
                        {
                            if (landmarks.All(item => item.Name != landmark.Name))
                            {
                                landmarks.Add(landmark);
                            }
                        }
                    }
                }
            }

            // If there were landmarks, list them
            if (landmarks is { Count: > 0 })
            {
                Console.WriteLine("Landmarks:");
                foreach (var landmark in landmarks)
                {
                    Console.WriteLine($" -{landmark.Name} (confidence: {landmark.Confidence:P})");
                }
            }

            // Get brands in the image
            if (analysis.Brands is { Count: > 0 })
            {
                Console.WriteLine("Brands:");
                foreach (var brand in analysis.Brands)
                {
                    Console.WriteLine($" -{brand.Name} (confidence: {brand.Confidence:P})");
                }
            }

            // Get objects in the image
            if (analysis.Objects is { Count: > 0 })
            {
                Console.WriteLine("Objects in image:");
                foreach (var detectedObject in analysis.Objects)
                {
                    Console.WriteLine($" -{detectedObject.ObjectProperty} (confidence: {detectedObject.Confidence:P})");
                }
            }
        }
    }
    private static async Task AnalyzeOnlineImage(string url, List<VisualFeatureTypes?> features)
    {

        // Get image analysis
        var analysis = await _cvClient.AnalyzeImageAsync(url, features);

        if (analysis.Description.Captions is { Count: > 0 })
        {
            // get image captions
            foreach (var caption in analysis.Description.Captions)
            {
                Console.WriteLine($"Description: {caption.Text} (confidence: {caption.Confidence:P})");
            }
        }

        // Get image tags
        if (analysis.Tags is { Count: > 0 })
        {
            Console.WriteLine("Tags:");
            foreach (var tag in analysis.Tags)
            {
                Console.WriteLine($" -{tag.Name} (confidence: {tag.Confidence:P})");
            }
        }

        // Get image categories
        List<LandmarksModel> landmarks = [];
        Console.WriteLine("Categories:");
        if (analysis.Categories is { Count: > 0 })
        {
            foreach (var category in analysis.Categories)
            {
                // Print the category
                Console.WriteLine($" -{category.Name} (confidence: {category.Score:P})");

                // Get landmarks in this category
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        if (landmarks.All(item => item.Name != landmark.Name))
                        {
                            landmarks.Add(landmark);
                        }
                    }
                }
            }
        }

        // If there were landmarks, list them
        if (landmarks is { Count: > 0 })
        {
            Console.WriteLine("Landmarks:");
            foreach (var landmark in landmarks)
            {
                Console.WriteLine($" -{landmark.Name} (confidence: {landmark.Confidence:P})");
            }
        }

        // Get brands in the image
        if (analysis.Brands is { Count: > 0 })
        {
            Console.WriteLine("Brands:");
            foreach (var brand in analysis.Brands)
            {
                Console.WriteLine($" -{brand.Name} (confidence: {brand.Confidence:P})");
            }
        }

        // Get objects in the image
        if (analysis.Objects is { Count: > 0 })
        {
            Console.WriteLine("Objects in image:");
            foreach (var detectedObject in analysis.Objects)
            {
                Console.WriteLine($" -{detectedObject.ObjectProperty} (confidence: {detectedObject.Confidence:P})");
            }
        }
    }
    private static async Task GetThumbnail(string imageFile)
    {
        Console.WriteLine("Generating thumbnail");

        using (var imageData = File.OpenRead(imageFile))
        {
            var thumbnailStream = await _cvClient.GenerateThumbnailInStreamAsync(100, 100, imageData, true);

            string thumbnailFileName = "thumbnail.png";
            using (Stream thumbnailFile = File.Create(thumbnailFileName))
            {
                thumbnailStream.CopyTo(thumbnailFile);
            }

            Console.WriteLine($"Thumbnail saved in {thumbnailFileName}");
        }
    }
    private static async Task GetThumbnailFromUrl(string imageUrl)
    {
        Console.WriteLine("Generating thumbnail");

        var thumbnailStream = await _cvClient.GenerateThumbnailAsync(100, 100, imageUrl, true);

        string thumbnailFileName = "thumbnail.png";
        using (Stream thumbnailFile = File.Create(thumbnailFileName))
        {
            thumbnailStream.CopyTo(thumbnailFile);
        }

        Console.WriteLine($"Thumbnail saved in {thumbnailFileName}");
    }

    private async Task AnswerUser(string userQuestion, (string LanguageName, string LanguageIso6391Name) language)
    {
        if (string.IsNullOrEmpty(language.LanguageIso6391Name))
        {
            Console.Write("Sorry, we currently do not support your language.\n" +
                          "Would you mind trying asking your question in a different language?\n\n" +
                          "(Y)es or (N)o: ");
            var choice = Console.ReadKey();
            if (choice.Key == ConsoleKey.Y)
            {
                Console.Clear();
                await ConfigureAiServices(_labChoice);
            }
        }

        var translatedQuestion = await TranslateText(userQuestion, language.LanguageIso6391Name);
        string answer;
        if (language.LanguageIso6391Name != "en")
        {
            var answerThatNeedsTranslation = Labb1Faq.AnswerFaq(translatedQuestion);
            answer = await TranslateText(answerThatNeedsTranslation, "en", language.LanguageIso6391Name);
        }
        else
        {
            answer = Labb1Faq.AnswerFaq(translatedQuestion);
        }

        Console.WriteLine(answer);
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
    private async Task<string> TranslateText(string textToTranslate, string languageIso6391, string toLanguage = "en")
    {
        if (languageIso6391 == "en" && toLanguage == "en")
        {
            return textToTranslate;
        }

        object[] body = [new { Text = textToTranslate }];
        var requestBody = JsonConvert.SerializeObject(body);
        using (var client = new HttpClient())
        {
            using (var request = new HttpRequestMessage())
            {
                // Build the request
                var path = "/translate?api-version=3.0&from=" + languageIso6391 + $"&to={toLanguage}";
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(TranslatorEndpoint + path);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _cogSvcKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", _cogSvcRegion);

                // Send the request and get response
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse JSON array and get translation
                var jsonResponse = JArray.Parse(responseContent);
                var translation = (string)jsonResponse[0]["translations"][0]["text"];
                return translation;
            }
        }
    }
    private async Task ProcessUserQuestion(string requestHeaders)
    {

        if (string.IsNullOrEmpty(_cogSvcEndpoint) || string.IsNullOrEmpty(_cogSvcKey) || string.IsNullOrEmpty(requestHeaders))
        {
            Console.WriteLine("You need to configure the AI services first!");
            return;
        }

        while (true)
        {
            Console.Clear();
            var userQuestion = ValidationHelper.GetString("Hello I'm Toby, I will be taking on your request today!\n" +
                                                          "Please describe what I can help you with today! (-1 to quit)\n");
            if (userQuestion == "-1")
            {
                return;
            }

            var language = await DetectLanguage(userQuestion, requestHeaders);

            await AnswerUser(userQuestion, language);
        }

    }
    private async Task<(string LanguageName, string LanguageIso6391Name)> DetectLanguage(string userQuestion, string requestHeaders)
    {
        var jsonBody = new JObject(
            new JProperty("documents",
                new JArray(
                    new JObject(
                        new JProperty("id", 1),
                        new JProperty("text", userQuestion)
                    ))));

        var utf8 = new UTF8Encoding(true, true);
        var encodedBytes = utf8.GetBytes(jsonBody.ToString());

        var client = new HttpClient();

        client.DefaultRequestHeaders.Add(requestHeaders, _cogSvcKey);
        var uri = $"{_cogSvcEndpoint}text/analytics/v3.1/languages?{HttpUtility.ParseQueryString(string.Empty)}";

        HttpResponseMessage response;

        using (var content = new ByteArrayContent(encodedBytes))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response = await client.PostAsync(uri, content);
        }

        if (response.StatusCode != HttpStatusCode.OK) return (string.Empty, string.Empty);
        var responseContent = await response.Content.ReadAsStringAsync();
        var results = JObject.Parse(responseContent);

        if (results["documents"] is not JArray { Count: > 0 } jArray) return (string.Empty, string.Empty);
        var language = (JObject)jArray[0];
        Console.WriteLine($"\t|| Detected Language: {language["detectedLanguage"]?["name"]}, {language["detectedLanguage"]?["iso6391Name"]} ||");
        return ((string)language["detectedLanguage"]?["name"]!, (string)language["detectedLanguage"]?["iso6391Name"]!);
    }
}

