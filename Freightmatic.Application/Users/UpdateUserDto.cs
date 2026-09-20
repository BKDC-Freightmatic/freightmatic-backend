using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Application.Users;

public class UpdateUserDto
{
    public string id { get; set; }
    public string Username { get; set; }
    public string Avatar { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? Address { get; set; }
    public string? OtherAddress { get; set; }
    public IdentityCardDto? IdentityCard { get; set; }
    public DriverLicenseDto? DriverLicense { get; set; }
    public TruckDto? Truck { get; set; }
    public string? DriveHistory { get; set; }
    public DeliveryCategoryDto? DeliveryCategory { get; set; }
    public WorkingTimeDto? WorkingTime { get; set; }
    public string? MostActiveRegion { get; set; }
    public double DeliveryPrice { get; set; }
}
