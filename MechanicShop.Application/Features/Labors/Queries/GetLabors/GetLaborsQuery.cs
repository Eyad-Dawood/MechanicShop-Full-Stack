
namespace MechanicShop.Application.Features.Labors.Queries.GetLabors;

public sealed record GetLaborsQuery() : ICachedQuery<Result<List<LaborDto>>>
{
    public string CacheKey => LaborsCachingConstants.laborsCachKey;
    public string[] Tags => [LaborsCachingConstants.laborCachTag];

    public TimeSpan Expiration => LaborsCachingConstants.laborCachingExpiration;
}