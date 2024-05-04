using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace CharacterScanner;

public class GoogleSheetsService
{
    private SheetsService _service;
    private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
    private readonly string _applicationName = "Google Sheets API .NET Quickstart";
    
    public GoogleSheetsService()
    {
        InitializeService();
    }
    
    private void InitializeService()
    {
        using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
        var credPath = "token.json";

        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            _scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;

        Console.WriteLine($"Credential file saved to: {credPath}");

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName,
        });

        Console.WriteLine("Google Sheets service initialized successfully.");
    }

    public void AppendData(string spreadsheetId, string range, IList<IList<object>> values)
    {
        var valueRange = new ValueRange { Values = values };
        var appendRequest = _service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
        appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        var appendResponse = appendRequest.Execute();
        Console.WriteLine("Data updated successfully.");
    }
}