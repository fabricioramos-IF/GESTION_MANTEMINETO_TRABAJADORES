namespace Trabajadores.Web.Services
{
    public interface IFileUploadService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "trabajadores");
        Task<bool> DeleteImageAsync(string? filePath);
        bool ValidateImage(IFormFile file, out string errorMessage);
    }
}
