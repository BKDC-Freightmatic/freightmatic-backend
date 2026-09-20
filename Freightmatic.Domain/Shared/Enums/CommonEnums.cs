namespace Freightmatic.Domain.Shared.Enums
{
    public struct ProductCategory
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public ProductCategory(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }

    public struct DeliveryInsurance
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public DeliveryInsurance(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
