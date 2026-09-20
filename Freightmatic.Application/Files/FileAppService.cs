using AutoMapper;
using Freightmatic.Domain.Files;

namespace Freightmatic.Application.Files
{
    public class FileAppService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        public FileAppService(IMapper mapper, IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }
        public async Task<List<FileDto>> Upload(List<FileUploadDto> files)
        {
            var domainFiles = files.Select(x => new Domain.Files.File(x.Title, string.Empty, x.Content)).ToList();
            var uploadedFiles = await _fileRepository.Upload(domainFiles);
            return _mapper.Map<List<FileDto>>(uploadedFiles);
        }
    }
}