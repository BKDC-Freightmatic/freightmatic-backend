using Freightmatic.Domain.Shared.Enums;

namespace Freightmatic.Domain.Deliveries;

public class DeliveryStep
{
    public int Step { get; set; }
    public string Title { get; set; }
    public DeliveryStepStatus Status { get; set; }
    public List<string> Files { get; set; }
    public string Description { get; set; }
}
