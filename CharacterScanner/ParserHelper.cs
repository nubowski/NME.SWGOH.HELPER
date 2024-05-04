namespace CharacterScanner;

public static class ParserHelper
{
    public static List<IList<object>> ParseCsvFile(string path)
    {
        var result = new List<IList<object>>();

        try
        {
            using var reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                    
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                    
                // TODO: pass the separator later as an arg or default
                var values = line.Split(',');
                var rowList = new List<object>(values);
                result.Add(rowList);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while parsing the CSV file: {e.Message}");
            throw;
        }

        return result;
    }

    public static List<string> GetCsvFiles(string dirPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        try
        {
            var files = Directory.EnumerateFiles(dirPath, "*.csv", searchOption).ToList();
            return files;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while listing CSV files: {e.Message}");
            throw;
        }
    }
}