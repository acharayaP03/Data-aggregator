Console.WriteLine("------------------------ Data Aggregator -----------------------------");


const string TicketsFolder = @"";

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
        throw new NotImplementedException();
    }
}