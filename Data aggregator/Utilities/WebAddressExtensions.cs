namespace Data_aggregator.Utilities;

public static class WebAddressExtensions
{
    public static string ExtractDomain(this string webAddress)
    {
        var lastDotIndex = webAddress.LastIndexOf('.');
        return webAddress.Substring(lastDotIndex);
    }
}