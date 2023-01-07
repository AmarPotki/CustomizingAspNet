namespace CustomizingAspNet.HostedServices.Caching
{
    public interface IDistributedCacheFactory
    {
        IDistributedCache<T> GetCache<T>();
    }
}
