namespace Data_aggregator.FileAccess;

public class FileWriter : IFileWriter
{
    public void Write(string content, params string[] pathParts)
    {
        var resultFilePath = Path.Combine(pathParts);
        File.WriteAllText(resultFilePath, content);
        Console.WriteLine($"Results saved to {resultFilePath}");
    }
}