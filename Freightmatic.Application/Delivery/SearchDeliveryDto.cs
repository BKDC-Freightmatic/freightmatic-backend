namespace Freightmatic.Application.Delivery
{
    public class SearchDeliveryDto
    {
        public string SearchText { get; set; }
        public string OrderDirection { get; set; }
        public string OrderFieldName { get; set; }
        public string SearchFieldName { get; set; }
        public string SearchFieldValue { get; set; }
    }
}
