using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;

namespace Data_aggregator.FileAccess;

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
