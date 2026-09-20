using Freightmatic.Domain.Shared;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Domain.Shared.Interfaces;
using System.Diagnostics;

namespace Freightmatic.Domain.Users;

public class User : EntityBase<string>, IAggregateRoot
{
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public string? Address { get; set; }
    public string? OtherAddress { get; set; }
    public UserTypeEnum UserType { get; set; }

    public IdentityCard? IdentityCard { get; set; }
    public DriverLicense? DriverLicense { get; set; }
    public Truck? Truck { get; set; }
    public string? DriveHistory { get; set; }
    public DeliveryCategory? DeliveryCategory { get; set; }
    public WorkingTime? WorkingTime { get; set; }
    public string? MostActiveRegion { get; set; }
    public float Rating { get; set; }
    public double DeliveryPrice { get; set; }

    public User() { }
    public User(
        string id,
        string userName,
        string name,
        string email,
        string phoneNumber,
        UserTypeEnum userType,
        string? address,
        string? otherAddress,
        IdentityCard? identityCard = null,
        DriverLicense? driverLicense = null,
        Truck? truck = null,
        string? driveHistory = null,
        DeliveryCategory? deliveryCategory = null,
        WorkingTime? workingTime = null,
        string? mostActiveRegion = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            this.id = Guid.NewGuid().ToString();
        else
            this.id = id;

        UserName = userName;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        UserType = userType;
        Address = address;
        OtherAddress = otherAddress;
        IdentityCard = identityCard;
        DriverLicense = driverLicense;
        Truck = truck;
        DriveHistory = driveHistory;
        DeliveryCategory = deliveryCategory;
        WorkingTime = workingTime;
        MostActiveRegion = mostActiveRegion;
    }

    public User(
        string id,
        string userName,
        string name,
        string email,
        string phoneNumber,
        UserTypeEnum userType,
        string? address,
        string? otherAddress)
    {
        if (string.IsNullOrWhiteSpace(id))
            this.id = Guid.NewGuid().ToString();
        else
            this.id = id;

        UserName = userName;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        UserType = userType;
        Address = address;
        OtherAddress = otherAddress;
    }
}
public class IdentityCard
{
    public string Number { get; set; }
    public string Region { get; set; }
    public List<string> Images { get; set; }

    public IdentityCard(string number, string region, List<string> images)
    {
        Number = number;
        Region = region;
        Images = images;
    }
}
public class DriverLicense
{
    public string Number { get; set; }
    public string Region { get; set; }
    public List<string> Images { get; set; }

    public DriverLicense(string number, string region, List<string> images)
    {
        Number = number;
        Region = region;
        Images = images;
    }
}
public class Truck
{
    public string ModelName { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double MaximumWeight { get; set; }
    public string Note { get; set; }

    public Truck(string modelName, DateTime releaseDate, double maximumWeight, string note)
    {
        ModelName = modelName;
        ReleaseDate = releaseDate;
        MaximumWeight = maximumWeight;
        Note = note;
    }
}
public class DeliveryCategory
{
    public bool AllowOutOfBoundary { get; set; }
    public bool AllowLongHaul { get; set; }
    public bool AllowShortHaul { get; set; }
    public bool AllowNow { get; set; }
    public bool AllowSchedule { get; set; }

    public DeliveryCategory(bool allowOutOfBoundary, bool allowLongHaul, bool allowShortHaul, bool allowNow, bool allowSchedule)
    {
        AllowOutOfBoundary = allowOutOfBoundary;
        AllowLongHaul = allowLongHaul;
        AllowShortHaul = allowShortHaul;
        AllowNow = allowNow;
        AllowSchedule = allowSchedule;
    }
}
public class WorkingTime
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public TimeSpan BreakTime { get; set; }
    public int DaysPerWeek { get; set; }

    public WorkingTime(TimeSpan startTime, TimeSpan endTime, TimeSpan breakTime, int daysPerWeek)
    {
        StartTime = startTime;
        EndTime = endTime;
        BreakTime = breakTime;
        DaysPerWeek = daysPerWeek;
    }
}