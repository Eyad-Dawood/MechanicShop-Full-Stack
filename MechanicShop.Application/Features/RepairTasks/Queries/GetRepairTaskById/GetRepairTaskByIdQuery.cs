namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;

public sealed record GetRepairTaskByIdQuery(Guid RepairTaskId) : ICachedQuery<Result<RepairTaskDto>>
{
    public string CacheKey => RepairTaskCachingConstants.repairTaskCachKey(RepairTaskId);

    public TimeSpan Expiration => RepairTaskCachingConstants.repairTaskCachingExpiration;

    public string[] Tags => [RepairTaskCachingConstants.repairTaskCachTag];
}