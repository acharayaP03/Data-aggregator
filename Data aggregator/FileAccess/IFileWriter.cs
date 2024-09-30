namespace Data_aggregator.FileAccess;

public interface IFileWriter
{
    void Write(string content, params string[] pathParts);
}
