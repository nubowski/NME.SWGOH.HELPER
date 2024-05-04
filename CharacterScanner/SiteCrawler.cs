using HtmlAgilityPack;

namespace CharacterScanner;

public class SiteCrawler
{
    private readonly HttpClient _httpClient;

    public SiteCrawler()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> FetchHtmlAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var htmlContent = await response.Content.ReadAsStringAsync();
            return htmlContent;

        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while fetching the URL: {e.Message}");
            return null;
        }
    }

    public void ParseHtml(string rawContent)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawContent);
        
        // Example: Extract all links from the webpage
        var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                var hrefValue = node.GetAttributeValue("href", string.Empty);
                Console.WriteLine(hrefValue);
            }
        }
        else
        {
            Console.WriteLine("No links found on the page.");
        }
    }
}