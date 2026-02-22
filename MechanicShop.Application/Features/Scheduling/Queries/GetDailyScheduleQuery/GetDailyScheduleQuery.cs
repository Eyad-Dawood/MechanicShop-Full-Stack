namespace MechanicShop.Application.Features.Scheduling.Queries.GetDailyScheduleQuery;

public sealed record GetDailyScheduleQuery(
    TimeZoneInfo TimeZone,
    DateOnly ScheduleDate,
    Guid? LaborId = null) : ICachedQuery<Result<ScheduleDto>>
{
    public string CacheKey => SchedulCachingConstants.workOrderCachKey(ScheduleDate,LaborId!.Value);
    public string[] Tags => ["work-order"];
    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}