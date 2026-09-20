using Freightmatic.Domain.Shared;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Domain.Shared.Interfaces;

namespace Freightmatic.Domain.News
{
    public class News : EntityBase<string>, IAggregateRoot
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public NewsKeyEnum Key { get; set; }
    }
}
