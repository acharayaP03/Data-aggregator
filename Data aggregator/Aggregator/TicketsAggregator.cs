using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Globalization;
using System.Text;
using Data_aggregator.Utilities;

namespace Data_aggregator.Aggregator;

public class TicketsAggregator
{
    private readonly string _ticketsFolder;
    private readonly FileWriter _fileWriter;
    private readonly IDocumentsReader _documentsReader;

    //domain mapping
    private readonly Dictionary<string, CultureInfo> _domainToCultureMapping = new()
    {
        [".com"] = new CultureInfo("en-US"),
        [".fr"] = new CultureInfo("fr-FR"),
        [".jp"] = new CultureInfo("ja-JP"),
    };

    public TicketsAggregator(string ticketsFolder, FileWriter fileWriter, IDocumentsReader documentsReader)
    {
        _ticketsFolder = ticketsFolder;
        _fileWriter = fileWriter;
        _documentsReader = documentsReader;
    }


    public void Run()
    {
        var stringBuilder = new StringBuilder();

        var ticketDocuments = _documentsReader.Read(_ticketsFolder);

        foreach (var document in ticketDocuments)
        {
            var lines = ProcessDocument(document);
            stringBuilder.AppendLine(string.Join(Environment.NewLine, lines));

        }

        _fileWriter.Write(stringBuilder.ToString(), _ticketsFolder, "tickets.txt");
    }



    private IEnumerable<string> ProcessDocument(string document)
    {
 
        var split = document.Split(
            new[] { "Title:", "Date:", "Time:", "Visit us:" }, StringSplitOptions.None
            );

        // extract domain for culture mapping
        var domain = split.Last().ExtractDomain();
        var ticketCulture = _domainToCultureMapping[domain];

        for (int i = 1; i < split.Length - 3; i += 3)
        {
            yield return BuildTicket(split, i, ticketCulture);
        }
    }


    private static string BuildTicket(string[] split, int i, CultureInfo ticketCulture)
    {
        var title = split[i];
        var dateAsString = split[i + 1];
        var timeAsString = split[i + 2];

        var date = DateOnly.Parse(
            dateAsString,
            ticketCulture
        );

        var time = TimeOnly.Parse(
            timeAsString,
            ticketCulture
        );
        var dateAsStringInvariant = date.ToString(CultureInfo.InvariantCulture);
        var timeAsStringInvariant = time.ToString(CultureInfo.InvariantCulture);

        var ticketData = $"{title,-40}|{dateAsStringInvariant}|{timeAsStringInvariant}";


        return ticketData;
    }

    private void SaveTicketData(StringBuilder stringBuilder)
    {

    }
}

public interface IDocumentsReader
{
   IEnumerable<string> Read(string directory);
}

public class DocumentsReader : IDocumentsReader
{
    public IEnumerable<string> Read(string directory)
    {
        foreach (var filePath in Directory.GetFiles(directory, "*.pdf"))
        {
            using PdfDocument document = PdfDocument.Open(filePath);
            Page page = document.GetPage(1);
            yield return page.Text;
        }
    }
}


public interface IFileWriter
{
    void Write(string content, params string[] pathParts);
}


public class FileWriter : IFileWriter
{
    public void Write(string content, params string[] pathParts)
    {
        var resultFilePath = Path.Combine(pathParts);
        File.WriteAllText(resultFilePath, content);
        Console.WriteLine($"Results saved to {resultFilePath}");
    }
}