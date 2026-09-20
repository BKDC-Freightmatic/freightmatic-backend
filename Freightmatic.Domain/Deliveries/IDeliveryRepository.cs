namespace Freightmatic.Domain.Deliveries;
public interface IDeliveryRepository
{
    Task<List<Delivery>> GetAllAsync();
    Task<bool> CreateDeliveryAsync(Delivery delivery); 
    Task<Delivery> GetDeliveryAsync(string deliveryId);
    Task<bool> UpdateDeliveryAsync(Delivery delivery);
}
