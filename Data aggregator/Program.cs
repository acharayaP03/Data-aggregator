using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Globalization;

Console.WriteLine("------------------------ Data Aggregator -----------------------------");


const string TicketsFolder = @"D:\dotnet\Data aggregator\Tickets";

try
{
    var ticketsAggregator = new TicketsAggregator(TicketsFolder);

    ticketsAggregator.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An exception occurred. Exception message:: {ex.Message}");
}


Console.WriteLine("Please press any key to ");



public class TicketsAggregator
{
    private readonly string _ticketsFolder;

    //domain mapping
    private readonly Dictionary<string, string> _domainToCultureMapping = new()
    {
        [".com"] = "en-US",
        [".fr"] = "fr-FR",
        [".jp"] = "ja-JP",
    };

    public TicketsAggregator(string ticketsFolder)
    {
        _ticketsFolder = ticketsFolder;
    }


    public void Run()
    {

        foreach(var filePath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {
            using PdfDocument document = PdfDocument.Open(filePath);
            Page page = document.GetPage(1);
            string text = page.Text;
            var split = text.Split(
                new[] {"Title:", "Date:", "Time:", "Visit us:"}, StringSplitOptions.None
                );

            // extract domain for culture mapping
            var domain = ExtractDomain(split.Last());
            var ticketCulture = _domainToCultureMapping[domain];

            for(int i = 1; i < split.Length - 3; i +=3)
            {
                var title = split[i];
                var dateAsString = split[i + 1];
                var timeAsString = split[i + 2];

                var date = DateOnly.Parse(
                    dateAsString,
                    new CultureInfo(ticketCulture)
                );

                var time = TimeOnly.Parse(
                    timeAsString,
                    new CultureInfo(ticketCulture)
                );
            }
        }
    }

    private static string  ExtractDomain(string webAddress)
    {
        var lastDotIndex = webAddress.LastIndexOf('.');
        return webAddress.Substring(lastDotIndex);
    }
}