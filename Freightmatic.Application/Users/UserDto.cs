using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Application.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? OtherAddress { get; set; }
        public string UserType { get; set; }
        public float Rating { get; set; }
        public string Avatar { get; set; }
        public double DeliveryPrice { get; set; }

        public IdentityCardDto? IdentityCard { get; set; }
        public DriverLicenseDto? DriverLicense { get; set; }
        public TruckDto? Truck { get; set; }
        public string? DriveHistory { get; set; }
        public DeliveryCategoryDto? DeliveryCategory { get; set; }
        public WorkingTimeDto? WorkingTime { get; set; }
        public string? MostActiveRegion { get; set; }
    }

    public class IdentityCardDto
    {
        public string Number { get; set; }
        public string Region { get; set; }
        public List<string> Images { get; set; }
    }

    public class DriverLicenseDto
    {
        public string Number { get; set; }
        public string Region { get; set; }
        public List<string> Images { get; set; }
    }

    public class TruckDto
    {
        public string ModelName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double MaximumWeight { get; set; }
        public string Note { get; set; }
    }

    public class DeliveryCategoryDto
    {
        public bool AllowOutOfBoundary { get; set; }
        public bool AllowLongHaul { get; set; }
        public bool AllowShortHaul { get; set; }
        public bool AllowNow { get; set; }
        public bool AllowSchedule { get; set; }
    }

    public class WorkingTimeDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan BreakTime { get; set; }
        public int DaysPerWeek { get; set; }
    }
}