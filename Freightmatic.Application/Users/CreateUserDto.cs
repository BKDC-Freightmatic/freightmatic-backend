using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Application.Users;

public class CreateUserDto
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public string? Address { get; set; }
    public string? OtherAddress { get; set; }
    public UserTypeEnum UserType { get; set; }
    public IdentityCardDto? IdentityCard { get; set; }
    public DriverLicenseDto? DriverLicense { get; set; }
    public TruckDto? Truck { get; set; }
    public string? DriveHistory { get; set; }
    public DeliveryCategoryDto? DeliveryCategory { get; set; }
    public WorkingTimeDto? WorkingTime { get; set; }
    public string? MostActiveRegion { get; set; }
}
