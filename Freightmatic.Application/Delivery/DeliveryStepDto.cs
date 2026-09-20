namespace Freightmatic.Application.Delivery;

public class DeliveryStepDto
{
    public int Step { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    public List<string> Files { get; set; }
    public string Description { get; set; }
}
