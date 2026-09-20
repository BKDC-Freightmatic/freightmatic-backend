using System.Net;
using AutoMapper;
using Freightmatic.Application.Delivery;
using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Freightmatic.Domain.Deliveries;
using Freightmatic.Domain.Shared.Enums;
using Freightmatic.Domain.Users;

namespace Freightmatic.Application.News;
public class DeliveryAppService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DeliveryAppService(IMapper mapper, IDeliveryRepository deliveryRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _deliveryRepository = deliveryRepository;
        _mapper = mapper;
    }

    public async Task<DeliveryDto> Create(string userId, CreateDeliveryDto dto)
    {
        var trucker = await _userRepository.GetUserByIdAsync(dto.TruckerId);

        if (trucker == null)
            throw new CustomException("Trucker does not exist");

        Domain.Deliveries.Delivery delivery = new()
        {
            id = Guid.NewGuid().ToString(),
            Weight = dto.Weight,
            Width = dto.Width,
            Length = dto.Length,
            Height = dto.Height,
            ProductCategoryId = dto.ProductCategoryId,
            PickupCategory = dto.PickupCategory,
            PickupTime = dto.PickupTime,
            ProductImages = dto.ProductImages,
            DeliveryInsuranceId = dto.DeliveryInsuranceId,
            TruckerId = dto.TruckerId,
            Price = trucker.DeliveryPrice,
            Description = dto.Description,
            ShippingCode = dto.PickupCategory == PickupCategoryEnum.NOW ? "NOW" + GenerateRandomNumber() : "SCH" + GenerateRandomNumber(),
            createdBy = userId,
            createdOn = DateTime.UtcNow,
            modifiedBy = userId,
            modifiedOn = DateTime.UtcNow,
            Steps = [
                new DeliveryStep() {
                    Step = 1,
                    Title = "Pick up",
                    Status = DeliveryStepStatus.Waiting
                },
                new DeliveryStep() {
                    Step = 2,
                    Title = "Drop off",
                    Status = DeliveryStepStatus.Waiting
                }
            ],
            PickupLocation = _mapper.Map<Location>(dto.PickupLocation),
            DeliveryLocation = _mapper.Map<Location>(dto.DeliveryLocation),
            Status = DeliveryStatus.NA,
        };

        bool isSuccess = await _deliveryRepository.CreateDeliveryAsync(delivery);
        if (!isSuccess)
            throw new CustomException("Error when create new delivery");

        DeliveryDto deliveryDto = _mapper.Map<DeliveryDto>(delivery);
        deliveryDto.Trucker = _mapper.Map<UserDto>(trucker);

        return deliveryDto;
    }

    public async Task<DeliveryDto> GetById(string deliveryId)
    {
        Domain.Deliveries.Delivery delivery = await _deliveryRepository.GetDeliveryAsync(deliveryId);
        var trucker = await _userRepository.GetUserByIdAsync(delivery.TruckerId);

        if (trucker == null)
            throw new CustomException("Trucker does not exist");
            
        DeliveryDto deliveryDto = _mapper.Map<DeliveryDto>(delivery);
        deliveryDto.Trucker = _mapper.Map<UserDto>(trucker);
        return deliveryDto;
    }

    public async Task<IEnumerable<DeliveryDto>> Search(SearchDeliveryDto searchDto)
    {
        List<Domain.Deliveries.Delivery> deliveries = await _deliveryRepository.GetAllAsync();

        // Filter by SearchText (description contains)
        if (!string.IsNullOrWhiteSpace(searchDto.SearchText))
        {
            deliveries = deliveries.Where(d => d.Description != null && d.Description.Contains(searchDto.SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter by SearchFieldName and SearchFieldValue
        if (!string.IsNullOrWhiteSpace(searchDto.SearchFieldName) && !string.IsNullOrWhiteSpace(searchDto.SearchFieldValue))
        {
            deliveries = FilterByField(deliveries, searchDto.SearchFieldName, searchDto.SearchFieldValue);
        }

        // Order by OrderFieldName and OrderDirection
        if (!string.IsNullOrWhiteSpace(searchDto.OrderFieldName))
        {
            deliveries = OrderByField(deliveries, searchDto.OrderFieldName, searchDto.OrderDirection);
        }

        return deliveries.Select(x => _mapper.Map<DeliveryDto>(x));
    }

    private string GenerateRandomNumber()
    {
        Random random = new Random();
        return string.Join("", Enumerable.Range(0, 9).Select(_ => random.Next(0, 10).ToString()));
    }

    private List<Domain.Deliveries.Delivery> FilterByField(List<Domain.Deliveries.Delivery> deliveries, string fieldName, string fieldValue)
    {
        return deliveries.Where(d =>
        {
            var prop = typeof(Domain.Deliveries.Delivery).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));
            if (prop == null) return false;

            var value = prop.GetValue(d)?.ToString();
            return value != null && value.Contains(fieldValue, StringComparison.OrdinalIgnoreCase);
        }).ToList();
    }

    private List<Domain.Deliveries.Delivery> OrderByField(List<Domain.Deliveries.Delivery> deliveries, string fieldName, string direction)
    {
        var prop = typeof(Domain.Deliveries.Delivery).GetProperties()
            .FirstOrDefault(p => string.Equals(p.Name, fieldName, StringComparison.OrdinalIgnoreCase));
        if (prop == null) return deliveries;

        var ordered = direction.ToLower() == "desc"
            ? deliveries.OrderByDescending(d => prop.GetValue(d))
            : deliveries.OrderBy(d => prop.GetValue(d));

        return ordered.ToList();
    }

    public async Task UpdateDeliveryStatus(string deliveryId, string status)
    {
        var delivery = await _deliveryRepository.GetDeliveryAsync(deliveryId);
        if (delivery == null)
            throw new CustomException("Delivery not found", "Not Found", HttpStatusCode.NotFound);

        if (!Enum.TryParse<DeliveryStatus>(status, out var newStatus))
            throw new CustomException("Invalid status", "Bad Request", HttpStatusCode.BadRequest);

        delivery.Status = newStatus;
        bool isSuccess = await _deliveryRepository.UpdateDeliveryAsync(delivery);
        if (!isSuccess)
            throw new CustomException("Error when update delivery status", "Bad Request", HttpStatusCode.BadRequest);
    }

    public async Task UpdateDeliveryStep(string deliveryId, UpdateDeliveryStepDto dto)
    {
        var delivery = await _deliveryRepository.GetDeliveryAsync(deliveryId);
        if (delivery == null)
            throw new CustomException("Delivery not found", "Not Found", HttpStatusCode.NotFound);

        var step = delivery.Steps.FirstOrDefault(s => s.Step == dto.Step);
        if (step == null)
            throw new CustomException("Step not found", "Not Found", HttpStatusCode.NotFound);

        if (!Enum.TryParse<DeliveryStepStatus>(dto.Status, out var newStatus))
            throw new CustomException("Invalid status", "Bad Request", HttpStatusCode.BadRequest);

        step.Status = newStatus;
        step.Files = dto.Files;
        step.Description = dto.Description;

        bool isSuccess = await _deliveryRepository.UpdateDeliveryAsync(delivery);
        if (!isSuccess)
            throw new CustomException("Error when update delivery step", "Bad Request", HttpStatusCode.BadRequest);
    }
}
