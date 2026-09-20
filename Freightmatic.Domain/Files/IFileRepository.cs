using Freightmatic.Domain.Shared.Interfaces;

namespace Freightmatic.Domain.Files
{
    public interface IFileRepository
    {
        public Task<List<File>> Upload(IEnumerable<File> files);
    }
}
