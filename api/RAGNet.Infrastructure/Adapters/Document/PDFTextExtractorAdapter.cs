using RAGNET.Domain.Services;
using UglyToad.PdfPig;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace RAGNET.Infrastructure.Adapters.Document
{
    public class PdfTextExtractorAdapter : IPdfTextExtractorService
    {
        public async Task<string> ExtractTextAsync(IFormFile file)
        {
            // Open the PDF stream
            using var stream = file.OpenReadStream();
            var sb = new StringBuilder();

            // Wraps the result in Task.FromResult.
            using (var pdf = PdfDocument.Open(stream))
            {
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }
            return await Task.FromResult(sb.ToString());
        }
    }
}
