using Freightmatic.Domain.Shared;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Domain.Shared.Interfaces;

namespace Freightmatic.Domain.Deliveries
{
    public class Delivery : EntityBase<string>, IAggregateRoot
    {
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string ProductCategoryId { get; set; }
        public PickupCategoryEnum PickupCategory { get; set; }
        public DateTime PickupTime { get; set; }
        public ICollection<string> ProductImages { get; set; }
        public string DeliveryInsuranceId { get; set; }
        public string TruckerId { get; set; }
        public string ShippingCode { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public ICollection<DeliveryStep> Steps { get; set; }
        public DeliveryStatus Status { get; set; }
        public int Progress { get; set; }
        public Location PickupLocation { get; set; }
        public Location DeliveryLocation { get; set; }
    }
}
