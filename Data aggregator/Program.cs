using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;

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

    public TicketsAggregator(string ticketsFolder)
    {
        _ticketsFolder = ticketsFolder;
    }


    public void Run()
    {

        foreach(var filePath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {            
            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                Page page = document.GetPage(1);
                string text = page.Text;
            }
        }
    }
}