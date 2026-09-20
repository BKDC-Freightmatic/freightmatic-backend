using Freightmatic.Application.Delivery;
using Freightmatic.Application.Users;
using Freightmatic.Domain.Deliveries;

namespace Freightmatic.Application.News;

public class DeliveryDto
{
    public string Id { get; set; }
    public float Weight { get; set; }
    public float Width { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public string ProductCategoryId { get; set; }
    public string PickupCategory { get; set; }
    public DateTime PickupTime { get; set; }
    public ICollection<string> ProductImages { get; set; }
    public string DeliveryInsuranceId { get; set; }
    public UserDto Trucker { get; set; }
    public string Description { get; set; }
    public string ShippingCode { get; set; }
    public double Price { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public ICollection<DeliveryStepDto> Steps { get; set; }
    public string Status { get; set; }
    public LocationDto PickupLocation { get; set; }
    public LocationDto DeliveryLocation { get; set; }
}
