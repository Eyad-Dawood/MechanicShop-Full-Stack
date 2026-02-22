using MechanicShop.Application.Common.Constants.Caching;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : ICachedQuery<Result<CustomerDto>>
{
    public string CacheKey => CustomersCachingConstants.CustomerCachKey(CustomerId);
    public string[] Tags => [CustomersCachingConstants.CustomerCachTag];
    public TimeSpan Expiration => CustomersCachingConstants.CustomerCachingExpiration;
}