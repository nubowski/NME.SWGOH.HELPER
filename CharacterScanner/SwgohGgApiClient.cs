using Newtonsoft.Json;

namespace CharacterScanner;

public class SwgohGgApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://swgoh.gg/api/";

    public SwgohGgApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl)
        };
        
        // SetBearerToken(token);
    }

    public async Task<dynamic> GetAllCharacters()
    {
        return await GetAsync<dynamic>("characters/");
    }

    public async Task<dynamic> GetPlayerGacBracket()
    {
        return await GetAsync<dynamic>("player/921-868-851/");
    }

    private async Task<T> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred connecting to SWGOH.GG API: {e.Message}");
            throw;
        }
    }
    
    private void SetBearerToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}