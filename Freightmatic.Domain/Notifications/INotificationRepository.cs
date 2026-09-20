namespace Freightmatic.Domain.Notifications;

public interface INotificationRepository
{
    Task<List<Notification?>> GetAllAsync();
}
