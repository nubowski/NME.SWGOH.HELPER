using Newtonsoft.Json;

namespace CharacterScanner;

class Program
{
    static async Task Main(string[] args)
    {
        var sheetsService = new GoogleSheetsService();

        var apiToken = "";
        var swgohApiClient = new SwgohGgApiClient();
        
        var siteCrawler = new SiteCrawler();
        
        var spreadsheetId = "1AlenLhe2PheGJbYTKibsLJ9DHH-X00ifjHBmTKvhuSo";
        var range = "Geonosians"; // TODO: change to input data var

        while (true)
        {
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Process CSV Files");
            Console.WriteLine("2. Fetch SWGOH Characters");
            Console.WriteLine("3. Fetch and Parse Web Page");
            Console.WriteLine("4. Exit");
            
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
                    foreach (var file in files)
                    {
                        Console.WriteLine($"Processing file: {file}");
                        var data = ParserHelper.ParseCsvFile(file);
                        if (data.Count > 0)
                        {
                            sheetsService.AppendData(spreadsheetId, range, data);
                        }
                        else
                        {
                            Console.WriteLine("No data found in file.");
                        }
                    }
                    break;
                
                case "2":
                    Console.WriteLine("Fetching SWGOH Characters...");
                    var characters = swgohApiClient.GetPlayerGacBracket().Result;
                    Console.WriteLine(JsonConvert.SerializeObject(characters, Formatting.Indented));
                    break;
                
                case "3":
                    Console.WriteLine("Enter the URL to fetch and parse:");
                    var url = Console.ReadLine();
                    try
                    {
                        var htmlContent = await siteCrawler.FetchHtmlAsync(url);  // Correct usage of await
                        
                        if (htmlContent != null)
                        {
                            siteCrawler.ParseHtml(htmlContent);
                        }
                        else
                        {
                            Console.WriteLine("Failed to fetch HTML content or content was empty.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                    break;
                
                case "4":
                    return;
                
                default:
                    Console.WriteLine("Invalid option, try again.");
                    break;
            }
        }
    }
}