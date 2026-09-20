using Freightmatic.Domain.Files;
using Microsoft.Extensions.Configuration;

namespace Freightmatic.Infrastructure.Files
{
    public class LocalFileRepository : IFileRepository
    {
        private readonly string _uploadPath;
        private readonly string _baseUrl;

        public LocalFileRepository(IConfiguration configuration)
        {
            _uploadPath = configuration["FileStorage:UploadPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            _baseUrl = configuration["FileStorage:BaseUrl"] ?? "http://localhost:4000/uploads";

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<List<Domain.Files.File>> Upload(IEnumerable<Domain.Files.File> files)
        {
            List<Domain.Files.File> uploadedFiles = new();

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file.Title);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(_uploadPath, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, file.Content);

                file.Url = $"{_baseUrl.TrimEnd('/')}/{fileName}";
                uploadedFiles.Add(file);
            }

            return uploadedFiles;
        }
    }
}
