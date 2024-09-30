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
