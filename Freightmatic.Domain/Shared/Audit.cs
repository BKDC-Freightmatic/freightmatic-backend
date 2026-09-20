namespace Freightmatic.Domain.Shared
{
    public abstract class Audit<TKey>
    {
        public TKey createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public TKey modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }
}
