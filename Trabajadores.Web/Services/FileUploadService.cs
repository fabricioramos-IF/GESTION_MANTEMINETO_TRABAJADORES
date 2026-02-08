namespace Trabajadores.Web.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool ValidateImage(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null || file.Length == 0)
            {
                errorMessage = "No se ha seleccionado ningún archivo";
                return false;
            }

            if (file.Length > _maxFileSize)
            {
                errorMessage = $"El archivo excede el tamaño máximo permitido de {_maxFileSize / 1024 / 1024}MB";
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                errorMessage = $"Formato no permitido. Solo se aceptan: {string.Join(", ", _allowedExtensions)}";
                return false;
            }

            return true;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder = "trabajadores")
        {
            try
            {
                if (!ValidateImage(file, out string errorMessage))
                {
                    return null;
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);
                
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return $"/images/{folder}/{uniqueFileName}";
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteImageAsync(string? filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return true;

                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
                
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
