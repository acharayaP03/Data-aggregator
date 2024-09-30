using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Globalization;
using System.Text;

public class TicketsAggregator
{
    private readonly string _ticketsFolder;

    //domain mapping
    private readonly Dictionary<string, CultureInfo> _domainToCultureMapping = new()
    {
        [".com"] = new CultureInfo("en-US"),
        [".fr"] = new CultureInfo("fr-FR"),
        [".jp"] = new CultureInfo("ja-JP"),
    };

    public TicketsAggregator(string ticketsFolder)
    {
        _ticketsFolder = ticketsFolder;
    }


    public void Run()
    {
        var stringBuilder = new StringBuilder();

        foreach (var filePath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {
            using PdfDocument document = PdfDocument.Open(filePath);
            Page page = document.GetPage(1);

            var lines = ProcessPage(page);
            stringBuilder.AppendLine(string.Join(Environment.NewLine, lines));

        }
        SaveTicketData(stringBuilder);
    }



    private IEnumerable<string> ProcessPage(Page page)
    {
        string text = page.Text;

        var split = text.Split(
            new[] { "Title:", "Date:", "Time:", "Visit us:" }, StringSplitOptions.None
            );

        // extract domain for culture mapping
        var domain = ExtractDomain(split.Last());
        var ticketCulture = _domainToCultureMapping[domain];

        for (int i = 1; i < split.Length - 3; i += 3)
        {
            yield return BuildTicket(split, i, ticketCulture);
        }
    }

    private string BuildTicket(string[] split, int i, CultureInfo ticketCulture)
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
        var outputFilePath = Path.Combine(_ticketsFolder, "aggregatedTickets.txt");
        File.WriteAllText(outputFilePath, stringBuilder.ToString());
        Console.WriteLine($"Results saved to {outputFilePath}");
    }

    private static string  ExtractDomain(string webAddress)
    {
        var lastDotIndex = webAddress.LastIndexOf('.');
        return webAddress.Substring(lastDotIndex);
    }
}