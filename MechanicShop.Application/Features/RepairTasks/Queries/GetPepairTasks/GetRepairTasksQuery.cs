namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTasks;

public sealed record GetRepairTasksQuery() : ICachedQuery<Result<List<RepairTaskDto>>>
{
    public string CacheKey => RepairTaskCachingConstants.repairTasksCachKey;

    public TimeSpan Expiration => RepairTaskCachingConstants.repairTaskCachingExpiration;

    public string[] Tags => [RepairTaskCachingConstants.repairTaskCachTag];
}