using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Application.Delivery
{
    public class CreateDeliveryDto
    {
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Description { get; set; }
        public string ProductCategoryId { get; set; }
        public PickupCategoryEnum PickupCategory { get; set; }
        public DateTime PickupTime { get; set; }
        public ICollection<string> ProductImages { get; set; }
        public string DeliveryInsuranceId { get; set; }
        public string TruckerId { get; set; }
        public LocationDto PickupLocation { get; set; }
        public LocationDto DeliveryLocation { get; set; }
    }
}
