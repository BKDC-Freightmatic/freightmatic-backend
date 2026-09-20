using System.Collections.Generic;

namespace Freightmatic.Application.Delivery
{
    public class UpdateDeliveryStepDto
    {
        public int Step { get; set; }
        public string Status { get; set; }
        public List<string> Files { get; set; }
        public string Description { get; set; }
    }
}
