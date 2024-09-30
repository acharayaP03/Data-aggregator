namespace Data_aggregator.FileAccess;

public interface IDocumentsReader
{
    IEnumerable<string> Read(string directory);
}
