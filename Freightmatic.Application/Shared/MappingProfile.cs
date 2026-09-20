using Freightmatic.Domain.Users;
using AutoMapper;
using Freightmatic.Application.Users;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Application.News;
using Freightmatic.Domain.Notifications;
using Freightmatic.Application.Notifications;
using Freightmatic.Application.Delivery;
using Freightmatic.Application.Files;
using Freightmatic.Domain.Deliveries;

namespace Freightmatic.Application.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString())); // Convert enum to string

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => Enum.Parse(typeof(UserTypeEnum), src.UserType))); // Convert string to enum

            CreateMap<IdentityCardDto, IdentityCard>();
            CreateMap<IdentityCard, IdentityCardDto>();
            CreateMap<DriverLicenseDto, DriverLicense>();
            CreateMap<DriverLicense, DriverLicenseDto>();

            CreateMap<TruckDto, Truck>();
            CreateMap<Truck, TruckDto>();

            CreateMap<DeliveryCategoryDto, DeliveryCategory>();
            CreateMap<DeliveryCategory, DeliveryCategoryDto>();

            CreateMap<WorkingTimeDto, WorkingTime>();
            CreateMap<WorkingTime, WorkingTimeDto>();

            CreateMap<Domain.News.News, NewsDto>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key.ToString())); // Convert enum to string;
            CreateMap<NewsDto, Domain.News.News>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => Enum.Parse(typeof(NewsKeyEnum), src.Key))); // Convert string to enum

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key.ToString()));
            CreateMap<NotificationDto, Notification>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => Enum.Parse(typeof(NotificationKeyEnum), src.Key)));

            CreateMap<CreateDeliveryDto, Domain.Deliveries.Delivery>();
            CreateMap<Domain.Deliveries.Delivery, CreateDeliveryDto>();
            
            CreateMap<DeliveryDto, Domain.Deliveries.Delivery>()
                .ForMember(dest => dest.PickupCategory, opt => opt.MapFrom(src => Enum.Parse(typeof(PickupCategoryEnum), src.PickupCategory)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(DeliveryStatus), src.Status)));

            CreateMap<Domain.Deliveries.Delivery, DeliveryDto>()
                 .ForMember(dest => dest.PickupCategory, opt => opt.MapFrom(src => src.PickupCategory.ToString()))
                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
                 
            CreateMap<DeliveryStep, DeliveryStepDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<DeliveryStepDto,  DeliveryStep>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse(typeof(DeliveryStepStatus), src.Status)));

            CreateMap<LocationDto, Location>();
            CreateMap<Location, LocationDto>();

            CreateMap<FileDto, Domain.Files.File>();
            CreateMap<Domain.Files.File, FileDto>();
        }
    }
}