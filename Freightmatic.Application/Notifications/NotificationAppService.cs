using AutoMapper;
using Freightmatic.Domain.Notifications;

namespace Freightmatic.Application.Notifications
{
    public class NotificationAppService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationAppService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> GetAll()
        {
            List<Notification?> notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(x => _mapper.Map<NotificationDto>(x));
        }
    }
}
