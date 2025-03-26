using Microsoft.AspNetCore.Http;

namespace RAGNET.Domain.Services
{
    public interface IPdfTextExtractorService
    {
        Task<string> ExtractTextAsync(IFormFile file);
    }
}