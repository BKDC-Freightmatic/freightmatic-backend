namespace Freightmatic.Domain.Shared;

public abstract class EntityBase<TKey> : Audit<TKey>
{
    public TKey id { get; set; }
    public string partitionKey { get => "feightmatic"; }
}