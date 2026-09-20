using AutoMapper;
using Freightmatic.Domain.News;

namespace Freightmatic.Application.News;
public class NewsAppService
{
    private readonly INewsRepository _newsRepository;
    private readonly IMapper _mapper;

    public NewsAppService(IMapper mapper, INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NewsDto>> GetAll()
    {
        List<Domain.News.News?> news = await _newsRepository.GetAllAsync();
        return news.Select(x => _mapper.Map<NewsDto>(x));
    }
}
