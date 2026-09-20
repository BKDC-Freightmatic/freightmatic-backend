namespace Freightmatic.Domain.News;

public interface INewsRepository
{
    Task<List<News?>> GetAllAsync();
}
