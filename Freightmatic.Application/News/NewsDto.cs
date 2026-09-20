using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Application.News;

public class NewsDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Key { get; set; }
}
