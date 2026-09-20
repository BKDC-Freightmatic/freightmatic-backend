using Freightmatic.Domain.Shared;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Domain.Shared.Interfaces;

namespace Freightmatic.Domain.Notifications;
public class Notification: EntityBase<string>, IAggregateRoot
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public NotificationKeyEnum Key { get; set; }
    public string Value { get; set; }
}
