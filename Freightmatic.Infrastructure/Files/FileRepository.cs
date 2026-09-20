using Azure.Storage.Blobs;
using Freightmatic.Domain.Files;
using Microsoft.Extensions.Configuration;

namespace Freightmatic.Infrastructure.Files
{
    public class FileRepository : IFileRepository
    {
        private BlobContainerClient _containerClient;
        private readonly BlobServiceClient _blobServiceClient;

        public FileRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BlobStorage")!;
            _blobServiceClient = new BlobServiceClient(connectionString);
            Init().Wait();
        }

        public async Task Init()
        {
            _containerClient = await _blobServiceClient.CreateBlobContainerAsync("freightmatics");
        }

        public async Task<List<Domain.Files.File>> Upload(IEnumerable<Domain.Files.File> files)
        {
            List<Domain.Files.File> uploadedFiles = [];

            foreach (var file in files)
            {
                // Generate unique blob name using GUID and preserve file extension
                string extension = Path.GetExtension(file.Title);
                string blobName = Guid.NewGuid().ToString() + extension;
                
                // Get blob client for the specific file
                BlobClient blobClient = _containerClient.GetBlobClient(blobName);
                
                // Upload the file content
                using (var stream = new MemoryStream(file.Content))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                blobClient.SetMetadata(new Dictionary<string, string> { { "originalname", file.Title } });

                // Get the URL of the uploaded blob
                string blobUrl = blobClient.Uri.ToString();
                
                // Update file with URL and add to result list
                file.Url = blobUrl;
                uploadedFiles.Add(file);
            }

            return uploadedFiles;
        }
    }
}
