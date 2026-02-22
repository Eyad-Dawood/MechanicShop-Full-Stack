using MechanicShop.Application.Common.Constants.Caching;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery : ICachedQuery<Result<List<CustomerDto>>>
{
    public string CacheKey => CustomersCachingConstants.CustomersCachKey;
    public string[] Tags => [CustomersCachingConstants.CustomerCachTag];
    public TimeSpan Expiration => CustomersCachingConstants.CustomerCachingExpiration;
}